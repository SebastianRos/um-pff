using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGenerator : MonoBehaviour
{
    public Tile floorTile;
    public Tile wallTile;
    public Tilemap highlightMap;

    public int width = 100;
    public int height = 100;

    [Range(0, 10)]
    public int cellularAutomatonSteps = 4;

    CellularAutomaton cellularAutomaton;
    RandomWalk randomWalk;

    void Awake() {
        cellularAutomaton = GetComponent<CellularAutomaton>();
        randomWalk = GetComponent<RandomWalk>();
    }

    void Update() {
        bool[][] map = generateEmptyMap();
        
        map = randomWalk.modify_map(map);
        map = cellularAutomaton.modify_map(map, cellularAutomatonSteps);


        for(int x=0; x<width; x++){
            for(int y=0; y<height; y++){
                highlightMap.SetTile(new Vector3Int(x, y, 0), map[x][y] ? floorTile : wallTile);
            }
        }

        Debug.Break();
    }

    public bool[][] generateEmptyMap()
    {
        bool[][] map = new bool[width][];

        for(int x=0; x<width; x++){
            map[x] = new bool[height];

            for(int y=0; y<height; y++){
                    map[x][y] = false;
            }
        }

        return map;
    }

}