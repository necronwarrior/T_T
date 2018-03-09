using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isUnlocked : MonoBehaviour {

	public string[] requirements;
	public enum Type{AND, OR};
	public Type UnlockType;
	bool Unlocked;
	// Use this for initialization
	void Start () {
		Unlocked = true;
		for(int i=0;i<requirements.Length;++i)
		{
			
			if(PlayerPrefs.GetInt(requirements[i])<1){
				Unlocked = false;
			}

			if (UnlockType == Type.OR) {
				if(PlayerPrefs.GetInt(requirements[i])>0){
					Unlocked = true;
				}
			}
		}
		if (Unlocked == false) {
			GetComponent<Button> ().onClick = new Button.ButtonClickedEvent();
			ColorBlock colorvar = GetComponent<Button> ().colors;
			colorvar.normalColor = Color.gray;
			colorvar.highlightedColor = Color.red;
			GetComponent<Button> ().colors = colorvar;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
