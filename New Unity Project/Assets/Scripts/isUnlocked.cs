using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* This class determines whether levels are unlocked based on typed in requirements
 * */
public class isUnlocked : MonoBehaviour {

	public string[] requirements;	//Container for levels that need completed
	public enum Type{AND, OR};		//Identifer as to logic required to unlock next level
	public Type UnlockType;			//current identifier
	bool Unlocked = true;			//Overall unlocked state, begins as true

	// On loading the level select, this is ran
	void Start () {

		//check every requirement
		for(int i=0;i<requirements.Length;++i)
		{
			//if a specified levl is not completed, the level isnt unlocked 
			if(PlayerPrefs.GetInt(requirements[i])<1){
				Unlocked = false;
			}

			//if one of the requirements is met for the OR functinality, the level is unlocked
			if (UnlockType == Type.OR) {
				if(PlayerPrefs.GetInt(requirements[i])>0){
					Unlocked = true;
					break;
				}
			}
		}

		// Change the onclicked event if the level is locked to point nowhere
		if (Unlocked == false) {
			GetComponent<Button> ().onClick = new Button.ButtonClickedEvent();
			ColorBlock colorvar = GetComponent<Button> ().colors;
			colorvar.normalColor = Color.gray;
			colorvar.highlightedColor = Color.red;
			GetComponent<Button> ().colors = colorvar;
		}
	}
}