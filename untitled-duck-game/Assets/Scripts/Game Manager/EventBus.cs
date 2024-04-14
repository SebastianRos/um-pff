using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Tupel<E, I> 
{
    public E e;
    public I i;

    public Tupel(E e, I i) {
        this.e = e;
        this.i = i;
    }
}

public class EventBus : MonoBehaviour
{
    private List<Tupel<string, GameObject>> listener = new List<Tupel<string, GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterInstance(string evt, GameObject gameObject) {
        Tupel<string, GameObject> newTupel = new Tupel<string, GameObject>(evt, gameObject);
        if(!this.listener.Exists(tupel => 
            tupel.e.Equals(newTupel.e) 
            && tupel.i.GetInstanceID().Equals(newTupel.i.GetInstanceID())
        ))
        {
            this.listener.Add(newTupel);
        }
    }

    void FireInstance(string evt) {
        bool firedOnce = false;
        foreach(Tupel<string, GameObject> tupel in this.listener) {
            if(tupel.e.Equals(evt) && !tupel.i.IsDestroyed()) {
                IListener listener = tupel.i.GetComponent<IListener>();
                if(listener != null) {
                    listener.Callback(evt);
                    firedOnce = true;
                }
            }
        }
        if(!firedOnce) {
            Debug.Log("evt " + evt + " was fired, but none was notified, did you 'RegisterForEvent'?");
        }
    }

    public static bool Fire(string evt) {
        EventBus eb = GameObject.FindAnyObjectByType<EventBus>();
        if(eb != null) {
            eb.FireInstance(evt);
            return true;
        }
        return false;
    }

    public static bool Register(string evt, GameObject gameObject) {
        EventBus eb = GameObject.FindAnyObjectByType<EventBus>();
        if(eb != null) {
            eb.RegisterInstance(evt, gameObject);
            return true;
        }
        return false;
    }
}
