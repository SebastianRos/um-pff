using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastBahaviour : AbstractInteractionBehavior
{
    public override void OnInteract()
    {
        EventBus.Fire("collect_toast");
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
