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
	//public Vector3 EndPositionCamera;

	float timeTakenDuringLerp = 1.0f;
	bool _isLerping;
	Vector3 _startPosition;
	Vector3 _endPosition;
	float _timeStartedLerping;


	//public GameObject scrollPanel;
	public GameObject[] LevelList;
	public int LevelCounter;
	public GameObject level1;
	public GameObject level2;


	Vector3 _level1Pos;
	Vector3 _level2Pos;
	Vector3 _level3Pos;



	void StartLerping()
	{
		_isLerping = true;
		_timeStartedLerping = Time.time;
        AkSoundEngine.PostEvent("Play_Transition", gameObject);

		//set the start position to be the cameras current position
		//and end position to be where we define
		_startPosition = Camera.main.transform.position;


	}


	// Use this for initialization
	void Start () 
	{
		LevelCounter = 0;
		_level1Pos = new Vector3 (level1.transform.position.x, level1.transform.position.y, Camera.main.transform.position.z);
		_level2Pos = new Vector3 (level2.transform.position.x, level2.transform.position.y, Camera.main.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("levelcounter: " + LevelCounter);
		//GetNextLevel ();

		if (Input.GetKeyDown (KeyCode.Space))
		{
			//_endPosition = _level1Pos;
			GetNextLevel(1);

			//Debug.Log ("b0ss");
			StartLerping ();
		}

		if (Input.GetKeyDown (KeyCode.KeypadEnter))
		{
			_endPosition = _level2Pos;
			StartLerping ();
		}
	}


	public void GetNextLevel(int levelCount)
	{
		//level count would equal the next level to go to
		//i.e if i beat level one and i want to go to level 2 then

		Debug.Log ("the level should be: " + levelCount);
        AkSoundEngine.SetRTPCValue("Level", levelCount, null, 0);

		//levelCount = next level to go to
		LevelCounter++;
		LevelList [LevelCounter].SetActive (true);
		LevelList [LevelCounter-1].SetActive (false);
		_endPosition = LevelList [LevelCounter].transform.position + (Vector3.back*10);
		StartLerping ();

//		switch (levelCount)
//		{
//		case 0:
//			_endPosition = _level1Pos;
//			StartLerping ();
//			break;
//
//		case 1:
//			_endPosition = _level2Pos;
//			StartLerping ();
//			break;
//
//		case 2:
//			break;
//
//		default:
//			Debug.Log ("Broke inside levelCount switch");
//			break;
//
//		}

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
