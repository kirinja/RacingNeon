using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStarter : MonoBehaviour
{
    private Timer _countdownTimer;

    public GameObject[] Players;
    public float CountdownTime = 3f;
    public Text CountdownText;
    public Text[] PlayerTimeTexts;

	// Use this for initialization
	void Start ()
    {
        SetPlayersActive(false);
        _countdownTimer = new Timer(CountdownTime);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_countdownTimer.Update(Time.deltaTime))
        {
            SetPlayersActive(true);
            CountdownText.gameObject.SetActive(false);
        }
        else
        {
            CountdownText.text = Mathf.CeilToInt(CountdownTime - _countdownTimer.TimePassed).ToString();
        }
	}


    private void SetPlayersActive(bool active)
    {
        foreach (var player in Players)
        {
            player.GetComponent<Car>().enabled = active;
            player.GetComponent<CarTimer>().enabled = active;
            player.GetComponent<PlayerCanon>().enabled = active;
        }
        foreach (var text in PlayerTimeTexts)
        {
            text.enabled = active;
        }
    }
}
