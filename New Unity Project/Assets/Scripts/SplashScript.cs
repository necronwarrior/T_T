using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*	This class is used for scene transitions 
 * */
public class SplashScript : MonoBehaviour 
{
	//This function Loads scenes that are passed into it
	public void startGame( string SceneToOpen)
	{
		AkSoundEngine.PostEvent ("Click_Start", gameObject);
		StartCoroutine (NextScene(SceneToOpen));
	}

	//this transisions the scenes passed into startGame
	IEnumerator NextScene(string SceneToOpen)
	{
		//wait until sound effects are over
		yield return new WaitForSeconds (0.2f);

		//load the scene
		SceneManager.LoadSceneAsync (SceneToOpen);
		AkSoundEngine.StopAll ();
	}

	//fade to black
	public void startFade( string xAxis)
	{
		AkSoundEngine.SetRTPCValue(xAxis, 0f, GameObject.FindGameObjectWithTag("MainCamera"), 500);
	}

	//exit the game
	public void quitGame()
	{
		AkSoundEngine.PostEvent ("Click_Start", gameObject);
		Application.Quit ();
	}

	//restart the current level
	public void restartGame()
	{
        //AkSoundEngine.PostEvent ("Click_Other", gameObject);
        AkSoundEngine.StopAll ();
        AkSoundEngine.PostEvent("Reset_Game", gameObject);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}