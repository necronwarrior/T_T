﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/*
	 * created 28/03/18
	 * 
	 * created by: Rory McLean
	 * 
	 * Purpose:
	 * tracking the progress in the each level by caluclating the
	 * percentage of each object in the world thats been infected
	 * 
	 * 
	 * */

public class SCR_ProgressBar : MonoBehaviour 
{
	public float levelCompletionPercentage = 50.0f;

	public Image progressUiBar;
	public GameObject progressIndicator;

	public List<GameObject> objectList;

	public bool levelComplete;

	// Use this for initialization
	void Start () 
	{
		progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);
		setupUiComponents ();
		//populateObjectList ();
		AddDescendantsWithTag(transform, "Technology", objectList);
		AddDescendantsWithTag (transform, "Human", objectList);
	}
	
	// Update is called once per frame
	void Update () 
	{
		calculateObjectPercentages ();
	}

	void setupUiComponents()
	{
		progressIndicator.transform.Translate (new Vector3 ((69.0f / 100.0f) * levelCompletionPercentage, 0.0f, 0.0f));
	}

	void populateObjectList()
	{
		//get a temp array of all the tech in the level
		GameObject[] tempList = GameObject.FindGameObjectsWithTag ("Technology");


		GameObject[] tempList2 = GameObject.FindGameObjectsWithTag ("Human");

		for (int i = 0; i < tempList.Length; i++)
		{
			objectList.Add (tempList [i]);

		}
	

		for (int i = 0; i < tempList2.Length; i++)
		{
			objectList.Add (tempList2 [i]);
		}
	}


	void calculateObjectPercentages()
	{
		int infectedObject = 0;
		progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);

		for (int i = 0; i < objectList.Count; i++)
		{
			if (objectList [i] != null) {
				if (objectList [i].GetComponent<Infected> ().objectInfected) { // when score is added 
					infectedObject += 1;
					//Debug.Log ("stuff:" + infectedObject);
				}
			}
		}

		if (levelComplete == false)
		{
			//calculate the percentage of objects infected
			float infectedPercentage = ((float)infectedObject / objectList.Count) *100.0f;

			if(GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ().endTimer>2.5f)
			{
				//update the UI text colour to illustrare if a goal has been met
				if (infectedPercentage < levelCompletionPercentage)
				{
					progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);
					levelComplete = false;
					progressUiBar.fillAmount = 0.0f;
					GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ().firstTouch = false;
					Destroy (transform.parent.gameObject);
					Instantiate(Resources.Load("Prefabs/LevelPrefabs/" +transform.parent.gameObject.name));

				} else
				{
					
						if (infectedPercentage < 100.0f)
						{
							Debug.Log ("level finished");
							Debug.Log ("current bar percentage: " + infectedPercentage);
							progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);
							levelComplete = true;

							GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ().EndLevel ();
						} else
						{
							progressUiBar.color = new Color (0.368f, 0.717f, 0.858f, 1.0f);	

							levelComplete = true;

							GameObject.FindGameObjectWithTag ("ScoreManagerTag").GetComponent<ScoreManager> ().EndLevel ();

						}
					}
			}

			//update the progress ui bar
			progressUiBar.fillAmount = (infectedPercentage / 100.0f);
		}




	}

	void checkEndCondition()
	{
		if (levelComplete)
		{
			//next level or whatever
			progressUiBar.fillAmount = 0.0f;
		}
	}

	public float getInfectedPercentage()
	{
		int infectedObject = 0;

		for (int i = 0; i < objectList.Count; i++)
		{
			if(objectList[i].GetComponent<Infected>().objectInfected) // when score is added 
			{
				infectedObject += 1;
			}

		}

		//calculate the percentage of objects infected
		float infectedPercentage = ((float)infectedObject / objectList.Count) *100.0f;
	
		return Mathf.Round (infectedPercentage);
	
	}




	private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
	{

		//get a temp array of all the tech in the level
		//GameObject[] tempList = GameObject.FindGameObjectsWithTag ("Technology");


		//GameObject[] tempList2 = GameObject.FindGameObjectsWithTag ("Human");


		foreach (Transform child in parent)
		{
			if (child.gameObject.tag == tag)
			{

				list.Add (child.gameObject);
//				//loop through the array and add eac object to the list
//				for (int i = 0; i < List.Length; i++)
//				{
//					objectList.Add (tempList [i]);
//				}
//
//				for (int i = 0; i < tempList2.Length; i++)
//				{
//					objectList.Add (tempList2 [i]);
//				}


			}
			AddDescendantsWithTag (child, tag, list);
		}

	}

}
