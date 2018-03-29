using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script controls the camera during the first cinematic with the jetbike
 * */
public class SCR_Cinematic1Matinee1 : MonoBehaviour {

	public GameObject JetBike;
	public GameObject VirusContainer;
	public GameObject Level1Position;
	public GameObject Splatman;

	private Vector3 offset;         //Private variable to store the offset distance between the player and camera
	private Vector3 TempVec;
	public int SceneSection =0;
	public float[] timings;
	public float TimingDeltatime;

	bool ToHoverLerpBool;

	// Use this for initialization
	void Start () 
	{
		ToHoverLerpBool = true;
		TimingDeltatime = 0.0f;
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = new Vector3(0.0f,0.0f,-10.0f);
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		TimingDeltatime += Time.deltaTime;
		switch(SceneSection)
		{ 
		case 0:
			Camera.main.transform.position += Vector3.left * 0.165f;
			VirusContainer.transform.position += Vector3.left * 0.2f;
			JetBike.transform.position += Vector3.left * 0.2f;
			break;
		case 1:
			if(ToHoverLerpBool){
				TempVec = Camera.main.transform.position;
				ToHoverLerpBool = false;
			}
			VirusContainer.transform.position += Vector3.left * 0.3f;
			JetBike.transform.position += Vector3.left * 0.3f;
			// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.

			Camera.main.transform.position = Vector3.Lerp(TempVec, JetBike.transform.position + offset,(TimingDeltatime-timings[0])*5);
			break;
		case 2:
			if(Camera.main.transform.position == JetBike.transform.position + offset){
				TempVec = JetBike.transform.position + offset;
				VirusContainer.AddComponent (typeof(Rigidbody2D));
			}
			JetBike.transform.position += Vector3.left;
			Camera.main.transform.position = Vector3.Lerp(TempVec, new Vector3(VirusContainer.transform.position.x, VirusContainer.transform.position.y*1.4f, offset.z),TimingDeltatime-timings[1]) ;
			break;
		case 3:
			if (Camera.main.transform.position.y > Level1Position.transform.position.y) {
				Camera.main.transform.position = new Vector3 (-160.0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
				TempVec = Camera.main.transform.position;
			}
			VirusContainer.transform.position = new Vector3 (-160.0f, VirusContainer.transform.position.y, VirusContainer.transform.position.z);
			Destroy (JetBike);
			Splatman.SetActive (true);
			Camera.main.transform.position = Vector3.Lerp(TempVec, new Vector3(Level1Position.transform.position.x, Level1Position.transform.position.y, offset.z),TimingDeltatime-timings[2]) ;
			break;
		case 4:
			Destroy (this);
			break;
		}

		if(timings.Length > SceneSection){
			if(TimingDeltatime>timings[SceneSection]){
				SceneSection++;
			}
		}

	}
}
