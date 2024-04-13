using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    // This one has to be
    public string INTERACTOR_TAG = "Player";
    private readonly List<Collider2D> withinVicinity = new List<Collider2D>();
    
    public bool InteractionPossible() {
        return this.withinVicinity.Count != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NotifyNearInteractible();
    }

    public void NotifyInteract() {
        for(int colliderIndex = 0; colliderIndex < this.withinVicinity.Count; colliderIndex++) {
            this.withinVicinity[colliderIndex].GetComponent<IInteractor>().Interact(this.GetComponent<Collider2D>());
        }
    }

    public void NotifyNearInteractible() {
        for(int colliderIndex = 0; colliderIndex < this.withinVicinity.Count; colliderIndex++) {
            this.withinVicinity[colliderIndex].GetComponent<IInteractor>().NearInteractable(this.GetComponent<Collider2D>());
        }
    }

    /**
     * unify collision detection 
     */
    private bool CollisionWithInteractor(Collider2D collider) {
        return collider.CompareTag(INTERACTOR_TAG);
    }

    private int FindCollider(Collider2D c1) {
        return this.withinVicinity.FindIndex((match) => c1.GetInstanceID() == match.GetInstanceID());
    }

    /**
     * detect interactor has entered vicinity
     */
    private void OnTriggerEnter2D(Collider2D collider) {
        if(CollisionWithInteractor(collider) && this.FindCollider(collider) == -1) {
            this.withinVicinity.Add(collider);
        }
    }
    
    /**
     * detect interactor has left vicinity
     */
    private void OnTriggerExit2D(Collider2D collider) {
        if(CollisionWithInteractor(collider) && this.FindCollider(collider) != -1) {
            this.withinVicinity.Remove(collider);
        }
    }
}
