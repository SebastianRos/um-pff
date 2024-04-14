using System;
using Unity.VisualScripting;
using UnityEngine;

public class Moving : MonoBehaviour, IInteractor
{
    public int seconds = 5;
    public int rotationFactor = 100;
    private float passed = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.passed+= Time.deltaTime;
        Move();
    }

    public void OnNearInteractable(Collider2D collider) {
        this.GetComponent<Transform>().Rotate(new Vector3(0,0,Time.deltaTime*this.rotationFactor));
    }

    public void Move() {
        Transform transform = this.GetComponent<Transform>();
        if(this.passed < 2.5) {
            transform.position += new Vector3(Time.deltaTime, 0, 0);
        } else if(this.passed < 5) {
            transform.position -= new Vector3(Time.deltaTime, 0, 0);
        } else {
            this.passed = 0;
        }
    }
}
