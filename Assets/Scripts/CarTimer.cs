using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTimer : MonoBehaviour
{
    public float CurrentTime { get; private set; }

	// Use this for initialization
	void Start ()
	{
	    CurrentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!GetComponent<Car>().ReachedGoal)
	        CurrentTime += Time.deltaTime;
    }
}
