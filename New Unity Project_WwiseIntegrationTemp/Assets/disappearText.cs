using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class disappearText : MonoBehaviour {

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			gameObject.SetActive (false);
		}
	}
}
