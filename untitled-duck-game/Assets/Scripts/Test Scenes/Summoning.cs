using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoning : Interactible
{
    public GameObject summonedDuck = null;
    // Start is called before the first frame update
    void Start()
    {
        if(this.summonedDuck) {
            this.summonedDuck.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact") && this.InteractionPossible()) {
            this.NotifyInteract();
            if(this.summonedDuck) {
                this.summonedDuck.SetActive(true);
            }
        }
    }
}
