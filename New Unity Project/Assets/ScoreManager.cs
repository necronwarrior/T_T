using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScoreManager : MonoBehaviour {

	public Image ScoreHolder;
	//Text ScoreNumber;

	public int[] ScoreArrayBestToWorst = new int[5];

	GameObject TextPrefab;

	public bool firstTouch = false;

	public float endTimer;

	public Text ScoreNumber;

	//determination of whether the level has been completed
	public void SetLevelUnlock(int value){
		PlayerPrefs.SetInt (SceneManager.GetActiveScene().name, value);
	}

	// Use this for initialization
	void Start () {
		TextPrefab = (GameObject)Resources.Load ("Prefabs/TextPrefab");
		//ScoreNumber = ScoreHolder.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (endTimer);
		if (int.Parse (ScoreNumber.text) != 0 && endTimer <= 2.6)
		{
			endTimer += Time.deltaTime;
		}
	}

	public void AddScore(int ScoreAdditive, Vector3 position)
	{
		StartCoroutine (TextLerp (ScoreAdditive,position));
		endTimer = 0.0f;
	}

	IEnumerator TextLerp(int ScoreAdditive, Vector3 position)
	{
		//if we want more complex animations, put it here
		GameObject NewText = (GameObject)Instantiate((Object)TextPrefab);
		NewText.transform.SetParent(gameObject.transform);
		NewText.transform.position = Camera.main.WorldToScreenPoint (position);
		Vector3 StartPos = NewText.transform.position;

		NewText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		NewText.GetComponent<Text> ().resizeTextForBestFit = true;

		NewText.GetComponent<Text> ().text = ScoreAdditive.ToString ();
		
		for (float lerptime = 0.0f; lerptime < 1;) {
			NewText.transform.position = Vector3.Lerp (StartPos, ScoreNumber.transform.position, lerptime);
			yield return new WaitForSeconds (0.01f);
			lerptime += 0.01f;
		}
		Destroy (NewText);
		ScoreNumber.text = (int.Parse(ScoreNumber.text) + ScoreAdditive).ToString();
	}

	public void EndLevel()
	{

		EventSystem.current.GetComponent<SCR_MoveCamera> ().GetNextLevel (
			EventSystem.current.GetComponent<SCR_MoveCamera> ().LevelCounter);

			firstTouch = false;
			endTimer = 0.0f;
			ScoreNumber.text = "0";
	}

}
