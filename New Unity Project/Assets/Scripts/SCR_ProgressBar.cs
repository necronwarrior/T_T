using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

	bool levelComplete;



	// Use this for initialization
	void Start () 
	{
		setupUiComponents ();
		populateObjectList ();
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
	
		//loop through the array and add eac object to the list
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

		for (int i = 0; i < objectList.Count; i++)
		{
			if(objectList[i].GetComponent<Infected>().objectInfected) // when score is added 
			{
				infectedObject += 1;
				Debug.Log ("stuff:" + infectedObject);
			}

		}

		//calculate the percentage of objects infected
		float infectedPercentage = ((float)infectedObject / objectList.Count) *100.0f;

		//update the UI text colour to illustrare if a goal has been met
		if (infectedPercentage < levelCompletionPercentage)
		{
			progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);
			levelComplete = false;
		} else if (infectedPercentage < 100.0f)
		{
			progressUiBar.color = new Color (0.486f, 0.819f, 0.290f, 1.0f);
			levelComplete = true;
		} 
		else
		{
			progressUiBar.color = new Color (0.368f, 0.717f, 0.858f, 1.0f);	
		}

		//update the progress ui bar
		progressUiBar.fillAmount = (infectedPercentage / 100.0f);


	}

	void checkEndCondition()
	{
		if (levelComplete)
		{
			//next level or whatever
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
}
