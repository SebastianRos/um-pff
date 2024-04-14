using System.Collections.Generic;
using UnityEngine;

public class Drawboard : MonoBehaviour, Observable
{
    public float height;
    public float width;
    public GameObject arrowPref;

    private List<Observer> observers = new List<Observer>();
    private Pattern pattern;
    private TimerBar timerbar;
    private LineDrawer lineDrawer;

    private float startTime;
    private ValidatorStatus status = ValidatorStatus.KEEP_GOING;
    private bool isButtonCurrentlyPressed = false;
    private PointPatternValidator validator;
    
    // Start is called before the first frame update
    void Start(){
        pattern = GetComponentInChildren<Pattern>();
        validator = pattern.getValidator();
        timerbar = GetComponentInChildren<TimerBar>();
        lineDrawer = GetComponentInChildren<LineDrawer>();
        startTime = Time.time;

        generateArrows();
    }

    void Update(){

        // Drawing
        if (!isButtonCurrentlyPressed && Input.GetAxis("Fire1") == 1){
            isButtonCurrentlyPressed = true;

            Vector3 camPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (contains(camPoint)) {
                lineDrawer.addPoint(camPoint);
                ValidatorStatus pointResult = validator.validateSinglePoint(camPoint);
                if (pointResult != ValidatorStatus.KEEP_GOING){
                    status = pointResult;
                    foreach (Observer observer in observers) observer.notify();
                }
            }
        } else if (Input.GetAxis("Fire1") == 0)  {
            isButtonCurrentlyPressed = false;
        }

        // Timing
        float timePassed = Time.time - startTime;
        timerbar.setPercentage(1 - (timePassed / pattern.time));
        if (status == ValidatorStatus.KEEP_GOING && pattern.time <= timePassed){
            status = ValidatorStatus.FAILED;
            foreach (Observer observer in observers) observer.notify();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 1, 1);
        Vector3 a = new Vector3(
            transform.position.x - width / 2,
            transform.position.y - height / 2,
            transform.position.z
        );
        Vector3 b = new Vector3(
            transform.position.x + width / 2,
            transform.position.y - height / 2,
            transform.position.z
        );
        Vector3 c = new Vector3(
            transform.position.x + width / 2,
            transform.position.y + height / 2,
            transform.position.z
        );
        Vector3 d = new Vector3(
            transform.position.x - width / 2,
            transform.position.y + height / 2,
            transform.position.z
        );

        Gizmos.DrawLineList(new Vector3[8]{a, b, b, c, c, d, d, a});
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

    private bool contains(Vector2 point){
        float xMin = transform.position.x - width / 2;
        float xMax = transform.position.x + width / 2;
        float yMin = transform.position.y - height / 2;
        float yMax = transform.position.y + height / 2;

        return point.x >= xMin && point.x <= xMax && point.y >= yMin && point.y <= yMax;
    }

    private void generateArrows(){
        Vector2[] points = pattern.getPoints();

        for (int i = 1; i < points.Length; i++){
            Vector2 pointer = points[i] - points[i-1];
            Vector2 arrowLocation = points[i-1] + pointer*0.5f;
            Quaternion arrowRotation = Quaternion.LookRotation(pointer);
            Instantiate(arrowPref, arrowLocation, arrowRotation, transform);
        }
    }
}


