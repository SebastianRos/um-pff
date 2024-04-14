using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoning : MonoBehaviour
{
    public GameObject summonedDuck = null;
    // Start is called before the first frame update
    void Start()
    {
        if(this.summonedDuck) {
            this.summonedDuck.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
