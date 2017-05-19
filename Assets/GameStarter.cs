using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStarter : MonoBehaviour
{
    private Timer _countdownTimer;
    private GameObject[] _players;

    public Transform Player1Position, Player2Position;
    public GameObject Player1DefaultPrefab, Player2DefaultPrefab;
    public float CountdownTime = 3f;
    public Text CountdownText;
    public Text[] PlayerTimeTexts;

	// Use this for initialization
	void Start ()
	{
        _players = new GameObject[2];
	    var gmo = GameObject.FindGameObjectWithTag("Game Manager");
	    if (gmo)
	    {
	        var gm = gmo.GetComponent<GameManager>();
	        if (gm)
	        {
	            var player1Prefab = gm.Player1Prefab == null ? Player1DefaultPrefab : gm.Player1Prefab;
	            var player2Prefab = gm.Player2Prefab == null ? Player2DefaultPrefab : gm.Player2Prefab;
	            _players[0] = Instantiate(player1Prefab, Player1Position.position, Player1Position.rotation);
                
	            _players[1] = Instantiate(player2Prefab, Player2Position.position, Player2Position.rotation);
	        }
	    }
	    else
	    {
	        _players[0] = Instantiate(Player1DefaultPrefab, Player1Position.position, Player1Position.rotation);
	        _players[1] = Instantiate(Player2DefaultPrefab, Player2Position.position, Player2Position.rotation);
        }

	    _players[0].GetComponent<Car>().PlayerId = 1;
	    _players[1].GetComponent<Car>().PlayerId = 2;
	    _players[0].GetComponent<CarUI>().TimeText = PlayerTimeTexts[0];
	    _players[1].GetComponent<CarUI>().TimeText = PlayerTimeTexts[1];

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
            //gameObject.SetActive(false);
            enabled = false;
        }
        else
        {
            CountdownText.text = Mathf.CeilToInt(CountdownTime - _countdownTimer.TimePassed).ToString();
        }
	}


    private void SetPlayersActive(bool active)
    {
        foreach (var player in _players)
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
