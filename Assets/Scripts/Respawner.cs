using System.Collections;
using UnityEngine;


public class Respawner : MonoBehaviour
{
    private Vector3 _spawnPosition;

    public float RaycastDistance = 1f;
    public LayerMask RoadMask;
    public GameObject ExplosionPrefab;
    public float RespawnTime = 1.5f;


    // Use this for initialization
    private void Start()
    {
        _spawnPosition = transform.position;
    }


    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RaycastDistance, RoadMask))
            if (hit.transform.CompareTag("Road"))
                _spawnPosition = transform.position;
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
    }
}