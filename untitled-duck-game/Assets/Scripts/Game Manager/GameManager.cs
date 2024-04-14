using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour, IListener
{
    // https://gamedev.stackexchange.com/questions/34874/where-to-attach-global-scripts-in-unity#answer-34879
    public static GameManager Instance { get; private set; } = null;
    
// --- initialisation ---
    public int maxPlayerLife = 1900;
    public int startBreadcrumbs = 5;
    // --- initialisation ---

    // --- current state ---
    private int currPlayerlife;
    private int currBreadcrumbs;
    private List<GameObject> enemies;
    // --- current state ---

    private enum Events {
        RESET,
        LOAD_SCENE,
        COLLECT_TOAST,
    }
    private string[] ListenToEvents = {
         "reset",
         "load_scene",
         "collect_toast",
    };

    private void Awake() {
        if(Instance != null)
        {
            Debug.LogError("more than one instance");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(string evt in this.ListenToEvents)
        {
            EventBus.Register(evt, this.gameObject);
        }

        EventBus.Fire(this.ListenToEvents[(int)Events.RESET]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Reset()
    {
        this.currPlayerlife = this.maxPlayerLife;        
        this.currBreadcrumbs = this.startBreadcrumbs;
        this.enemies = new List<GameObject>();
    }

    public void Callback(string evt) {
        if(evt.Equals(this.ListenToEvents[(int)Events.RESET])) {
            this.Reset();
            return;
        }
        if(evt.Equals(this.ListenToEvents[(int)Events.COLLECT_TOAST])) {
            this.currBreadcrumbs++;
            Debug.Log("breadcrumbs: " + this.currBreadcrumbs);
            return;
        }
    }
}
