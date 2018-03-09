using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endStateButtons : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void restartLevelButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void returnToSplash()
	{
		SceneManager.LoadScene ("Splash");
	}

	public void NextLevelButton()
	{
		SceneManager.LoadScene ("Scene1");
	}
}
