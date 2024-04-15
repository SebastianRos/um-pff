using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellularAutomaton : MonoBehaviour
{
    [Range(0, 8)]
    public int deathLimit = 4;
    [Range(0, 8)]
    public int birthLimit = 3;

    public bool[][] modify_map(bool[][] cellmap, int numberOfSteps)
    {
        //And now run the simulation for a set number of steps
        for(int i = 0; i < numberOfSteps; i++)
        {
            cellmap = doSimulationStep(cellmap);
        }

        return cellmap;
    }


    public bool[][] initialiseMap(bool[][] map, int width, int height, float chanceToStartAlive){

        for(int x=0; x<width; x++){
            for(int y=0; y<height; y++){
                if (Random.value < chanceToStartAlive)
                {
                    map[x][y] = true;
                }
            }
        }

        return map;

    }


    public int countAliveNeighbours(bool[][] map, int x, int y)
    {
        int count = 0;
        for(int i=-1; i<2; i++){
            for(int j=-1; j<2; j++){
                int neighbour_x = x+i;
                int neighbour_y = y+j;

                //If we're looking at the middle point

                if(i == 0 && j == 0){
                    //Do nothing, we don't want to add ourselves in!
                }

                //In case the index we're looking at it off the edge of the map
                else if(neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= map.Length || neighbour_y >= map[0].Length){
                    //Do nothing, we don't want to add out of bounds
                }

                //Otherwise, a normal check of the neighbour
                else if(map[neighbour_x][neighbour_y]){
                    count = count + 1;
                }
            }
        }

        return count;
    }

    public bool[][] doSimulationStep(bool[][] oldMap)
    {
        int width = oldMap.Length;
        int height = oldMap[0].Length;

        bool[][] newMap = new bool[width][];
        for (int i = 0; i < width; i++)
        {
            newMap[i] = new bool[height];
        }

        //Loop over each row and column of the map
        for (int x = 0; x < oldMap.Length; x++)
        {
            for (int y = 0; y < oldMap[0].Length; y++)
            {
                int nbs = countAliveNeighbours(oldMap, x, y);

                //The new value is based on our simulation rules
                //First, if a cell is alive but has too few neighbours, kill it.
                if (oldMap[x][y])
                {
                    if (nbs < deathLimit)
                    {
                        newMap[x][y] = false;
                    }
                    else
                    {
                        newMap[x][y] = true;
                    }
                }
                //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
                else
                {
                    if (nbs > birthLimit)
                    {
                        newMap[x][y] = true;
                    }
                    else
                    {
                        newMap[x][y] = false;
                    }
                }
            }
        }

        return newMap;
    }

}