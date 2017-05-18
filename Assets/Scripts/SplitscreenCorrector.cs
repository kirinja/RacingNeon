using UnityEngine;

public class SplitscreenCorrector : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var playerId = GetComponent<Car>().PlayerId;

	    if (playerId == 1)
	    {
	        GetComponentInChildren<Camera>().rect = new Rect(0f, 0.5f, 1f, 0.5f);
	    }
        else if (playerId == 2)
	    {
	        GetComponentInChildren<Camera>().rect = new Rect(0f, 0f, 1f, 0.5f);
        }
	}
}
