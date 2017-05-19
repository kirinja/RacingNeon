using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CitySelector : MonoBehaviour
{
    private int _selectedIndex;

    public Text Lvl1Text, Lvl2Text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxisRaw("Vertical P1") < -0.5f || Input.GetAxisRaw("Vertical P2") < -0.5f)
	    {
	        _selectedIndex++;
	        if (_selectedIndex >= 2) _selectedIndex = 1;
	    }
	    if (Input.GetAxisRaw("Vertical P1") > 0.5f || Input.GetAxisRaw("Vertical P2") > 0.5f)
	    {
	        _selectedIndex--;
	        if (_selectedIndex < 0) _selectedIndex = 0;
	    }

	    Lvl1Text.fontSize = _selectedIndex == 0 ? 75 : 50;
	    Lvl2Text.fontSize = _selectedIndex == 1 ? 75 : 50;

	    if (Input.GetButtonDown("Start P1") || Input.GetButtonDown("Start P2"))
	    {
	        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().NextLevel = _selectedIndex == 0 ? "Bana1" : "Bana2";
            SceneManager.LoadScene("CarSelect");
	    }
    }
}
