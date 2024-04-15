using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public abstract class Interactible : MonoBehaviour
{
    [Serializable]
    public struct EventToTrigger {
        public string tagName;
        public string eventName;
    }

    // This one has to be
    public List<string> interactor_tags = new List<string>{"Player"};
    public EventToTrigger[] TriggerEvents;
    private Dictionary<string, List<string>> eventsForTag = new Dictionary<string, List<string>>();
    private readonly Dictionary<string, List<GameObject>> withinVicinity = new Dictionary<string, List<GameObject>>();

    private AbstractInteractionBehavior[] ownInteractors;

    public abstract bool ShouldInteractionTrigger();


    void Awake() {
        ownInteractors = this.GetComponents<AbstractInteractionBehavior>();
        
        foreach (string tag in interactor_tags)
        {
            this.withinVicinity[tag] = new List<GameObject>();
        }
        
        foreach (EventToTrigger eventToTrigger in TriggerEvents)
        {
            if(!this.eventsForTag.ContainsKey(eventToTrigger.tagName)) {
                this.eventsForTag.Add(eventToTrigger.tagName, new List<string>());
            }
            this.eventsForTag[eventToTrigger.tagName].Add(eventToTrigger.eventName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ShouldInteractionTrigger())
        {
            foreach (string tag in interactor_tags)
            {
                if(this.InteractionPossible(tag)) {
                    this.OnInteract(tag);

                    if(eventsForTag.ContainsKey(tag)) {
                        foreach(string eventName in eventsForTag[tag]) {
                            EventBus.Fire(eventName);
                        }
                    }
                }
            }
        }
    }
    
    private bool InteractionPossible(string tag) {
        if(this.withinVicinity.ContainsKey(tag)) {
            return this.withinVicinity[tag].Count != 0;
        }
        return false;
    }


    private void OnInteract(string tag) {
        foreach(AbstractInteractionBehavior interactor in this.ownInteractors) {
            interactor.OnInteract(tag);
        }
        foreach(GameObject go in withinVicinity[tag]) {
            go.GetComponent<IInteractor>()?.OnInteract(this.GetComponent<Collider2D>());
        }
    }

    /**
     * unify collision detection 
     */
    private string GetInteractorTag(Collider2D collider) {
        foreach (string tag in interactor_tags)
        {
            if (collider.CompareTag(tag))
            {
                return tag;
            }
        }
        return null;
    }

    /**
     * detect interactor has entered vicinity
     */
    private void OnTriggerEnter2D(Collider2D collider) {
        string tag = GetInteractorTag(collider);
        if(tag != null && this.interactor_tags.Contains(tag)) {
            if(!this.withinVicinity.ContainsKey(tag)) {
                this.withinVicinity[tag] = new List<GameObject>();
            }
            if(!this.withinVicinity[tag].Contains(collider.gameObject)) {
                this.withinVicinity[tag].Add(collider.gameObject);
            }
        }
    }
    
    /**
     * detect interactor has left vicinity
     */
    private void OnTriggerExit2D(Collider2D collider) {
        foreach (string tag in interactor_tags)
        {
            if(this.withinVicinity[tag].Contains(collider.gameObject)) {
                this.withinVicinity[tag].Remove(collider.gameObject);
            }
        }
    }
}
