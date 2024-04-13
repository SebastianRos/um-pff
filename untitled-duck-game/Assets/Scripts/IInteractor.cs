using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    public void NearInteractable(Collider2D collider);
    public void Interact(Collider2D collider);
}
