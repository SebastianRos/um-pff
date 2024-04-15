using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreadUiCounter : MonoBehaviour{
    private TMP_Text text;

    void Start(){
        text = GetComponent<TMP_Text>();
    }
    void Update(){
        string breadCount;
        if (GameManager.instance != null){
            breadCount = GameManager.instance.getBreadCount().ToString();
        } else  {
            breadCount = "undefined";
        }
        text.text = "x" + breadCount;
    }
}
