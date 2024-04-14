using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    // This one has to be
    public string interactor_tag = "Player";
    private readonly List<GameObject> withinVicinity = new List<GameObject>();

    private AbstractInteractionBehavior[] ownInteractors;

    public abstract bool ShouldInteractionTrigger();


    void Start() {
        ownInteractors = this.GetComponents<AbstractInteractionBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ShouldInteractionTrigger() && this.InteractionPossible())
        {
            this.OnInteract();
        }
    }
    
    private bool InteractionPossible() {
        return this.withinVicinity.Count != 0;
    }


    private void OnInteract() {
        foreach(AbstractInteractionBehavior interactor in this.ownInteractors) {
            interactor.OnInteract();
        }
        foreach(GameObject go in withinVicinity) {
            go.GetComponent<IInteractor>()?.OnInteract(this.GetComponent<Collider2D>());
        }
    }

    /**
     * unify collision detection 
     */
    private bool CollisionWithInteractor(Collider2D collider) {
        return collider.CompareTag(interactor_tag);
    }

    private int FindCollider(Collider2D c1) {
        return this.withinVicinity.FindIndex((match) => c1.GetInstanceID() == match.GetInstanceID());
    }

    /**
     * detect interactor has entered vicinity
     */
    private void OnTriggerEnter2D(Collider2D collider) {
        if(CollisionWithInteractor(collider) && this.FindCollider(collider) == -1) {
            this.withinVicinity.Add(collider.gameObject);
        }
    }
    
    /**
     * detect interactor has left vicinity
     */
    private void OnTriggerExit2D(Collider2D collider) {
        if(CollisionWithInteractor(collider) && this.FindCollider(collider) != -1) {
            this.withinVicinity.Remove(collider.gameObject);
        }
    }
}
