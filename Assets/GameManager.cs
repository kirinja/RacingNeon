using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject Player1Prefab, Player2Prefab;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("CarSelect");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
