using UnityEngine;

public class playerNearPond : MonoBehaviour
{

    public void Start() {
        this.GetComponent<Canvas>().enabled = false;
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject player = GameObject.Find("Player");
        if(player != null && player.GetInstanceID().Equals(collider.gameObject.GetInstanceID()))
        {
            this.GetComponent<Canvas>().enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        GameObject player = GameObject.Find("Player");
        if(player != null && player.GetInstanceID().Equals(collider.gameObject.GetInstanceID()))
        {
            this.GetComponent<Canvas>().enabled = false;
        }
    }
}
