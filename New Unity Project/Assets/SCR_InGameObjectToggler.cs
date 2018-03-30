
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_InGameObjectToggler : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.down;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.up;
		}
	}
}
