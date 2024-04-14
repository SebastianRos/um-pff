using UnityEngine;

public class DuckEggInteractionBehavior : AbstractInteractionBehavior
{
    public DuckBrain duck;
    public Transform duckGroup;

    public override void OnInteract()
    {
        Debug.Log("Interacted with duck egg");
        DuckBrain newDuck = Instantiate(duck, this.transform.position, Quaternion.identity);
        newDuck.transform.SetParent(duckGroup);
        Destroy(this.gameObject);
    }
}
