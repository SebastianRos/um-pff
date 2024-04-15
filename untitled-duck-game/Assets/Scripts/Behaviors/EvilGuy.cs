using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EvilGuy : MonoBehaviour
{
    public FangirlBehavior fangirlBehavior;
    private Rigidbody2D rb;

    void Awake()
    {
        fangirlBehavior = GetComponent<FangirlBehavior>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 move = fangirlBehavior.CalculateDirection();
        this.rb.velocity = move;
    }
}