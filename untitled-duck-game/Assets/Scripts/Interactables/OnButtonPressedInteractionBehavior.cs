using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonPressedInteractionBehavior : Interactible
{
    public override bool ShouldInteractionTrigger()
    {
        if (Input.GetButtonDown("Interact"))
            Debug.Log(Input.GetButtonDown("Interact"));
        return Input.GetButtonDown("Interact");
    }
}
