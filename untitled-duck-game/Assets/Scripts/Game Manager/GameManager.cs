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
        DAMAGE_PLAYER
    }
    private string[] ListenToEvents = {
         "reset",
         "load_scene",
         "collect_toast",
         "damage_player"
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

    private bool EvtEquals(string evt, Events events) {
        return evt.Equals(this.ListenToEvents[(int)events]);
    }

    public void Callback(string evt) {
        if(this.EvtEquals(evt, Events.RESET)) {
            this.Reset();
            return;
        }
        if(this.EvtEquals(evt, Events.COLLECT_TOAST)) {
            this.currBreadcrumbs++;
            Debug.Log("breadcrumbs: " + this.currBreadcrumbs);
            return;
        }
        if(this.EvtEquals(evt, Events.DAMAGE_PLAYER)) {
            this.currPlayerlife--;
            if(this.currPlayerlife < 1) {
                EventBus.Fire("game_over");
                Debug.Log("GAME OVER");
            }
        }
    }
}
