using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class AbstractInteractionBehavior : MonoBehaviour
{
    public abstract void OnInteract(string tag);
}
