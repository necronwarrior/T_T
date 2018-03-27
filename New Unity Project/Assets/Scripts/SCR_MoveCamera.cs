using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * created: 27/03/18
 * Created by: Rory McLean
 * 
 * Moving the camera via lerping when the level is finished
 * used for narrative and cuntscenes
 * 
 * */

public class SCR_MoveCamera : MonoBehaviour 
{

	//we define this end position in the scene maybe?
	public Vector3 EndPositionCamera;

	float timeTakenDuringLerp = 1.0f;
	bool _isLerping;
	Vector3 _startPosition;
	Vector3 _endPosition;
	float _timeStartedLerping;

	void StartLerping()
	{
		_isLerping = true;
		_timeStartedLerping = Time.time;

		//set the start position to be the cameras current position
		//and end position to be where we define
		_startPosition = Camera.main.transform.position;
		_endPosition = new Vector3 (EndPositionCamera.x,
			EndPositionCamera.y,
			EndPositionCamera.z);
	}


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			Debug.Log ("b0ss");
			StartLerping ();
		}
	}


	void FixedUpdate()
	{
		if (_isLerping)
		{
			float timeSinceStarted = Time.time - _timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;


			Camera.main.transform.position = Vector3.Lerp (_startPosition, _endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				_isLerping = false;
			}

		}
	}
}
