using System.Collections.Generic;
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
            instance.Reset();
            DontDestroyOnLoad(gameObject);
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
        Debug.Log("AWAKE " + instance.currStage);
    }
   
    // --- initialisation ---
    public int maxPlayerLife = 1900;
    public int startBreadcrumbs = 5;
    // --- initialisation ---

    // --- current state ---
    public int currStage { get; set; } = 1;
    private int currPlayerlife;
    private int currBreadcrumbs;
    private List<GameObject> enemies;
    // --- current state ---

    private enum Events {
        RESET,
        LOAD_SCENE,
        COLLECT_TOAST,
        DAMAGE_PLAYER,
        GAME_OVER,
        LEVEL_COMPLETE,
    }
    private readonly Dictionary<Events, string> MyEvents = new Dictionary<Events, string>()
    {
        {Events.RESET, "reset"},
        {Events.LOAD_SCENE, "load_scene"},
        {Events.COLLECT_TOAST, "collect_toast"},
        {Events.DAMAGE_PLAYER, "damage_player"},
        {Events.GAME_OVER, "game_over"},
        {Events.LEVEL_COMPLETE, "level_complete"},
    };
    
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(string evt in this.MyEvents.Values)
        {
            EventBus.Register(evt, this.gameObject);
        }
    }

    void Reset()
    {
        this.currPlayerlife = this.maxPlayerLife;        
        this.currBreadcrumbs = this.startBreadcrumbs;
        this.currStage = 1;
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
                // EventBus.Fire(this.MyEvents[Events.GAME_OVER]);
                Debug.Log("GAME OVER");
            }
            return;
        }
        if(evt.Equals(this.MyEvents[Events.LEVEL_COMPLETE])) {
            this.currStage++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
    }

    public int getBreadCount(){
        return currBreadcrumbs;
    }
    public void decrementBread(){
        currBreadcrumbs--;
    }
    public void incrementBread(){
        currBreadcrumbs++;
    }
}
