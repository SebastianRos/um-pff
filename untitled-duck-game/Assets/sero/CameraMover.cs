using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    GameObject player;    
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -100
        );
    }
}
