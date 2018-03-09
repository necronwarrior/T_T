using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionChanger : MonoBehaviour {

	public Vector2 resolutionNew; 
	public ScreenOrientation OrientationSwitch;
	// Use this for initialization
	void Awake () {
		//Screen.SetResolution ((int)resolutionNew.x, (int)resolutionNew.y, false);
		//Screen.orientation = OrientationSwitch;
	}
}
