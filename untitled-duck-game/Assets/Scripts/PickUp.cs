using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactible
{
    new readonly string INTERACTOR_TAG = "Player";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")) {
            this.NotifyInteract();
            Destroy(this);
        }
    }
}
