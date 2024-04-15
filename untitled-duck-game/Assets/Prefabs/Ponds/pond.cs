using System;
using UnityEngine;

public class Pond : AbstractInteractionBehavior, Observer
{
    public GameObject drawboardToInstantiate;
    public DuckBrain duckToInstantiate;

    private Drawboard drawboardScript;
    private GameObject drawboardGo;
    private bool drawboardOpen = false;
    private bool duckSummend = false;
    

    public override void OnInteract(string tag) {
        if (
            0 < GameManager.instance.getBreadCount() 
            && !drawboardOpen
            && !duckSummend
        )
            openDrawBoard();
    }

    public void notify(){
        if (drawboardScript.getStatus() == ValidatorStatus.SUCCESS)
            onDrawBoardSuccess();
        else if (drawboardScript.getStatus() == ValidatorStatus.FAILED)
            onDrawBoardFailed();
    }

    private void openDrawBoard(){
        EventBus.Fire("disablePlayer");

        GameManager.instance.decrementBread();
        drawboardOpen = true;
        drawboardGo = Instantiate(drawboardToInstantiate, transform.position, Quaternion.identity);
        drawboardScript = drawboardGo.GetComponent<Drawboard>();
        drawboardScript.register(this);
    }

    private void onDrawBoardFailed(){
        drawboardOpen = false;
        Destroy(drawboardGo);
        Debug.Log("Nay");
        EventBus.Fire("enablePlayer");
    }
    private void onDrawBoardSuccess(){
        duckSummend = true;
        drawboardOpen = false;
        Destroy(drawboardGo);
        EventBus.Fire("enablePlayer");
        
        DuckBrain newDuck = Instantiate(duckToInstantiate, transform.position, Quaternion.identity);
        newDuck.SetPlayer(GameObject.FindGameObjectWithTag("Player").transform);
        Destroy(gameObject);
    }

}


