using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class deals with the infection of all objects. It contains:
 * >Reticle placement of currently infected
 * >Animation calls to SpriteRoots
 * >Creation of explosions (infectors of other objects)
 * >Passing of points gained to Scorepoints
 * >Handling of initial click/touch to start game.
 * */

public class Infected : MonoBehaviour 
{
	ScoreManager Scorepoints;						//Reference to the overall score held in the UI 
	public int ScoreValue = 0;						//Score granted by infecting this object

	public float ExplosionRadius = 1.0f;			//Size of the infection explosion (if there is no polygon collider)
	public float IncubationPeriod = 1.0f;			//How long until the infection explosion is called

	Vector2 OldInfectorPos;							//Position of the object infection the current one.

	//Spritesheet is mainly used for human infectees
	public string InfectionSpritesheet = "NULL";	//file location of spritesheet to be used. - This is defaulted to NULL if there is none, in which case SpriteRoots happens.
	Sprite[] InfectedSpritesheet;					//actual spritesheet file

	void Start () 
	{
		//Get reference to the UI element that contains the score
		Scorepoints = GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ();

		//Fill the spritesheet with data, if it exists
		if(InfectionSpritesheet!="NULL"){
			InfectedSpritesheet = Resources.LoadAll<Sprite> (InfectionSpritesheet);
		}

		//And Create reticle 
		if (InfectionSpritesheet == "NULL") { 

			//Origin at (0,0)
			StartCoroutine (ReticleHover (0.0f, 1.0f));
		} else {

			//Spritesheet has its origin at (0.5,0.5) 						// FIXME todo
			StartCoroutine (ReticleHover (-0.5f,0.5f));
		}
	}

	IEnumerator ReticleHover(float LowerBounds, float UpperBounds){

		float widthoffset = 0;
		float heightoffset = 0;

		if (gameObject.GetComponent<SpriteRenderer> ().flipX == true) {
			widthoffset = gameObject.GetComponent<SpriteRenderer> ().bounds.size.x;
		}
		if (gameObject.GetComponent<SpriteRenderer> ().flipY == true) {
			heightoffset = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		}

		//Create and place 'Bottom Right' reticle
		GameObject BR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BR"));
		BR_new.transform.parent = gameObject.transform;
		BR_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * UpperBounds - widthoffset,	//Right
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * LowerBounds - heightoffset,	//Bottom
			0.0f);

