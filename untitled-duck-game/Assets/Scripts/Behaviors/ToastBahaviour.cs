using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastBahaviour : AbstractInteractionBehavior
{
    public override void OnInteract(string tag)
    {
        EventBus.Fire("collect_toast");
        Destroy(this.gameObject);
    }
}
