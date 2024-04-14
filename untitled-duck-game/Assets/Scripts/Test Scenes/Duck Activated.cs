using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckActivated : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Register("reset", this.gameObject);
        EventBus.Register("duck_activated", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Callback(string evt)
    {
        if(evt.Equals("reset")) {
            this.gameObject.SetActive(false);
        }
        if(evt.Equals("duck_activated")) {
            this.gameObject.SetActive(true);
            Debug.Log("Quack");
        }
    }
    
}
