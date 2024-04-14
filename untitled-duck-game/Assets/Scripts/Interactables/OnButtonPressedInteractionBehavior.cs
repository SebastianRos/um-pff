using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonPressedInteractionBehavior : Interactible
{
    public override bool ShouldInteractionTrigger()
    {
        return Input.GetButtonDown("Interact");
    }
}
