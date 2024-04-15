using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DuckBrain : MonoBehaviour
{
    private FangirlBehavior fangirlBehavior;
    private DuckFollowingBehavior duckFollowingBehavior;
    private Rigidbody2D rb;

    public int EnemyTargetRange;
    public LayerMask EnemyLayer;

    private GameObject enemyNumberOne;
    public Transform Player;


    void Awake()
    {
        fangirlBehavior = GetComponent<FangirlBehavior>();
        duckFollowingBehavior = GetComponent<DuckFollowingBehavior>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!this.duckFollowingBehavior.target)
        {
            return;
        }


        // Path to target isn't blocked: Move to target
        if (!duckFollowingBehavior.isPathBlockedByDucks())
        {
            fangirlBehavior.senpai = duckFollowingBehavior.target;
            fangirlBehavior.stopAtDistToTarget = 0.6f;
        }
        // Path to target is blocked; Follow frontman
        else 
        {
            DuckBrain closestDuck = duckFollowingBehavior.getClosestBlockingDuck();
            fangirlBehavior.senpai = closestDuck.transform;
            fangirlBehavior.stopAtDistToTarget = 0.45f;
        }

        Vector2 move = fangirlBehavior.CalculateDirection();
        this.rb.velocity = move;
    }

    void FixedUpdate() {
        if(this.enemyNumberOne == null) {
            this.chooseEnemyNumberOne();

            if(this.duckFollowingBehavior.target != this.Player) {
                this.changeTargetToPlayer();
            }
        } else {
            this.changeTargetToEnemy();
        }
    }

    void changeTargetToEnemy() {
        this.duckFollowingBehavior.target = this.enemyNumberOne.transform;
    }
    void changeTargetToPlayer() {
        this.duckFollowingBehavior.target = this.Player;
    }

    void chooseEnemyNumberOne() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, this.EnemyTargetRange, 1 << this.EnemyLayer);

        GameObject closestEnemy = null;
        float minDistance = this.EnemyTargetRange + 1;

        foreach(Collider2D hit in hits) {
            float dist = Vector2.Distance(hit.transform.position, transform.position);
            if(dist < minDistance) {
                closestEnemy = hit.gameObject;
                minDistance = dist;
            }
        }

        this.enemyNumberOne = closestEnemy;
    }


    public void SetPlayer(Transform player)
    {
        this.Player = player;
        this.duckFollowingBehavior.target = player;
    }
}