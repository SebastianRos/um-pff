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
using System.Data;

public class CellarGenerator : MonoBehaviour
{
    public Tile floorTile;
    public RuleTile wallTile;
    public Tilemap floorTiles;
    public Tilemap wallTiles;

    public ToastBahaviour BreadPrefab;
    public Pond[] PondPrefabs;
    public GameObject ExitPrefab;
    public EvilGuy[] EnemyPrefabs;

    private playercontroller player;


    public Vector2Int RoomWidthRange = new Vector2Int(10, 20);
    public Vector2Int RoomHeightRange = new Vector2Int(10, 20);

    [Range(0.0f, 0.99f)]
    public float ChanceOfBread = 0.5f;

    [Range(0.0f, 0.99f)]
    public float ChanceOfPond = 0.75f;

    [Range(0.0f, 0.99f)]
    public float ChanceOfEnemies = 0.75f;
    [Range(0, 10)]
    public int MaxEnemiesPerRoom = 5;



    List<Room> levelRooms;


    private List<IPoint> points = new List<IPoint>();
    void Start() {
        this.levelRooms = new List<Room>();

        // Create "Room 0" with a fixed size
        levelRooms.Add(new Room(
            6,
            6,
            new Vector2(0, 0),
            this,
            true
        ));
        
        // Create a number of rooms based on stage number
        for (int i = 1; i <= 2 + GameManager.instance.currStage; i++) {
            float w = RollNDice(RoomWidthRange.x, RoomWidthRange.y, 3) / 3;
            float h = RollNDice(RoomHeightRange.x, RoomHeightRange.y, 3) / 3;

            levelRooms.Add(new Room(
                (int) math.round(w), 
                (int) math.round(h),
                new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)),
                this
            ));
        }


        // Move rooms away from each other
        bool didMove = MoveRooms(this.levelRooms);
        int j;
        for(j = 0; j < 100 && didMove; j++) {
            didMove = MoveRooms(this.levelRooms);
        }
        Debug.Log("Room moves: " + j);


        // Generate paths between rooms
        foreach(Room room in levelRooms) {
            Debug.Log("Room pos " + room.position);
            room.position = Vector2Int.RoundToInt(room.position);
            points.Add(new Point(room.position.x, room.position.y));
        }
        Delaunator delaunator = new Delaunator(points.ToArray());
        List<IEdge> edges = delaunator.GetEdges().ToList();

        // Generate minimum spanning tree to determine paths
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


        // Draw rooms in tilemap
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

        // Draw paths in tilemap
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

        // Draw walls around tiles
        Vector2Int tileStart = (Vector2Int) floorTiles.cellBounds.position;
        Vector2Int tileLimit = (Vector2Int) floorTiles.cellBounds.size;
        for(int x = tileStart.x - 1; x < tileLimit.x + 1; x++) {
            for(int y = tileStart.y - 1; y < tileLimit.y + 1; y++) {
                if(checkShouldDrawWall(x, y)) {
                    wallTiles.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        // Create player in scene
        this.player = GameObject.Find("Player").GetComponent<playercontroller>();
        this.player.transform.position = this.levelRooms[0].position;

        // Generate the exit in the room furthest from the player
        float maxDist = 0;
        Room maxDistRoom = null;
        foreach(Room room in levelRooms) {
            float dist = Vector2.Distance(room.position, player.transform.position);
            if(dist > maxDist) {
                maxDist = dist;
                maxDistRoom = room;
            }
        }
        maxDistRoom.PopulateExit();

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

    float RollNDice(int min, int max, int N) {
        int r = 0;
        for (int k = 0; k < 3; k++) {
            r += UnityEngine.Random.Range(min, max + 1);
        }
        return r;
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
                        if(!room.isStatic)
                            room.position += direction / 2;
                        if(!room2.isStatic)
                            room2.position -= direction / 2;
                    }
                }
            }
        }

        return moved;
    }

    void PlaceBread(float x, float y)
    {
        Instantiate(BreadPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    void PlacePond(float x, float y)
    {
        int r = UnityEngine.Random.Range(0, this.PondPrefabs.Length);
        Instantiate(PondPrefabs[r], new Vector3(x, y, 0), Quaternion.identity);
    }

    void PlaceEnemy(float x, float y)
    {
        int r = UnityEngine.Random.Range(0, this.EnemyPrefabs.Length);
        EvilGuy newEnemy = Instantiate(EnemyPrefabs[r], new Vector3(x, y, 0), Quaternion.identity);
        newEnemy.fangirlBehavior.senpai = this.player.transform;
    }

    void PlaceExit(float x, float y)
    {
        Instantiate(ExitPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    private class Room {
        public int width;
        public int height;
        public bool isStatic;
        public Vector2 position;
        public float radius;
        CellarGenerator Parent;

        public Room(int width, int height, Vector2 position, CellarGenerator parent, bool isStatic = false)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.radius = Mathf.Sqrt(Mathf.Pow(width / 2, 2) + Mathf.Pow(height / 2, 2)) + 0.5f;
            this.Parent = parent;
            this.isStatic = isStatic;
        }


        List<Vector2> blockedPositions = new List<Vector2>();
        public void Populate() {
            int difficulty = GameManager.instance.currStage;

            this.PopulateBread(difficulty);
            this.PopulatePonds(difficulty);
            this.PopulateEnemies(difficulty);
        }
        
        public void PopulatePonds(int difficulty) {
            // Loose 10% duck chance per level
            float r = UnityEngine.Random.value;
            if(r < this.Parent.ChanceOfPond - difficulty * 0.05) {
                float x = this.position.x - this.width/2 + UnityEngine.Random.Range(0, this.width-1) + 0.5f;
                float y = this.position.y - this.height/2 + UnityEngine.Random.Range(0, this.height-1) + 0.5f;
                
                if(!blockedPositions.Contains(new Vector2(x, y))) {
                    this.blockedPositions.Add(new Vector2(x, y));
                    this.Parent.PlacePond( x, y );
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