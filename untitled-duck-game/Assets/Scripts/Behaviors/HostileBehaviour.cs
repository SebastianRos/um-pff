using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileBehaviour : AbstractInteractionBehavior
{
    public bool DestroyOnImpact = false;
    public override void OnInteract()
    {
        EventBus.Fire("damage_player");
        if(this.DestroyOnImpact)
        {
            Destroy(this.gameObject);
        }
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
