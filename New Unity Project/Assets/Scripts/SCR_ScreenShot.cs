﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ScreenShot : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			Debug.Log ("screenshot taken");
			ScreenCapture.CaptureScreenshot ("Assets/Image.png", 1);
		}
	}
}
