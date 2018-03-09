using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour 
{
	Animator WalkingAnimator;

	float timerlerp, timeralterer;

	public bool IdleOnly;

	public GameObject leftPoint;
	public GameObject rightPoint;

	// Use this for initialization
	public float smooth; //= 0.5f;

	void Awake () 
	{
		WalkingAnimator = GetComponent<Animator> ();
		if (IdleOnly == true) {
			WalkingAnimator.SetBool ("IsWalking", false);
			GetComponent<Move> ().enabled = false;
		} else {
			

			WalkingAnimator.SetBool ("IsWalking", true);
			WalkingAnimator.SetBool ("LeftFacing", false);
			timeralterer = Time.deltaTime * smooth;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		PositionChanging ();
	}

	void PositionChanging()
	{
		

		//Vector3 positionA = new Vector3 (-1, 0, 0);
		Vector3 positionA = new Vector3 (leftPoint.transform.position.x, 
			                    leftPoint.transform.position.y, leftPoint.transform.position.z);


		//Vector3 positionB = new Vector3 (1, 0, 0);
		Vector3 positionB = new Vector3 (rightPoint.transform.position.x, 
			rightPoint.transform.position.y, rightPoint.transform.position.z);

		//this funcion moves between positions A and B through Timerlerp; which goes between 0-1
		transform.position = Vector3.Lerp (positionA, positionB, timerlerp)	;

		//if at position A, move towards position B (From 0 to 1)
		if (timerlerp <= 0)
		{
			timeralterer = Time.deltaTime* smooth;
			WalkingAnimator.SetBool("LeftFacing",false);
			transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
		}
		//if at position B move towards position A (From 1 to 0)
		if (timerlerp >=1 )
		{
			timeralterer = -1*Time.deltaTime* smooth;
			WalkingAnimator.SetBool("LeftFacing",true);
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		}

		//change the value along the line
		timerlerp += timeralterer* smooth;
	}
}
