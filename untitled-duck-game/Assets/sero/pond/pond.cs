using System;
using UnityEngine;

public class Pond : AbstractInteractionBehavior
{
    // Start is called before the first frame update
    void Start(){}

    public override void OnInteract() {
        openDrawBoard();
    }

    private void openDrawBoard(){
        Debug.Log("bla");
    }

}


