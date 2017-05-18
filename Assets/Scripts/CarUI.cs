using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    private Car _car;
    public Text TimeText;
    public Text AttackText;
	// Use this for initialization
	void Start ()
	{
	    _car = GetComponent<Car>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    TimeText.text = FormatTime(_car._currentTime);
	}

    private string FormatTime(float time)
    {
        var minutes = Mathf.Floor(time / 60.0f);

        var seconds = Mathf.Floor(time - minutes * 60.0f);

        var milliseconds = time - Mathf.Floor(time);

        milliseconds = Mathf.Floor(milliseconds * 1000.0f);



        var sMinutes = "00" + minutes.ToString();

        sMinutes = sMinutes.Substring(sMinutes.Length - 2);

        var sSeconds = "00" + seconds.ToString();

        sSeconds = sSeconds.Substring(sSeconds.Length - 2);

        var sMilliseconds = "000" + milliseconds.ToString();

        sMilliseconds = sMilliseconds.Substring(sMilliseconds.Length - 3);



        var timeText = sMinutes + ":" + sSeconds + ":" + sMilliseconds;

        return timeText;
        
    }
}
