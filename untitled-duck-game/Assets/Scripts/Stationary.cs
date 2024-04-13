using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stationary : Interactible
{
    public GameObject showWhileNear = null;
    private bool lastFrame = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool thisFrame = this.InteractionPossible();
        if(thisFrame != this.lastFrame) {
            this.lastFrame = thisFrame;
            // toggle visibility
            if(this.showWhileNear) {
                this.showWhileNear.SetActive(thisFrame);
            }
        }
        
    }
}
