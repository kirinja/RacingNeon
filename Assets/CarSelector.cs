using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CarSelector : MonoBehaviour
{
    private int _p1Index, _p2Index;
    private bool _p1CanMove, _p2CanMove;
    private bool _p1Ready, _p2Ready;

    public GameObject[] Player1Indicators, Player2Indicators;
    public GameObject[] CarPrefabs;
    public Text P1ReadyText, P2ReadyText;


	// Use this for initialization
	void Start ()
	{
	    _p1CanMove = true;
	    _p2CanMove = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxisRaw("Vertical P1") < -0.5f && _p1CanMove)
	    {
	        _p1Index++;
	        if (_p1Index >= Player1Indicators.Length) _p1Index = Player1Indicators.Length - 1;
	        else
	        {
	            StartCoroutine(EnableP1Input());
	        }
	    }
	    if (Input.GetAxisRaw("Vertical P1") > 0.5f && _p1CanMove)
	    {
	        _p1Index--;
	        if (_p1Index < 0) _p1Index = 0;
	        else
	        {
	            StartCoroutine(EnableP1Input());
	        }
	    }

	    if (Input.GetAxisRaw("Vertical P2") < -0.5f && _p2CanMove)
	    {
	        _p2Index++;
	        if (_p2Index >= Player2Indicators.Length) _p2Index = Player2Indicators.Length - 1;
	        else
	        {
	            StartCoroutine(EnableP2Input());
	        }
	    }
	    if (Input.GetAxisRaw("Vertical P2") > 0.5f && _p2CanMove)
	    {
	        _p2Index--;
	        if (_p2Index < 0) _p2Index = 0;
	        else
	        {
	            StartCoroutine(EnableP2Input());
	        }
	    }

        ScaleIndicators();

	    if (Input.GetButtonDown("Start P1"))
	    {
	        _p1Ready = true;
	        _p1CanMove = false;
	        P1ReadyText.enabled = true;
	    }
	    if (Input.GetButtonDown("Start P2"))
	    {
	        _p2Ready = true;
	        _p2CanMove = false;
            P2ReadyText.enabled = true;
	    }

	    if (_p1Ready && _p2Ready)
	    {
	        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().Player1Prefab =
	            CarPrefabs[_p1Index];
	        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().Player2Prefab =
	            CarPrefabs[_p2Index];
            // TODO: Load correct scene here?
            SceneManager.LoadScene(GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().NextLevel);
	    }
    }


    private void ScaleIndicators()
    {
        for(var i = 0; i < Player1Indicators.Length; ++i)
        {
            Player1Indicators[i].transform.localScale = i == _p1Index ? Vector3.one * 1.5f : Vector3.one;
        }

        for (var i = 0; i < Player2Indicators.Length; ++i)
        {
            Player2Indicators[i].transform.localScale = i == _p2Index ? Vector3.one * 1.5f : Vector3.one;
        }
    }


    private IEnumerator EnableP1Input()
    {
        _p1CanMove = false;
        yield return new WaitForSeconds(0.5f);

        _p1CanMove = true;
    }


    private IEnumerator EnableP2Input()
    {
        _p2CanMove = false;
        yield return new WaitForSeconds(0.5f);

        _p2CanMove = true;
    }
}
