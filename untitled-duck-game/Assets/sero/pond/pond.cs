using System;
using UnityEngine;

public class Pond : AbstractInteractionBehavior, Observer
{
    public GameObject drawboardToInstantiate;

    private Drawboard drawboardScript;
    private GameObject drawboardGo;
    // Start is called before the first frame update
    void Start(){}

    public override void OnInteract() {
        openDrawBoard();
    }

    public void notify(){
        if (drawboardScript.getStatus() == ValidatorStatus.SUCCESS)
            onDrawBoardSuccess();
        else if (drawboardScript.getStatus() == ValidatorStatus.FAILED)
            onDrawBoardFailed();
    }

    private void openDrawBoard(){
        drawboardGo = Instantiate(drawboardToInstantiate, transform.position, Quaternion.identity);
        drawboardScript = drawboardGo.GetComponent<Drawboard>();
        drawboardScript.register(this);
    }

    private void onDrawBoardFailed(){
        Destroy(drawboardGo);
        Debug.Log("Nay");
    }
    private void onDrawBoardSuccess(){
        Destroy(drawboardGo);
        Debug.Log("Yay");
    }

}


