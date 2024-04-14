using System.Collections;
using System.Collections.Generic;
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

public class GameManager : MonoBehaviour
{
    // --- initialisation ---
    public int maxPlayerLife = 1900;
    public int maxBreadcrumbs = 100;
    public int startBreadcrumbs = 5;
    // --- initialisation ---

    // --- current state ---
    private int currPlayerlife;
    private int currBreadcrumbs;
    private List<GameObject> enemies;
    private List<Tupel<string, GameObject>> listener;
    // --- current state ---

    // Start is called before the first frame update
    void Start()
    {
        this.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterInstance(string evt, GameObject gameObject) {
        this.listener.Add(new Tupel<string, GameObject>(evt, gameObject));
    }

    void FireInstance(string evt) {
        bool firedOnce = false;
        foreach(Tupel<string, GameObject> tupel in this.listener) {
            if(tupel.e.Equals(evt)) {
                Listener listener = tupel.i.GetComponent<Listener>();
                if(listener != null) {
                    listener.Callback();
                    firedOnce = true;
                }
            }
        }
        if(!firedOnce) {
            Debug.Log("evt " + evt + " was fired, but none was notified, did you 'RegisterForEvent'?");
        }
    }

    public static bool Fire(string evt) {
        GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
        if(gm != null) {
            gm.FireInstance(evt);
            return true;
        }
        return false;
    }

    public static bool RegisterForEvent(string evt, GameObject gameObject) {
        GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
        if(gm != null) {
            gm.RegisterInstance(evt, gameObject);
            return true;
        }
        return false;
    }

    void Reset()
    {
        this.currPlayerlife = this.maxPlayerLife;        
        this.currBreadcrumbs = this.startBreadcrumbs;
        this.enemies = new List<GameObject>();
        this.listener = new List<Tupel<string, GameObject>>();

    }
}
