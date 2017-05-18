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

    public float TopAccel = 2.0f;

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
        if (Input.GetButton("Jump"))
        {
            // accelerate up to a point
            _acceleration += Acceleration;
        }
        else
        {
            // deaccel
            _acceleration *= 0.99f;
            _velocity *= 0.99f; // have a value [0,1] depending on current speed compared to top speed
        }
        if (Input.GetAxisRaw("Vertical") < -0.25f && !Input.GetButton("Jump"))
        {
            _velocity *= 0.80f;
            _acceleration *= 0.80f;
        }

        if (_acceleration >= TopAccel)
            _acceleration = TopAccel;

        var turning = Input.GetAxisRaw("Horizontal");
        // apply rotation
        _transform.Rotate(_transform.up, turning * HandleRate);

        if (_acceleration >= TopSpeed)
            _acceleration = TopSpeed;

        _velocity += _acceleration;
        if (IsGrounded())
        {
            _transform.position += _transform.forward * _velocity * Time.deltaTime;
            _rigidbody.position = _transform.position;
        }
    }

    // need to check input

    // need to force car to ground (stronger gravity)

    // need to move the car

    // check if grounded
    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, 1.0f))
        {
            return true;
        }

        return false;
    }

    // need a reset method
}
