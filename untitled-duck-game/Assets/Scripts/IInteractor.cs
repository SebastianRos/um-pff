using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    // provide default implementation, since mostly only one required
    public void NearInteractable(Collider2D collider) {}
    
    // provide default implementation, since mostly only one required
    public void Interact(Collider2D collider) {}
}
