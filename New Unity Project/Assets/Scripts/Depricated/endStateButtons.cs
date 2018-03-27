using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*	This class is a deprecated button class for the game jam 
 * */
public class endStateButtons : MonoBehaviour 
{
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