using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorize : MonoBehaviour
{
    public Color primaryColor = Color.green;
    public Color secondaryColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        Interactible interactible = this.GetComponent<Interactible>();

        if(!sprite.color.Equals(this.primaryColor) && interactible != null && interactible.InteractionPossible()) {
            sprite.color = this.primaryColor;
        }
        if(interactible != null && !interactible.InteractionPossible() && !sprite.color.Equals(this.secondaryColor)) {
            sprite.color = this.secondaryColor;
        }
    }
}
