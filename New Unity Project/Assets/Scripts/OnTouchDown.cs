using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	This class converts touch events to mousedown events for both computer and touch device usage
 * */
public class OnTouchDown : MonoBehaviour {
	
	RaycastHit hit;				//hit data for touch raycast

	// Update is called once per frame
	void Update () {

		hit = new RaycastHit ();
		for (int i = 0; i < Input.touchCount; ++i) {
			//for each touch event test if its just began
			if (Input.GetTouch (i).phase.Equals (TouchPhase.Began)) {
				//cast ray from screen point into level
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (i).position);
				if (Physics.Raycast(ray, out hit)){
					//if it hits an object execute that object's OnMouseDown function
					hit.transform.gameObject.SendMessage("OnMouseDown",SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}