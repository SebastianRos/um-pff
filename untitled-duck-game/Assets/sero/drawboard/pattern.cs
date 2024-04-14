using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {
    // Start is called before the first frame update
    public float tolerance;
    public float time;

    public PointPatternValidator getValidator(){
        return new PointPatternValidator(
            getPoints(),
            tolerance
        );
    }

    public Vector2[] getPoints(){
        List<Vector2> patternPoints = new List<Vector2>();
        foreach (Transform point in transform.GetComponentsInChildren<Transform>()){
            if (point == transform) continue;
            patternPoints.Add(point.position);
        }
        patternPoints.Add(patternPoints[0]);
        return patternPoints.ToArray();
    }
}

public class PointPatternValidator{
    private Vector2[] pattern;
    private float tolerance;

    private int nextPatternIndex = 0;

    public PointPatternValidator(Vector2[] pattern, float tolerance){
        this.pattern = pattern;
        this.tolerance = tolerance;
    }

    public ValidatorStatus validateSinglePoint(Vector2 toValidate){
        if (pattern.Length <= nextPatternIndex) return ValidatorStatus.SUCCESS;
        if (nextPatternIndex == -1) return ValidatorStatus.FAILED;

        float distance = Vector2.Distance(pattern[nextPatternIndex], toValidate);
        if (distance <= tolerance) {
            nextPatternIndex++;
            if (pattern.Length == nextPatternIndex) return ValidatorStatus.SUCCESS;
            else return ValidatorStatus.KEEP_GOING;
        } 
        return ValidatorStatus.FAILED;
    }
}

public enum ValidatorStatus {SUCCESS, FAILED, KEEP_GOING}
