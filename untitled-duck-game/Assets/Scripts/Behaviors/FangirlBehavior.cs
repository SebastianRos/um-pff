using System;
using System.Collections.Generic;
using UnityEngine;

public class FangirlBehavior: MonoBehaviour
{
    [Range(0.1f, 5f)]
    public float aStarDistance = 1.0f;

    [Range(0.1f, 5f)]
    public float collisionPreventionRadius = 1.0f;
    public bool stopOnTouch = true;

    [Range(1f, 15f)]
    public float aStarMaxDistance = 5.0f;
    
    public Transform senpai;

    [Range(0.0f, 10f)]
    public float speed = 5f;

    public bool debug = false;


    public LayerMask blockingLayers;

    public float stopAtDistToTarget = 0.0f;

    void Awake()
    {
        if (senpai == null)
        {
            senpai = GameObject.Find("Player").transform;
        }
    }

    public Vector2 CalculateDirection()
    {
        if(debug) 
        {
            Debug.DrawLine(transform.position, senpai.position, Color.blue);
            CustomDebug.DrawCircle(transform.position, collisionPreventionRadius, 16, Color.red);
        }

        if (senpai != null)
        {
            Vector2[] path = FindBestPath(transform.position, senpai.position);

            if (path.Length > 0)
            {
                if(debug) 
                {
                    Vector2 prev = path[0];
                    foreach (Vector2 dot in path) {
                        Debug.DrawLine(prev, dot, Color.cyan);
                        prev = dot;
                    }
                }
                
                float distToTarget = Vector2.Distance(transform.position, senpai.position);

                if (distToTarget <= collisionPreventionRadius && stopOnTouch) {
                    return Vector2.zero;
                } else if (distToTarget <= stopAtDistToTarget)
                {
                    return Vector2.zero;
                }
                return (path[0] - (Vector2)transform.position).normalized * speed;
            }
        }

        return Vector2.zero;
    }


    private Vector2[] FindBestPath(Vector2 currentPosition, Vector2 targetPosition)
    {
        var frontier = new PriorityQueue<Vector2>();
        frontier.Enqueue(currentPosition, 0);
        
        var cameFrom = new Dictionary<Vector2, Vector2>();
        var costSoFar = new Dictionary<Vector2, float>();
        
        cameFrom[currentPosition] = Vector2.zero;
        costSoFar[currentPosition] = 0;
        costSoFar[targetPosition] = aStarMaxDistance + 1;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var currentCost = costSoFar[current];

            //float distToTarget = Vector2.Distance(current, targetPosition) - collisionPreventionRadius;

            RaycastHit2D hit = Physics2D.Raycast(current, targetPosition - current, Vector2.Distance(current, targetPosition), 1 << senpai.gameObject.layer);
            if(hit.collider == null) {
                throw new System.Exception("Senpai not found");
            }
            float distToTarget = Vector2.Distance(current, hit.point) - collisionPreventionRadius;

            if (currentCost > aStarMaxDistance || currentCost + distToTarget > costSoFar[targetPosition])
            {
                continue;
            }


            // Either we're that close, that we already hit the target
            // Then no further movement is required
            if (distToTarget <= 0)
            {
                if(debug)
                    Debug.DrawLine(current, targetPosition, Color.yellow);
                
                if (currentCost < costSoFar[targetPosition]) {
                    costSoFar[targetPosition] = currentCost;
                    cameFrom[targetPosition] = current;
                }
                continue;
            }
            else if (distToTarget <= aStarDistance)
            {
                if(debug)
                    Debug.DrawLine(current, hit.point, Color.yellow);
                
                if (currentCost + distToTarget < costSoFar[targetPosition]) {
                    costSoFar[targetPosition] = currentCost + distToTarget;
                    cameFrom[targetPosition] = current;
                }
                continue;
            }

            foreach (Vector2 next in GetNeighbors(current))
            {
                var newCost = currentCost + Vector2.Distance(current, next);

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }

        // Reconstruct path
        var path = new List<Vector2>();
        Vector2 currentNode = targetPosition;

        if(!cameFrom.ContainsKey(targetPosition)) {
            return path.ToArray();
        }

        while (currentNode != currentPosition) 
        {
            path.Add(currentNode);
            currentNode = cameFrom[currentNode];
        }
        path.Reverse();

        return path.ToArray();
    }


    private Vector2[] directions = new Vector2[] { 
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, 1),
        new Vector2(-1, -1),
    };
    private List<Vector2> GetNeighbors(Vector2 current)
    {
        List<Vector2> result = new List<Vector2>();
        foreach (Vector2 direction in directions)
        {
            if(Physics2D.CircleCast(current, collisionPreventionRadius, direction, aStarDistance, this.blockingLayers).collider == null) {
                result.Add(current + (direction * aStarDistance));
                if(debug)
                    Debug.DrawLine(current, current + (direction * aStarDistance), Color.green);
            }
        }
        return result;
    }


    private class PriorityQueue<TElement> {
        private SortedList<float, Queue<TElement>> list = new SortedList<float, Queue<TElement>>();

        public void Enqueue(TElement item, float priority)
        {
            if (!list.ContainsKey(priority))
            {
                list.Add(priority, new Queue<TElement>());
            }
            list[priority].Enqueue(item);
        }

        public TElement Dequeue()
        {
            while(list.Count > 0) {
                var firstKey = list.Keys[0];
                var firstQueue = list[firstKey];

                var dequed = firstQueue.Dequeue();

                if (firstQueue.Count == 0)
                {
                    list.Remove(firstKey);
                }

                return dequed;
            }
            throw new System.Exception("Priority queue is empty");
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}