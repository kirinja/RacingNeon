using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class TrailRendererController : MonoBehaviour
{
    public CarController CarController;
    private TrailRenderer _renderer;

	// Use this for initialization
	void Start ()
	{
	    _renderer = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (CarController.CurrentSpeed > 0.0f)
        {
            _renderer.enabled = true;
        }
        else if (CarController.CurrentSpeed <= 0.01f)
        {
            _renderer.enabled = false;
        }
    }
}
