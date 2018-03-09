using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;

public class ResolutionChanger : MonoBehaviour {

	public Vector2 resolutionNew; 
	public ScreenOrientation OrientationSwitch;
	// Use this for initialization
	void Start () {
		//Screen.SetResolution ((int)resolutionNew.x, (int)resolutionNew.y, false);
		//Screen.orientation = OrientationSwitch;
		AkSoundEngine.PostEvent ("Play_Ambient", gameObject);
	}
}
