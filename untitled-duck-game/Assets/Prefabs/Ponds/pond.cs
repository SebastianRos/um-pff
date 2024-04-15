using System;
using UnityEngine;

public class Pond : AbstractInteractionBehavior, Observer
{
    public GameObject drawboardToInstantiate;
    public GameObject duckToInstantiate;

    private Drawboard drawboardScript;
    private GameObject drawboardGo;
    private bool drawboardOpen = false;
    private bool duckSummend = false;
    // Start is called before the first frame update
    void Start(){}

    public override void OnInteract() {
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
        GameManager.instance.decrementBread();
        duckSummend = true;
        drawboardOpen = false;
        Destroy(drawboardGo);
        EventBus.Fire("enablePlayer");
        Instantiate(duckToInstantiate, transform.position, Quaternion.identity);
    }

}


