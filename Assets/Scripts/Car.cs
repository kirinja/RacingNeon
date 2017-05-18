using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    public float Acceleration;
    public float HandleRate;
    public float TopSpeed;
    public int PlayerId;

    private float _acceleration;
    private float _velocity;

	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		// do input stuff here
        if (Input.GetButton("Accelerate P" + PlayerId))
        {
            // accelerate up to a point
            _acceleration += Acceleration;
        }
        else
        {
            // deaccel
            _acceleration *= 0.95f;
            _velocity *= 0.75f; // have a value [0,1] depending on current speed compared to top speed
        }
        if (Input.GetAxisRaw("Vertical P" + PlayerId) < -0.25f && !Input.GetButton("Accelerate P" + PlayerId))
        {
            Debug.Log("Holding down");
            //_velocity *= 0.80f;
            _acceleration *= 0.80f;
        }

        var turning = Input.GetAxisRaw("Horizontal P" + PlayerId);
        // apply rotation
        _transform.Rotate(_transform.up, turning);

        if (_acceleration >= TopSpeed)
            _acceleration = TopSpeed;

        _velocity += _acceleration;

        _transform.position += _transform.forward * _velocity * Time.deltaTime;
    }
}
