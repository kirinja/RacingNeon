using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJitterShit : MonoBehaviour
{
    private Renderer _renderer;
    public Texture[] Textures;
    private Timer _timer;
    private int _cnt;

	// Use this for initialization
	void Start ()
	{
	    _renderer = GetComponent<Renderer>();
	    _renderer.sharedMaterial.mainTexture = Textures[_cnt];
        _timer = new Timer(0.1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_timer.Update(Time.deltaTime))
        {
            _cnt++;
            if (_cnt >= Textures.Length)
                _cnt = 0;
            _renderer.sharedMaterial.mainTexture = Textures[_cnt];

            _timer.Reset();
        }
	}
}
