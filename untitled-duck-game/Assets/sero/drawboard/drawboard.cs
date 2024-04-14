using System;
using UnityEngine;
using UnityEngine.Video;

public class drawboard : MonoBehaviour
{
    Pattern pattern;
    TimerBar timerbar;
    float startTime;
    float endTime;
    
    // Start is called before the first frame update
    void Start(){
        pattern = GetComponentInChildren<Pattern>();
        timerbar = GetComponentInChildren<TimerBar>();
        startTime = Time.time;
        endTime = Time.time + pattern.time;
    }

    void Update(){
        float timePassed = Time.time - startTime;
        timerbar.setPercentage(1 - (timePassed / pattern.time));
    }

}


