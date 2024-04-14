using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    // provide default implementation, since mostly only one required
    public void OnNearInteractable(Collider2D collider) {}
    
    // provide default implementation, since mostly only one required
    public void OnInteract(Collider2D collider) {}
}
