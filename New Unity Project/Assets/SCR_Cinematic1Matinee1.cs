using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script controls the camera during the first cinematic with the jetbike
 * */
public class SCR_Cinematic1Matinee1 : MonoBehaviour {

	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = Camera.main.transform.position - transform.position;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		transform.position += Vector3.left*0.1f;
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Camera.main.transform.position = transform.position + offset;

	}
}
