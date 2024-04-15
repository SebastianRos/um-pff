using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomWalk : MonoBehaviour
{
    public int border = 10;
    public int maxSteps = 10000;
    public int maxWalkers = 10;

    [Range(0.01f, 1f)]
    public float maxFillPercentage = 0.5f;


    [Range(0.01f, 1f)]
    public float chanceToClone = 0.45f;
    [Range(0.01f, 1f)]
    public float chanceToDie = 0.45f;
    [Range(0.01f, 1f)]
    public float chanceToChangeDirection = 0.45f;

    protected int width = 0;
    protected int height = 0;


    public bool[][] modify_map(bool[][] cellmap)
    {
        this.width = cellmap.Length;
        this.height = cellmap[0].Length;


        List<Walker> walkers = new List<Walker>
        {
            new Walker(
                new Vector2Int((int) width / 2, (int) height / 2),
                RandomWalk.GetRandomDirection(),
                this
            ),
            new Walker(
                new Vector2Int((int) width / 2, (int) height / 2),
                RandomWalk.GetRandomDirection(),
                this
            ),
            new Walker(
                new Vector2Int((int) width / 2, (int) height / 2),
                RandomWalk.GetRandomDirection(),
                this
            )
        };


        int steps = 0;
        int filled = 0;
        while (steps < maxSteps && filled < (width-2*border) * (height-2*border) * maxFillPercentage) {
            List<Walker> toRemove = new List<Walker>();
            List<Walker> toAdd = new List<Walker>();

            foreach (Walker w in walkers) {
                Vector2Int pos = w.Step();
                steps++;

                if(!cellmap[pos.x][pos.y]) {
                    filled++;
                    cellmap[pos.x][pos.y] = true;
                }

                if(w.TryClone()) {
                    Debug.Log("Cloned");
                    toAdd.Add(w.Clone());
                }
                if(w.TryUnalive()) {
                    Debug.Log("Died");
                    toRemove.Add(w);
                }
            }

            foreach(Walker w in toRemove) {
                if (walkers.Count == 1) {
                    break;
                }
                walkers.Remove(w);
            }
            foreach(Walker w in toAdd) {
                if (walkers.Count >= maxWalkers) {
                    break;
                }
                walkers.Add(w);
            }
            Debug.Log("Now: " + walkers.Count);
        }


        return cellmap;
    }

    private static Vector2Int[] directions = new Vector2Int[] { 
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
    };

    protected static Vector2Int GetRandomDirection() {
        return RandomWalk.directions[UnityEngine.Random.Range(0, 4)];
    }


    private class Walker
    {
        public Vector2Int Position;
        public Vector2Int Direction;
        private RandomWalk Parent;

        public Walker(Vector2Int pos, Vector2Int dir, RandomWalk parent){
            Position = pos;
            Direction = dir;
            Parent = parent;
        }


        public Vector2Int Step()
        {
            if (UnityEngine.Random.value < Parent.chanceToChangeDirection)
            {
                this.Direction = RandomWalk.GetRandomDirection();
            }

            this.Position += this.Direction;
            this.Position.x = Mathf.Clamp(this.Position.x, Parent.border-1, Parent.width-Parent.border);
            this.Position.y = Mathf.Clamp(this.Position.y, Parent.border-1, Parent.height-Parent.border);

            return this.Position;
        }


        public Walker Clone()
        {
            return new Walker(
                this.Position,
                this.Direction,
                this.Parent
            );
        }


        public bool TryClone()
        {
            return UnityEngine.Random.value < Parent.chanceToClone;
        }


        public bool TryUnalive()
        {
            return UnityEngine.Random.value < Parent.chanceToDie;
        }
    }
}
