using UnityEngine;


public class PlayerCanon : MonoBehaviour
{
    private int _shotsLeft;
    private float _shootInput;

    public int Shots = 3;
    public GameObject ShotPrefab;


    // Use this for initialization
    private void Start()
    {
        _shotsLeft = 0;
        _shootInput = 1f;
    }


    // Update is called once per frame
    private void Update()
    {
        if (_shootInput <= float.Epsilon)
        {
            var shoot = Input.GetAxisRaw("Shoot P" + GetComponent<Car>().PlayerId) > float.Epsilon;
            if (shoot) Shoot();
        }
        _shootInput = Input.GetAxisRaw("Shoot P" + GetComponent<Car>().PlayerId);
    }


    private void Shoot()
    {
        _shotsLeft--;
        var shot = Instantiate(ShotPrefab, transform.position, transform.rotation);
        shot.GetComponent<Shot>().PlayerId = GetComponent<Car>().PlayerId;
    }


    public void Activate()
    {
        _shotsLeft = Shots;
    }


    public void Reset()
    {
        _shotsLeft = 0;
    }
}