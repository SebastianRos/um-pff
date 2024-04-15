using UnityEngine;

public abstract class AbstractInteractionBehavior : MonoBehaviour
{
    public abstract void OnInteract(string tag);
}
