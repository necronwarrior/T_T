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
	public GameObject Level1Name;

	private Vector3 offset;         //Private variable to store the offset distance between the player and camera
	private Vector3 TempVec, TempVec2;
	public int SceneSection =0;
	public float[] timings;
	public float TimingDeltatime;
	float rumbleSpeed;
	float rumbleAmount;
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

			rumbleSpeed = 30.0f; //how fast it shakes
			rumbleAmount = 0.03f; //how much it shakes

			VirusContainer.transform.position = new Vector3(VirusContainer.transform.position.x+(Mathf.Sin(Time.time * rumbleSpeed) * rumbleAmount), VirusContainer.transform.position.y+(Mathf.Sin(Time.time * rumbleSpeed) * rumbleAmount), VirusContainer.transform.position.z );

			VirusContainer.transform.position += Vector3.left * 0.2f;
			JetBike.transform.position += Vector3.left * 0.2f;
			break;
		case 1:
			if(ToHoverLerpBool){
				TempVec = Camera.main.transform.position;
				ToHoverLerpBool = false;
			}

			rumbleSpeed = 50.0f; //how fast it shakes
			rumbleAmount = 0.05f; //how much it shakes

			VirusContainer.transform.position = new Vector3(VirusContainer.transform.position.x+(Mathf.Sin(Time.time * rumbleSpeed) * rumbleAmount), VirusContainer.transform.position.y+(Mathf.Sin(Time.time * rumbleSpeed) * rumbleAmount), VirusContainer.transform.position.z );
			

			VirusContainer.transform.position += Vector3.left * 0.3f;
			JetBike.transform.position += Vector3.left * 0.3f;
			// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.

			Camera.main.transform.position = Vector3.Lerp(TempVec, JetBike.transform.position + offset,(TimingDeltatime-timings[0])*5);
			break;
		case 2:
			if (Camera.main.transform.position == JetBike.transform.position + offset) {
				TempVec = JetBike.transform.position + offset;
				VirusContainer.AddComponent (typeof(Rigidbody2D));

				VirusContainer.GetComponent<Rigidbody2D> ().mass =1.0f;
				VirusContainer.GetComponent<Rigidbody2D> ().gravityScale = 1.0f;
			}
			JetBike.transform.position += Vector3.left;
			VirusContainer.transform.Rotate (0.0f, 0.0f, 10.0f);
			Camera.main.transform.position = Vector3.Lerp(TempVec, new Vector3(VirusContainer.transform.position.x, VirusContainer.transform.position.y*1.4f, offset.z),TimingDeltatime-timings[1]);

			Level1Name.transform.position = Vector3.Lerp(Level1Position.transform.position - offset, Camera.main.transform.position - offset,((TimingDeltatime-timings[1])-0.5f)*0.9f);


			break;
		case 3:
			if (Camera.main.transform.position.y > Level1Position.transform.position.y) {
				Camera.main.transform.position = new Vector3 (Level1Position.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
				TempVec = Camera.main.transform.position;
				Destroy (VirusContainer.GetComponent<Rigidbody2D> ());
				VirusContainer.transform.position = new Vector3 (Level1Position.transform.position.x, VirusContainer.transform.position.y, VirusContainer.transform.position.z);
				VirusContainer.transform.rotation = Quaternion.identity;
				TempVec2 = VirusContainer.transform.position;
			}

			if (TimingDeltatime - timings [2] < 0.1f) {
				Level1Name.transform.position = Camera.main.transform.position - offset;
			}

			VirusContainer.transform.position = Vector3.Lerp (TempVec2, Level1Position.transform.position - Vector3.forward, TimingDeltatime - timings [2] - 3.1f);
			Destroy (JetBike);
			Splatman.SetActive (true);
			Camera.main.transform.position = Vector3.Lerp(TempVec, new Vector3(Level1Position.transform.position.x, Level1Position.transform.position.y, offset.z),TimingDeltatime-timings[2]) ;


			break;
		case 4:
			Destroy (Level1Name);

			GameObject Starting_Object = GameObject.Find ("0 - Tutorial Monitor");
			Starting_Object.GetComponent<Infected> ().StartHover ();
			GameObject.FindGameObjectWithTag("ScoreManagerTag").transform.GetChild (0).gameObject.SetActive (true);
			GameObject.FindGameObjectWithTag("ScoreManagerTag").transform.GetChild (1).gameObject.SetActive (true);
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
