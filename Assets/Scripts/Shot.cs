using UnityEngine;


public class Shot : MonoBehaviour
{
    private Timer _lifeTimer;

    public float LifeTime = 10f;
    public int PlayerId;
    public float Speed = 10f;


    // Use this for initialization
    private void Start()
    {
        _lifeTimer = new Timer(LifeTime);
    }


    // Update is called once per frame
    private void Update()
    {
        if (_lifeTimer.Update(Time.deltaTime))
            Destroy(gameObject);

        transform.position += transform.TransformDirection(Vector3.forward * Speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Car>().PlayerId != PlayerId)
        {
            other.GetComponent<Respawner>().Explode();
            Destroy(gameObject);
        }
    }
}