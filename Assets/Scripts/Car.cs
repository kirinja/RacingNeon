using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    public float Acceleration;
    public float HandleRate;
    //public float TopSpeed;
    public float CrashSpeedTreshold = 10f;
    public int PlayerId;
    public float FrictionDeacceleration = 0.99f;
    public float BrakeDeacceleration = 0.8f;
    public float SidewaysDeacceleration = 0.05f;

    public float TopAccel = 2.0f;
    public float TopBackAccel = 0.5f;

    private float _acceleration;
    private Vector3 _velocity;
    
    public bool ReachedGoal { get; private set; }

    public AudioClip RevvingSound;
    private AudioSource _source;
    public Text WinnerText;

    private Camera _camera;
    public float MinFOV = 80.0f;
    public float MaxFOV = 100.0f;

	// Use this for initialization
	private void Start ()
	{
	    _transform = transform;
	    _rigidbody = GetComponent<Rigidbody>();
	    _source = GetComponent<AudioSource>();
	    _source.clip = RevvingSound;
	    _source.loop = true;
	    _camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	private void FixedUpdate ()
	{
        

		// do input stuff here
        if (Input.GetButton("Accelerate P" + PlayerId))
        {
            // accelerate up to a point
            _acceleration += Acceleration;
            _source.pitch += 0.0125f;
            if (_source.pitch >= 1.5f)
                _source.pitch = 1.5f;
            if (!_source.isPlaying)
                _source.Play();

            float currFov = _camera.fieldOfView;
            currFov += 4.0f * Time.deltaTime;
            if (currFov >= MaxFOV)
                currFov = MaxFOV;

            _camera.fieldOfView = currFov;
        }
        else
        {
            _source.pitch -= 0.01f;
            if (_source.pitch <= -0.4f)
                _source.pitch = -0.4f;

            float currFov = _camera.fieldOfView;
            currFov -= 4.0f * Time.deltaTime;
            if (currFov <= MinFOV)
                currFov = MinFOV;

            _camera.fieldOfView = currFov;
        }
	    if (Input.GetButton("Brake P" + PlayerId))
	    {
	        _acceleration -= Acceleration;
            // TODO: Sound stuff?
	    }
        /*else
        {/**/
            // deaccel
            //_source.Stop();
            _acceleration *= 1 - FrictionDeacceleration * Time.deltaTime;
            _velocity *=
                1 - FrictionDeacceleration *
                Time.deltaTime; // have a value [0,1] depending on current speed compared to top speed
        /*}/**/
        /*if (Input.GetButton("Brake P" + PlayerId) && !Input.GetButton("Accelerate P" + PlayerId))
        {
            _velocity *= 1 - BrakeDeacceleration * Time.deltaTime;
            _acceleration *= 1 - BrakeDeacceleration * Time.deltaTime;
        }/**/

	    var localVelocity = transform.InverseTransformDirection(_velocity);
        localVelocity = new Vector3(localVelocity.x * (1 - SidewaysDeacceleration * Time.deltaTime), localVelocity.y, localVelocity.z);
	    _velocity = transform.TransformDirection(localVelocity);

        if (_acceleration >= TopAccel)
            _acceleration = TopAccel;
        else if (_acceleration < -TopBackAccel)
        {
            _acceleration = -TopBackAccel;
        }
        
        // kinda janky but you cant stand still and turn around anymore
        // want to make it so the turn rate is dependant on the speed (turn slower at lower speeds)
	    //if (_velocity.magnitude > 1f)
	    //{
	        var turnRate = _velocity.magnitude / 11.5f; // hack, we kinda know the max magitude will be 20 or so (can probably calculate it but hardcoded for now)
	        var turning = Input.GetAxis("Horizontal P" + PlayerId);
	        // apply rotation
	        _transform.Rotate(_transform.up, turning * HandleRate * turnRate);
	    //}

	    /*if (transform.InverseTransformDirection(_velocity).z >= TopSpeed)
	    {
	        var localVel = transform.InverseTransformDirection(_velocity);

            localVel = new Vector3(localVel.x, localVel.y, TopSpeed);

	        _velocity = transform.TransformDirection(localVel);
	    }/**/

	    _velocity += transform.forward * _acceleration;
        if (IsGrounded())
        {
            _transform.position += _velocity * Time.deltaTime;
            _rigidbody.position = _transform.position;
        }
    }

    // need to check input

    // need to force car to ground (stronger gravity)

    // need to move the car

    // check if grounded
    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(_transform.position, -_transform.up, out hit, 1.0f);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (_velocity.magnitude >= CrashSpeedTreshold)
            {
                GetComponent<Respawner>().Explode();
            }
            else
            {
                ResetVelocity();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            var otherCar = other.gameObject.GetComponent<Car>();
            var globalVelocity = _velocity;
            var otherGlobalVelocity = otherCar._velocity;
            var velocityDiff = globalVelocity - otherGlobalVelocity;
            if (velocityDiff.magnitude >= CrashSpeedTreshold)
            {
                var slowest = _velocity.magnitude < otherCar._velocity.magnitude ? this : otherCar;
                slowest.GetComponent<Respawner>().Explode();
            }
            else
            {
                // TODO: Find some smart way to only reset velocity on the car behind or something
                // ResetVelocity();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Goal"))
        {
            ReachedGoal = true;
            // get other players and see if we won
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in players)
            {
                if (go != gameObject)
                {
                    if (!go.GetComponent<Car>().ReachedGoal)
                        WinnerText.text = "PLAYER " + PlayerId + " WINS";
                    else
                    {
                        // both players have finished the race
                        // do something
                        // go back to select or something?
                        Debug.Log("båda spelarna har kommit i mål");
                    }
                }
            }
        }
    }


    public void ResetVelocity()
    {
        _velocity = Vector3.zero;
        _acceleration = 0f;
    }

    // need a reset method
}
