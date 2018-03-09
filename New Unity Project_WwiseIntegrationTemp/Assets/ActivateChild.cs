using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChild : MonoBehaviour {

	void OnMouseDown()
	{
		transform.GetChild (0).transform.gameObject.SetActive (true);
	}

	void Update()
	{
		if (GetComponent<Infected> ().isInfected == true) {
			transform.GetChild (0).transform.gameObject.SetActive (true);
		}
	}
}
