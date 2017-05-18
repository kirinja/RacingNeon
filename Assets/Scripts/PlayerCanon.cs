using UnityEngine;


public class PlayerCanon : MonoBehaviour
{
    private int _shotsLeft;

    public int Shots = 3;
    public GameObject ShotPrefab;


    // Use this for initialization
    private void Start()
    {
        _shotsLeft = 0;
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Shoot P" + GetComponent<Car>().PlayerId) && _shotsLeft > 0)
            Shoot();
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