using UnityEngine;


public class Respawner : MonoBehaviour
{
    private Vector3 _spawnPosition;

    public float RaycastDistance = 1f;
    public LayerMask RoadMask;


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


    public void Respawn()
    {
        // Do some more stuff here

        transform.position = _spawnPosition;
    }
}