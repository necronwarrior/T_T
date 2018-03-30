
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_InGameObjectToggler : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.down;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.up;
		}
	}
}
