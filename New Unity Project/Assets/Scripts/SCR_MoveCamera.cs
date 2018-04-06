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

	float timeTakenDuringLerp = 12.0f;
	bool _isLerping;
	Vector3 _endPosition;


	//public GameObject scrollPanel;
	public GameObject[] LevelList;
	public int LevelCounter;
	public GameObject level1;
	public GameObject level2;

	void StartLerping()
	{
		_isLerping = true;
	}

	// Use this for initialization
	void Start () 
	{
		LevelCounter = 0;
	}

	public void GetNextLevel(int levelCount)
	{
		//level count would equal the next level to go to
		//i.e if i beat level one and i want to go to level 2 then

        AkSoundEngine.SetRTPCValue("Level", levelCount, null, 0);
		//levelCount = next level to go to
		LevelCounter++;

		LevelList [LevelCounter].SetActive (true);
		_endPosition = LevelList [LevelCounter].transform.position + (Vector3.back*10);
		StartLerping ();
	}

	void FixedUpdate()
	{
		if (_isLerping)
		{
			//_timeStartedLerping += (Time.deltaTime*0.5f);

			Camera.main.transform.position = Vector3.MoveTowards (Camera.main.transform.position, _endPosition, Time.deltaTime*timeTakenDuringLerp);

			if (Camera.main.transform.position == _endPosition) {

				LevelList [LevelCounter-1].SetActive (false);
				_isLerping = false;
			}
		}
	}
}