		//Create and place 'Bottom Left' reticle
		GameObject BL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BL"));
		BL_new.transform.parent = gameObject.transform;
		BL_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * LowerBounds - widthoffset, //Left
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * LowerBounds - heightoffset, //Bottom
			0.0f);

		//Create and place 'Top Right' reticle
		GameObject TR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TR"));
		TR_new.transform.parent = gameObject.transform;
		TR_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * UpperBounds - widthoffset, //Right
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * UpperBounds - heightoffset, //Top
			0.0f);

		//Create and place 'Top Left' reticle
		GameObject TL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TL"));
		TL_new.transform.parent = gameObject.transform;
		TL_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * LowerBounds - widthoffset,	//Left
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * UpperBounds - heightoffset,	//Top
			0.0f);
		
		
		//Values to make the reticle pulse in and out
		int IncrementerVal = 60;
		float IncrementVal = -0.004f;

		//until the first tap has completed, pulse 
		while (Scorepoints.firstTouch == false) {

			//move based on up and downs
			BR_new.transform.localPosition = new Vector3( 
				BR_new.transform.localPosition.x + IncrementVal,	//Right
				BR_new.transform.localPosition.y - IncrementVal,	//Bottom
				-1.0f);

			BL_new.transform.localPosition = new Vector3( 
				BL_new.transform.localPosition.x - IncrementVal,	//Left
				BL_new.transform.localPosition.y - IncrementVal,	//Bottom
				-1.0f);

			TR_new.transform.localPosition = new Vector3( 
				TR_new.transform.localPosition.x + IncrementVal,	//Right
				TR_new.transform.localPosition.y + IncrementVal,	//Top
				-1.0f);

			TL_new.transform.localPosition = new Vector3( 
				TL_new.transform.localPosition.x - IncrementVal,	//Left
				TL_new.transform.localPosition.y + IncrementVal,	//Top
				-1.0f);

			//count up and down
			if (IncrementVal > 0) {
				IncrementerVal++;
			} else {
				IncrementerVal--;
			}

			//magic numbers OwO counting based on value for up and down
			if (IncrementerVal > 60) {
				IncrementVal = -0.004f;
			}
			if (IncrementerVal < 0) {
				IncrementVal = 0.004f;
			}

			yield return new WaitForSeconds(0.012f);
		}

		//Remove reticles
		Destroy (BR_new);
		Destroy (BL_new);
		Destroy (TR_new);
		Destroy (TL_new);
	}

	//Called externally to begin multiple infection processes
	public void SetInfected(Vector2 InfectorPos)
	{
		//Disable movement and animation
		if (gameObject.tag =="Human") {
			GetComponent<Move> ().enabled = false;
			GetComponent<Animator> ().enabled = false;
		}

		//Disable own boxcollider to prevent accidental collisions.
		GetComponent<BoxCollider2D> ().enabled = false;

		//begin the infection countdown
		StartCoroutine (Infection());	

		//And Create reticle 
		if (InfectionSpritesheet == "NULL") { 
			
			//Get data of prior infector for SpriteRoots
			OldInfectorPos = InfectorPos;

			//Origin at (0,0)
			StartCoroutine (ReticleCountdown (0.0f, 1.0f));
		} else {
			
			//Spritesheet has its origin at (0.5,0.5) 						// FIXME todo
			StartCoroutine (ReticleCountdown (-0.5f,0.5f));
		}
	}

	/*Placement of reticle 													/*FIXME for
	* flipped sprites
	* design descisons as to whether this exists at the start of the game
	* cleanup for lack of repeated code
	* animations?
	**/
	IEnumerator ReticleCountdown(float LowerBounds, float UpperBounds)
	{
		float widthoffset = 0;
		float heightoffset = 0;

		if (gameObject.GetComponent<SpriteRenderer> ().flipX == true) {
			widthoffset = gameObject.GetComponent<SpriteRenderer> ().bounds.size.x;
		}
		if (gameObject.GetComponent<SpriteRenderer> ().flipY == true) {
			heightoffset = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		}

		//Create and place 'Bottom Right' reticle
		GameObject BR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BR"));
		BR_new.transform.parent = gameObject.transform;
		BR_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * UpperBounds - widthoffset,	//Right
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * LowerBounds - heightoffset,	//Bottom
			0.0f);

		//Create and place 'Bottom Left' reticle
		GameObject BL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/BL"));
		BL_new.transform.parent = gameObject.transform;
		BL_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * LowerBounds - widthoffset, //Left
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * LowerBounds - heightoffset, //Bottom
			0.0f);

		//Create and place 'Top Right' reticle
		GameObject TR_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TR"));
		TR_new.transform.parent = gameObject.transform;
		TR_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * UpperBounds - widthoffset, //Right
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * UpperBounds - heightoffset, //Top
			0.0f);

		//Create and place 'Top Left' reticle
		GameObject TL_new = (GameObject)Instantiate((Object)Resources.Load ("Reticle/TL"));
		TL_new.transform.parent = gameObject.transform;
		TL_new.transform.localPosition = new Vector3( 
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.x * LowerBounds - widthoffset,	//Left
			gameObject.GetComponent<SpriteRenderer> ().bounds.size.y * UpperBounds - heightoffset,	//Top
			0.0f);

		//Time until explosion
		yield return new WaitForSeconds (IncubationPeriod);

		//Remove reticles
		Destroy (BR_new);
		Destroy (BL_new);
		Destroy (TR_new);
		Destroy (TL_new);
	}

	/* Main messenger for exposion
	 * >it *feels* like there might be a more efficient way to do this. Still not the happiest with changing polygon shapes on a per-item basis
	 * */
	IEnumerator Infection()
	{

		//Determine whether to animate on a spritesheet or not
		if (InfectionSpritesheet == "NULL") {
			
			//Begin SpriteRoots with data of infector
			GetComponent<SpriteRoots>().Begin(OldInfectorPos);
			//wait the overall infection time before exploding
			yield return new WaitForSeconds ((float)IncubationPeriod);
		} else {

			//Run through all frames at a speed determined by the length of infectiontime
			for (int i = 0; i < InfectedSpritesheet.Length; ++i) {
				//wait the proportianal amount of time for each frame of anmation 
				yield return new WaitForSeconds ((float)(IncubationPeriod / InfectedSpritesheet.Length));
				gameObject.GetComponent<SpriteRenderer> ().sprite = InfectedSpritesheet [i];
			}
		}

		//increment the score by this objects value
		Scorepoints.AddScore (ScoreValue, transform.position);

		//Determine if the explosion should be human (for graphical sakes)
		if (gameObject.tag == "Human") {

			//Create and centre explosion
			GameObject Explosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/InfectedExplosion"));
			Explosion.transform.position = transform.position;

			//green explosion for flavour
			GameObject GExplosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/GreenExplosion"));
			GExplosion.transform.position = transform.position;

			//Play horrible screamng sound
			AkSoundEngine.PostEvent("Human_Contraction",gameObject);

			//Determine how to handle explosion shape 
			if (GetComponentInChildren<PolygonCollider2D> () == true) {
				//For obscure polygons
				Explosion.GetComponent<InfectionTriggerChild> ().Explode (GetComponentInChildren<PolygonCollider2D> ());
			} else {
				//Generate circle collider for default explosion
				CircleCollider2D newCircle = gameObject.AddComponent<CircleCollider2D> ();
				newCircle.isTrigger = true;
				newCircle.radius = ExplosionRadius;
				Explosion.GetComponent<InfectionTriggerChild> ().Explode (newCircle );
			}

			//destroy the object
			Destroy (gameObject);
		}

		//same determination for tech
		if (gameObject.tag == "Technology") {

			//machine explosion is invisible but required for spreading infection
			GameObject TExplosion = (GameObject)Instantiate ((Object)Resources.Load ("Prefabs/InfectedTechExplosion"));
			TExplosion.transform.position = new Vector3(transform.position.x + (gameObject.GetComponent<SpriteRenderer> ().bounds.size.x* 0.5f), 
				transform.position.y + (gameObject.GetComponent<SpriteRenderer> ().bounds.size.y* 0.5f), 
				transform.position.z);

			//Machine noises of death zap
			AkSoundEngine.PostEvent("Buzz",gameObject);

			//determine explosion shape, similarly
			if (GetComponentInChildren<PolygonCollider2D> () == true) {
				TExplosion.GetComponent<InfectionTriggerChild> ().Explode (GetComponentInChildren<PolygonCollider2D> ());
			} else {
				CircleCollider2D newCircle = gameObject.AddComponent<CircleCollider2D> ();
				newCircle.isTrigger = true;
				newCircle.radius = ExplosionRadius;
				TExplosion.GetComponent<InfectionTriggerChild> ().Explode (newCircle);
			}
		}
	}

	//initial infection 
	void OnMouseDown()
	{
		//bool for determining first touch
		if (Scorepoints.firstTouch == false)
		{
			//spawn in center of object
			SetInfected (new Vector2(0,0));
			//set bool
			Scorepoints.firstTouch = true;
			//play dynamic action music
			AkSoundEngine.StopAll ();
			AkSoundEngine.PostEvent ("VirusTap", gameObject);
		}
	}
}
