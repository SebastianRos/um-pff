using System;
using UnityEngine;

public class Pond : AbstractInteractionBehavior, Observer
{
    public GameObject drawboardToInstantiate;

    private Drawboard drawboardScript;
    private GameObject drawboardGo;
    private bool drawboardOpen = false;
    // Start is called before the first frame update
    void Start(){}

    public override void OnInteract() {
        if (!drawboardOpen)
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
        drawboardOpen = false;
        Destroy(drawboardGo);
        Debug.Log("Yay");
        EventBus.Fire("enablePlayer");
    }

}


