using System.Collections.Generic;
using UnityEngine;

public class Drawboard : MonoBehaviour, Observable
{
    Pattern pattern;
    TimerBar timerbar;
    float startTime;

    private List<Observer> observers = new List<Observer>();
    private ValidatorStatus status = ValidatorStatus.KEEP_GOING;
    
    // Start is called before the first frame update
    void Start(){
        pattern = GetComponentInChildren<Pattern>();
        timerbar = GetComponentInChildren<TimerBar>();
        startTime = Time.time;
    }

    void Update(){
        float timePassed = Time.time - startTime;
        timerbar.setPercentage(1 - (timePassed / pattern.time));

        if (status == ValidatorStatus.KEEP_GOING && pattern.time <= timePassed){
            status = ValidatorStatus.FAILED;
            foreach (Observer observer in observers) observer.notify();
        }
    }

    public ValidatorStatus getStatus() {
        return status;
    }

    public void register(Observer observer){
        observers.Add(observer);
    }
    public void unregister(Observer observer){
        if (observers.Contains(observer))
            observers.Remove(observer);
    }
}


