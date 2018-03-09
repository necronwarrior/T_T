using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedLevelChecker : MonoBehaviour {

	public string currentWorld;
	public GameObject[] levelList; //container for all levels in the scene

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake()
	{


		//if (levelList == null)
			levelList = GameObject.FindGameObjectsWithTag("level"); //populate list with al tagged items
		
		foreach (GameObject level in levelList){
			level.gameObject.GetComponent<Button>().enabled = (PlayerPrefs.GetInt (currentWorld+level.name)>0); //load and activate each level that has been completed
		}
	}
}
