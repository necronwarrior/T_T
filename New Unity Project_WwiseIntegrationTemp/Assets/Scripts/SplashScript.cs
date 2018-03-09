using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void startGame(string SceneToOpen)
	{
		SceneManager.LoadScene (SceneToOpen);
		Debug.Log ("imclick");
	}

	public void quitGame()
	{
		Application.Quit ();
	}

	public void restartGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
