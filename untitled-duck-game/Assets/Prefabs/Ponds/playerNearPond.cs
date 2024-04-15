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
    // Start is called before the first frame update
    void Start()
    {
        // maybe size this Collider2D to that of the parent?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EnableSprite()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
