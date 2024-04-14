using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DuckBrain : MonoBehaviour
{
    private FangirlBehavior fangirlBehavior;
    private DuckFollowingBehavior duckFollowingBehavior;
    private Rigidbody2D rb;

    private void Start()
    {
        fangirlBehavior = GetComponent<FangirlBehavior>();
        duckFollowingBehavior = GetComponent<DuckFollowingBehavior>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!duckFollowingBehavior.isPathBlockedByDucks())
        {
            fangirlBehavior.senpai = duckFollowingBehavior.target;
            fangirlBehavior.stopAtDistToTarget = 0.6f;
        }
        else 
        {
            DuckBrain closestDuck = duckFollowingBehavior.getClosestBlockingDuck();
            fangirlBehavior.senpai = closestDuck.transform;
            fangirlBehavior.stopAtDistToTarget = 0.45f;
        }

        Vector2 move = fangirlBehavior.CalculateDirection();
        this.rb.velocity = move;
    }
}