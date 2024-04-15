using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using DelaunatorSharp;
using System.Linq;
using DelaunatorSharp.Unity.Extensions;
using Unity.Collections;
using UnityEditor.Tilemaps;

public class CellarGenerator : MonoBehaviour
{
    public Tile floorTile;
    public RuleTile wallTile;
    public Tilemap floorTiles;
    public Tilemap wallTiles;

    public playercontroller playerPrefab;
    public ToastBahaviour breadPrefab;
    public DuckBrain duckPrefab;
    public GameObject exitPrefab;
    public EvilGuy[] enemies;

    private playercontroller player;


    public Vector2Int RoomWidthRange = new Vector2Int(10, 20);
    public Vector2Int RoomHeightRange = new Vector2Int(10, 20);

    [Range(0.0f, 0.99f)]
    public float ChanceOfBread = 0.5f;

    [Range(0.0f, 0.99f)]
    public float ChanceOfDucks = 0.75f;

    [Range(0.0f, 0.99f)]
    public float ChanceOfEnemies = 0.75f;
    [Range(0, 10)]
    public int MaxEnemiesPerRoom = 5;



    List<Room> levelRooms;


    private List<IPoint> points = new List<IPoint>();
    void Start() {
        this.levelRooms = new List<Room>();
        levelRooms.Add(new Room(
            7,
            7,
            new Vector2(0, 0),
            this
        ));
        for (int i = 1; i < 7; i++) {
            float w = 0;
            for (int k = 0; k < 3; k++) {
                w += RollDice(RoomWidthRange.x, RoomWidthRange.y);
            }
            w /= 3;
            float h = 0;
            for (int k = 0; k < 3; k++) {
                h += RollDice(RoomHeightRange.x, RoomHeightRange.y);
            }
            h /= 3;

            levelRooms.Add(new Room(
                (int) math.round(w), 
                (int) math.round(h),
                new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)),
                this
            ));
        }


        bool didMove = MoveRooms(this.levelRooms);
        int j;
        for(j = 0; j < 100 && didMove; j++) {
            didMove = MoveRooms(this.levelRooms);
        }
        Debug.Log(j);


        foreach(Room room in levelRooms) {
            room.position = Vector2Int.RoundToInt(room.position);
            points.Add(new Point(room.position.x, room.position.y));
        }
        Delaunator delaunator = new Delaunator(points.ToArray());
        List<IEdge> edges = delaunator.GetEdges().ToList();


        edges.Sort((a, b) => EdgeLength(a) > EdgeLength(b) ? 1 : -1);

        HashSet<IPoint> visited = new HashSet<IPoint>();
        List<IEdge> mst = new List<IEdge>();

        IPoint startPoint = edges[0].P;
        visited.Add(startPoint);

        foreach (IEdge edge in edges)
        {
            IPoint p1 = edge.P;
            IPoint p2 = edge.Q;

            if (visited.Contains(p1) && !visited.Contains(p2))
            {
                mst.Add(edge);
                visited.Add(p2);
            }
            else if (!visited.Contains(p1) && visited.Contains(p2))
            {
                mst.Add(edge);
                visited.Add(p1);
            }
        }

        foreach(Room room in levelRooms) {
            Vector2Int roomSize = new Vector2Int(room.width, room.height);
            Vector2Int minPos = Vector2Int.FloorToInt(room.position - roomSize/2);
            Vector2Int maxPos = Vector2Int.CeilToInt(room.position + roomSize/2);

            for(int x = minPos.x; x < maxPos.x; x++) {
                for(int y = minPos.y; y < maxPos.y; y++) {
                    floorTiles.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }

        foreach (IEdge edge in mst)
        {
            Vector3 start = edge.P.ToVector3();
            Vector3 end = edge.Q.ToVector3();
            Vector3 distance = end - start;
            float r = 1.5f;


            for(float t = 0.0f; t <= 1.0f; t += 0.001f) {
                Vector3 center = start + t * distance;

                for (int x = (int)math.ceil(center.x - r); x <= (int)math.floor(center.x + r); x++)
                {
                    for (int y = (int)math.ceil(center.y - r); y <= (int)math.floor(center.y + r); y++)
                    {
                        floorTiles.SetTile(new Vector3Int(x, y, 0), floorTile);
                    }
                }
            }
        }

        Vector2Int tileStart = (Vector2Int) floorTiles.cellBounds.position;
        Vector2Int tileLimit = (Vector2Int) floorTiles.cellBounds.size;
        for(int x = tileStart.x - 1; x < tileLimit.x + 1; x++) {
            for(int y = tileStart.y - 1; y < tileLimit.y + 1; y++) {
                if(checkShouldDrawWall(x, y)) {
                    wallTiles.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        this.player = Instantiate(playerPrefab, this.levelRooms[0].position, Quaternion.identity);
        levelRooms[levelRooms.Count - 1].PopulateExit();
        for(int i = 1; i < levelRooms.Count; i++) {
            levelRooms[i].Populate();
        }
    }

    public bool checkShouldDrawWall(int x, int y)
    {
        TileBase ownTile = floorTiles.GetTile(new Vector3Int(x, y, 0));
        if (ownTile != null) {
            return false;
        }

        for(int i =- 1; i <= 1; i++){
            for(int j =- 1; j <= 1; j++){
                int neighbour_x = x+i;
                int neighbour_y = y+j;

                TileBase tile = floorTiles.GetTile(new Vector3Int(neighbour_x, neighbour_y, 0));
                if(tile != null) {
                    return true;
                }
            }
        }
        return false;
    }

    float EdgeLength(IEdge e) {
        return Vector2.Distance(e.P.ToVector2(), e.Q.ToVector2());
    }

    float RollDice(int min, int max) {
        return UnityEngine.Random.Range(min, max + 1);
    }

    bool MoveRooms(List<Room> rooms) {
        bool moved = false;

        foreach(Room room in rooms) {
            foreach(Room room2 in rooms) {
                if (room != room2) {
                    float distance = Vector2.Distance(room.position, room2.position);
                    if(distance < room.radius + room2.radius) {
                        moved = true;
                        Vector2 direction = (room.position - room2.position).normalized;
                        room.position += direction / 2;
                        room2.position -= direction / 2;
                    }
                }
            }
        }

        return moved;
    }

    void PlaceBread(float x, float y)
    {
        Instantiate(breadPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    void PlaceDuck(float x, float y)
    {
        DuckBrain newDuck = Instantiate(duckPrefab, new Vector3(x, y, 0), Quaternion.identity);
        newDuck.SetPlayer(this.player.transform);
    }

    void PlaceEnemy(float x, float y)
    {
        int r = UnityEngine.Random.Range(0, this.enemies.Length);
        EvilGuy newEnemy = Instantiate(enemies[r], new Vector3(x, y, 0), Quaternion.identity);
        newEnemy.fangirlBehavior.senpai = this.player.transform;
    }

    void PlaceExit(float x, float y)
    {
        Instantiate(exitPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    private class Room {
        public int width;
        public int height;
        public Vector2 position;
        public float radius;
        CellarGenerator Parent;

        public Room(int width, int height, Vector2 position, CellarGenerator parent)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.radius = Mathf.Sqrt(Mathf.Pow(width / 2, 2) + Mathf.Pow(height / 2, 2)) + 0.5f;
            this.Parent = parent;
        }


        List<Vector2> blockedPositions = new List<Vector2>();
        public void Populate() {
            int difficulty = GameManager.instance.currStage;

            this.PopulateBread(difficulty);
            this.PopulateDucks(difficulty);
            this.PopulateEnemies(difficulty);
        }
        
        public void PopulateDucks(int difficulty) {
            // Loose 10% duck chance per level
            while(UnityEngine.Random.value < this.Parent.ChanceOfDucks - difficulty * 0.1) {
                float x = this.position.x - this.width/2 + UnityEngine.Random.Range(0, this.width-1) + 0.5f;
                float y = this.position.y - this.height/2 + UnityEngine.Random.Range(0, this.height-1) + 0.5f;
                
                if(!blockedPositions.Contains(new Vector2(x, y))) {
                    this.blockedPositions.Add(new Vector2(x, y));
                    this.Parent.PlaceDuck( x, y );
                }
            }
        }
        
        public void PopulateBread(int difficulty) {
            // Loose 5% bread chance per level
            while(UnityEngine.Random.value < this.Parent.ChanceOfBread - difficulty * 0.05) {
                float x = this.position.x - this.width/2 + UnityEngine.Random.Range(0, this.width-1) + 0.5f;
                float y = this.position.y - this.height/2 + UnityEngine.Random.Range(0, this.height-1) + 0.5f;
                
                if(!blockedPositions.Contains(new Vector2(x, y))) {
                    this.blockedPositions.Add(new Vector2(x, y));
                    this.Parent.PlaceBread( x, y );
                }
            }
        }
        
        public void PopulateEnemies(int difficulty) {
            // Gain 5% enemy chance per level
            for(int i = 0; i < this.Parent.MaxEnemiesPerRoom && UnityEngine.Random.value < this.Parent.ChanceOfEnemies + 0.05 * difficulty; i++) {
                float x = this.position.x - this.width/2 + UnityEngine.Random.Range(1, this.width-2) + 0.5f;
                float y = this.position.y - this.height/2 + UnityEngine.Random.Range(1, this.height-2) + 0.5f;
                
                if(!blockedPositions.Contains(new Vector2(x, y))) {
                    this.blockedPositions.Add(new Vector2(x, y));
                    this.Parent.PlaceEnemy( x, y );
                }
            }
        }
        
        public void PopulateExit() {
            float x = this.position.x - this.width/2 + UnityEngine.Random.Range(1, this.width-2) + 0.5f;
            float y = this.position.y - this.height/2 + UnityEngine.Random.Range(1, this.height-2) + 0.5f;
            
            while(blockedPositions.Contains(new Vector2(x, y))) {
                x = this.position.x - this.width/2 + UnityEngine.Random.Range(1, this.width-2) + 0.5f;
                y = this.position.y - this.height/2 + UnityEngine.Random.Range(1, this.height-2) + 0.5f;
            }

            this.blockedPositions.Add(new Vector2(x, y));
            this.Parent.PlaceExit( x, y );
        }
    }
}