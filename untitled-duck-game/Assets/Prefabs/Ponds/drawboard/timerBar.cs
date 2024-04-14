using System;
using UnityEngine;

public class TimerBar : MonoBehaviour
{
    SpriteRenderer timerBarSprite;
    float initialSpriteWidth;
    // Start is called before the first frame update
    void Start(){
        timerBarSprite = transform.Find("timerbar").GetComponent<SpriteRenderer>();
        initialSpriteWidth = timerBarSprite.size.x;
    }

    public void setPercentage(float percentage){
        percentage = Math.Clamp(percentage, 0, 1);
        timerBarSprite.size = new Vector2(
            initialSpriteWidth * percentage,
            timerBarSprite.size.y
        );
    }
}


