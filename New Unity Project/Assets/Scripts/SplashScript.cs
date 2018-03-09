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

	public void startGame( string SceneToOpen)
	{
		AkSoundEngine.PostEvent ("Click_Start", gameObject);
		StartCoroutine (NextScene(SceneToOpen));
	}

	public void startFade( string xAxis)
	{
		AkSoundEngine.SetRTPCValue(xAxis, 0f, GameObject.FindGameObjectWithTag("MainCamera"), 500);

	}

	public void quitGame()
	{
		AkSoundEngine.PostEvent ("Click_Start", gameObject);
		Application.Quit ();
	}

	public void restartGame()
	{
		AkSoundEngine.PostEvent ("Click_Other", gameObject);
		AkSoundEngine.StopAll ();
		Application.LoadLevel(Application.loadedLevel);
	}

	IEnumerator NextScene(string SceneToOpen)
	{
		yield return new WaitForSeconds (0.2f);

		SceneManager.LoadSceneAsync (SceneToOpen);
		AkSoundEngine.StopAll ();
	}

}
