using System.Collections;
using UnityEngine;


public class CanonPickup : MonoBehaviour
{
    public float RespawnTime = 3f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCanon>().Activate();
            SetPickupActive(false);
            StartCoroutine(StartRespawn());
        }
    }


    private IEnumerator StartRespawn()
    {
        yield return new WaitForSeconds(RespawnTime);

        SetPickupActive(true);
    }


    private void SetPickupActive(bool active)
    {
        GetComponent<Collider>().enabled = active;
        GetComponent<Renderer>().enabled = active;
    }
}