using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public int maxSpeed;
    public int accelartion;
    public int decceleration;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        Vector2 inputDirection = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
        Vector2 velocity = inputDirection * maxSpeed;
        
        rb.velocity = velocity;
    }
}
