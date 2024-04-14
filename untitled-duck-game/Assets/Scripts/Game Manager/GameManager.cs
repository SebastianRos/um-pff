using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Listener
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
    // --- current state ---
    
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Register("reset", this.gameObject);
        EventBus.Fire("reset");
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

    public override void Callback(string evt) {
        if(evt.Equals("reset")) {
            this.Reset();
        }
    }
}
