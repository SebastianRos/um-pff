using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    GameObject player;

    void Update(){
        if(player == null){
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -100
        );
    }
}
