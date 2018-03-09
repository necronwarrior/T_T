using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;

public class resetPrefs : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();
		AkSoundEngine.PostEvent ("Play_Theme", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
