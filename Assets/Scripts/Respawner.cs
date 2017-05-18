using System.Collections;
using UnityEngine;


public class Respawner : MonoBehaviour
{
    private Vector3 _spawnPosition;
    private Timer _orientationTimer;

    public float RaycastDistance = 1f;
    public LayerMask RoadMask;
    public GameObject ExplosionPrefab;
    public float RespawnTime = 1.5f;
    public float UpsideDownToleratedTime = 2f;
    public float ToleratedAngle = 70f;


    // Use this for initialization
    private void Start()
    {
        _spawnPosition = transform.position;
        _orientationTimer = new Timer(UpsideDownToleratedTime);
    }


    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RaycastDistance, RoadMask))
            if (hit.transform.CompareTag("Road"))
                _spawnPosition = transform.position;

        if (Mathf.Abs(transform.rotation.eulerAngles.x) >= ToleratedAngle || Mathf.Abs(transform.rotation.eulerAngles.z) >= ToleratedAngle)
        {
            if (_orientationTimer.Update(Time.deltaTime))
            {
                Explode();
                _orientationTimer.Reset();
            }
        }
        else
        {
            _orientationTimer.Reset();
        }
    }

    
    public void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        SetCarActive(false);
        StartCoroutine(StartRespawning());
    }


    private void SetCarActive(bool active)
    {
        GetComponent<Renderer>().enabled = active;
        GetComponent<Car>().enabled = active;
        GetComponent<Collider>().enabled = active;
        GetComponent<Rigidbody>().isKinematic = !active;
        foreach (var trail in GetComponentsInChildren<TrailRenderer>())
        {
            trail.enabled = active;
        }
    }


    private IEnumerator StartRespawning()
    {
        yield return new WaitForSeconds(RespawnTime);
        Respawn();
    }


    private void Respawn()
    {
        GetComponent<Car>().ResetVelocity();
        SetCarActive(true);
        transform.position = _spawnPosition;
        transform.rotation = Quaternion.identity;
    }
}