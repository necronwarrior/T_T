using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infected : MonoBehaviour 
{
	public float IncubationPeriod;
	public float Radius;
	public string InfectionSpritesheet;

	public int ScoreValue;

	Vector2 OldInfectorPos;
	Sprite[] InfectedSpritesheet;
	ScoreManager Scorepoints;


	public AudioClip soundHumanInfected;
	public AudioClip soundMachineInfected;

	public bool isInfected;
	private AudioSource source;

	// Use this for initialization
	void Start () 
	{
		isInfected = false;
		Scorepoints = GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ();
		//set player to the 'healthy'sprite
		if(InfectionSpritesheet!="NULL"){
		InfectedSpritesheet = Resources.LoadAll<Sprite> (InfectionSpritesheet);
		}

		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SetInfected(Vector2 InfectorPos)
	{
		OldInfectorPos = InfectorPos;
		isInfected = true;
		//disable movement
		if (gameObject.tag =="Human") {
			GetComponent<Move> ().enabled = false;
			source.PlayOneShot (soundHumanInfected);
			GetComponent<Animator> ().enabled = false;
		}
		if (gameObject.tag == "Technology") {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		//begin the infection countdown
		StartCoroutine (Infection());	
		//and animate reticle
		if (InfectionSpritesheet != "NULL") { 
			StartCoroutine (ReticleCountdown ());
		} else {
			StartCoroutine (ReticleCountdown2 ());
		}
	}

	IEnumerator ReticleCountdown2()
	{
		GameObject BR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BR"));
		BR_new.transform.parent = gameObject.transform;
		BR_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 1.0f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 0.0f,
			0.0f);

		GameObject BL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BL"));
		BL_new.transform.parent = gameObject.transform;
		BL_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 0.0f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 0.0f,
			0.0f);

		GameObject TR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TR"));
		TR_new.transform.parent = gameObject.transform;
		TR_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 1.0f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 1.0f,
			0.0f);

		GameObject TL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TL"));
		TL_new.transform.parent = gameObject.transform;
		TL_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 0.0f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 1.0f,
			0.0f);

		yield return new WaitForSeconds (IncubationPeriod);

		Destroy (BR_new);
		Destroy (BL_new);
		Destroy (TR_new);
		Destroy (TL_new);
	}

	IEnumerator ReticleCountdown()
	{
		GameObject BR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BR"));
		BR_new.transform.parent = gameObject.transform;
		BR_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 0.5f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * -0.5f,
			0.0f);

		GameObject BL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BL"));
		BL_new.transform.parent = gameObject.transform;
		BL_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * -0.5f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * -0.5f,
			0.0f);

		GameObject TR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TR"));
		TR_new.transform.parent = gameObject.transform;
		TR_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * 0.5f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 0.5f,
			0.0f);

		GameObject TL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TL"));
		TL_new.transform.parent = gameObject.transform;
		TL_new.transform.localPosition = new Vector3( gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * -0.5f,
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * 0.5f,
			0.0f);

		yield return new WaitForSeconds (IncubationPeriod);

		Destroy (BR_new);
		Destroy (BL_new);
		Destroy (TR_new);
		Destroy (TL_new);
	}

	IEnumerator Infection()
	{


		//animate over a specified period of time 
		if (InfectionSpritesheet != "NULL") {
			for (int i = 0; i < InfectedSpritesheet.Length; ++i) {
				yield return new WaitForSeconds ((float)(IncubationPeriod / InfectedSpritesheet.Length));
				gameObject.GetComponent<SpriteRenderer> ().sprite = InfectedSpritesheet [i];
			}
		} else {
			GetComponent<SpriteRoots>().Begin(OldInfectorPos);
			yield return new WaitForSeconds ((float)IncubationPeriod);
		}

		Scorepoints.AddScore (ScoreValue, transform.position);

		if (gameObject.tag == "Human") {
			GameObject Explosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/InfectedExplosion"));
			Explosion.transform.position = transform.position;

			AkSoundEngine.PostEvent("Human_Contraction",gameObject);

			if (GetComponentInChildren<PolygonCollider2D> () == true) {
				Explosion.GetComponent<InfectionTriggerChild> ().Explode (GetComponentInChildren<PolygonCollider2D> ());
			} else {
				CircleCollider2D newCircle = gameObject.AddComponent<CircleCollider2D> ();
				newCircle.isTrigger = true;
				newCircle.radius = Radius;
				Explosion.GetComponent<InfectionTriggerChild> ().Explode (newCircle );
			}
			GameObject GExplosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/GreenExplosion"));
			GExplosion.transform.position = transform.position;



			Destroy (gameObject);
		}
		if (gameObject.tag == "Technology") {

			//AkSoundEngine.PostEvent("Buzz",gameObject);

			if (InfectionSpritesheet != "NULL") {
				gameObject.GetComponent<SpriteRenderer> ().sprite = InfectedSpritesheet [InfectedSpritesheet.Length - 1];
			}

			GameObject TExplosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/InfectedTechExplosion"));
			TExplosion.transform.position = transform.position;
			if (GetComponentInChildren<PolygonCollider2D> () == true) {
				TExplosion.GetComponent<InfectionTriggerChild> ().Explode (GetComponentInChildren<PolygonCollider2D> ());
			} else {
				//CircleCollider2D newCircle = gameObject.AddComponent<CircleCollider2D> ();
				//newCircle.radius = Radius;
				TExplosion.GetComponent<InfectionTriggerChild> ().Explode (transform.GetChild(0).GetComponent<CircleCollider2D>());
			}


		}

	}

	void OnMouseDown()
	{
		if (Scorepoints.firstTouch == false)
		{
			SetInfected (new Vector2(0,0));
			Scorepoints.firstTouch = true;
			//GameObject.FindGameObjectWithTag ("LIGHTNING").GetComponent<Animator> ().enabled = true;

//			Camera.main.GetComponent<AudioSource> ().clip = ((AudioClip)Resources.Load ("FX_Lightning_Click") as AudioClip);
//			Camera.main.GetComponent<AudioSource> ().Play ();
			//AkSoundEngine.StopAll ();
			//AkSoundEngine.PostEvent ("VirusTap", gameObject);

	//		Camera.main.GetComponent<AudioSource> ().clip = ((AudioClip)Resources.Load ("Music_Intense") as AudioClip);
	//		Camera.main.GetComponent<AudioSource> ().Play ();
		}
	}
}
