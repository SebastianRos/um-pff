using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerNearPond : AbstractInteractionBehavior
{
    Coroutine _coroutine = null;
    public override void OnInteract(string tag)
    {
        if(this._coroutine != null) {
            StopCoroutine(this._coroutine);
        }
        this._coroutine = StartCoroutine(this.EnableSprite());
    }

    private IEnumerator EnableSprite()
    {
        this.GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
