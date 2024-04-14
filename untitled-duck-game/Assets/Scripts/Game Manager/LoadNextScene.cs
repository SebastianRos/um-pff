using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : Listener
{
    public string scene_name = null;
    public string load_scene_event = null;

    private Scene nextScene;
    // Start is called before the first frame update
    void Start()
    {
        this.nextScene = SceneManager.CreateScene(this.scene_name);
        EventBus.Register(this.load_scene_event, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene()
    {
        if(this.scene_name != null && !this.scene_name.Equals(""))
        {
            SceneManager.LoadScene(this.nextScene.name);
        }
    }

    public override void Callback(string evt)
    {
        if(evt.Equals(this.load_scene_event)) {
            this.LoadScene();
            return;
        }
    }
}
