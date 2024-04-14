using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchInteractionBehavior : Interactible
{
    public override bool ShouldInteractionTrigger()
    {
        return true;
    }
}
