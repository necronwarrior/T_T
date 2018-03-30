
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_InGameObjectToggler : MonoBehaviour {

	public GameObject[] ActivatedObjects;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.down;

			for(int i=0; i<ActivatedObjects.Length;i++)
			{
				ActivatedObjects [i].SetActive (false);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals ("Human")) {
			transform.position += Vector3.up;
		}
		for(int i=0; i<ActivatedObjects.Length;i++)
		{
			ActivatedObjects [i].SetActive (true);
		}
	}
}
