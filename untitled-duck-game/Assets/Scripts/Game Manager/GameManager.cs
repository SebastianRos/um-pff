using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IListener
{
    // https://discussions.unity.com/t/dontdestroyonload-many-instances-of-one-object/154454/2
    public static GameManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
   
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
    private readonly Dictionary<Events, string> MyEvents = new Dictionary<Events, string>()
    {
        {Events.RESET, "reset"},
        {Events.LOAD_SCENE, "load_scene"},
        {Events.COLLECT_TOAST, "collect_toast"},
        {Events.DAMAGE_PLAYER, "damage_player"}
    };
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(string evt in this.MyEvents.Values)
        {
            EventBus.Register(evt, this.gameObject);
        }

        EventBus.Fire(this.MyEvents[Events.RESET]);

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
        if(evt.Equals(this.MyEvents[Events.RESET])) {
            this.Reset();
            return;
        }
        if(evt.Equals(this.MyEvents[Events.COLLECT_TOAST])) {
            this.currBreadcrumbs++;
            Debug.Log("breadcrumbs: " + this.currBreadcrumbs);
            return;
        }
        if(evt.Equals(this.MyEvents[Events.DAMAGE_PLAYER])) {
            this.currPlayerlife--;
            if(this.currPlayerlife < 1) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                EventBus.Fire(this.MyEvents[Events.RESET]);
                // EventBus.Fire("game_over");
                Debug.Log("GAME OVER");
            }
        }
    }
}
