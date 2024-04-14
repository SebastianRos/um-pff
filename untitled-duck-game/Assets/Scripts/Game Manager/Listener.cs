using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

interface IListener {
    public void Callback();
}

public abstract class Listener : MonoBehaviour, IListener
{
    public string eventName = "eventName";
    // Start is called before the first frame update
    void Start()
    {
        GameManager.RegisterForEvent(this.eventName, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Callback();
}
