using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ObjectMove : MonoBehaviour {

	public GameObject[] WaypointList;
	public float[] TimeToLerp;
	int LastPoint, NextPoint;
	float CountingTimeHolder;

	void Start()
	{
		LastPoint = 0;
		NextPoint = 1;
		CountingTimeHolder = 0.0f;
	}

	// Update is called once per frame
	void Update () {

		transform.position = Vector3.Lerp (WaypointList [LastPoint].transform.position, WaypointList [NextPoint].transform.position, CountingTimeHolder / TimeToLerp[NextPoint]);
		transform.position -= (Vector3.up + Vector3.right);

		CountingTimeHolder += Time.deltaTime;

		if (transform.position == new Vector3(WaypointList [NextPoint].transform.position.x -1.0f, WaypointList [NextPoint].transform.position.y -1.0f,  WaypointList [NextPoint].transform.position.z )) {
			
			LastPoint = NextPoint;

			if (NextPoint == (WaypointList.Length-1)) {
				NextPoint = 0;
			} else {
				NextPoint++;
			}

			CountingTimeHolder = 0.0f;
		}
	}
}