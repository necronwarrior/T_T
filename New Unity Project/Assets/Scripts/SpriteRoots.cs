using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRoots : MonoBehaviour {

	//Custom vector2 framework because pixel texture coordinates need to be absolute (there is no '0.2' of a pixel)
	public struct IntVector2
	{
		public int x;
		public int y;

		//Default constructor, for that nice RAII completeness
		public IntVector2(int newX=0, int newY=0)
		{
			x = newX;
			y = newY;
		}

		//Basic overrides for the little math that gets done

		public static IntVector2 operator +(IntVector2 c1, IntVector2 c2)
		{
			return new IntVector2 (c1.x + c2.x, c1.y + c2.y);
		}

		public static IntVector2 operator -(IntVector2 c1, IntVector2 c2)
		{
			return new IntVector2 (c1.x - c2.x, c1.y - c2.y);
		}

		public void SetX(int newX)
		{
			x = newX;
		}

		public void SetY(int newY)
		{
			y = newY;
		}


	}

	public float InputDegrees = 0; 	//Default until collision supplies this
	public Vector2 branchLength;	//Default options for branch distances
	public Color PlayerColor = Color.green;
	Texture2D MyTex;				//Container for the altered, updating texture
	List<IntVector2> currCoords;	//list of current branching points
	int PixelThreshold;

	void Start () {
		//Initialise: texture, list of branch spawning points, and colouring limit before finishing
		MyTex = Instantiate (GetComponent<SpriteRenderer> ().sprite.texture); 	
		currCoords = new List<IntVector2>();	

		PixelThreshold = (int)((MyTex.width * MyTex.height)*0.4f);
	}

	/*
	 * 		Diagram of a basic sprite and its relation to the quadrants system for infecton branching initialistaion points
	 * 
	 * 										Quadrant 1								
	 * 					90 Deg										180 Deg																									
	 * 					(0,1)										(1,1)																								
	 * 						\										/																								
	 * 							\								/																								
	 * 								\						/																										
	 * 									\				/																											
	 * 										\		/																												
	 * 		Quadrant 0							X							Quadrant 2																						
	 * 										/		\																												
	 * 									/				\																											
	 * 								/						\																										
	 * 							/								\																									
	 * 						/										\																								
	 * 					(0,0)										(1,0)																									
	 * 					0 Deg										270 Deg	
	 *										Quadrant 3	
	 * */

	public void Begin(Vector2 InfectorPos)
	{
		Vector2 InfecteePos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		InputDegrees = ((Mathf.Atan2 (InfectorPos.y - InfecteePos.y, InfectorPos.x - InfecteePos.x))*180/Mathf.PI);

		InputDegrees = (225 - InputDegrees);

		IntVector2 StartPoint;
		int Quadrant =0;

		//determine what quadrant the infection is coming from
		while (InputDegrees > 90) {
			InputDegrees -= 90;
			Quadrant++;
		}
			

		//calculate direction of infection
		switch (Quadrant) {
		case 0: StartPoint=new IntVector2(0,(int)((InputDegrees / 90) * MyTex.height)); break;
		case 1: StartPoint=new IntVector2((int)((InputDegrees / 90) * MyTex.width),MyTex.height);break;
		case 2: StartPoint=new IntVector2(MyTex.width,(int)((InputDegrees / 90) * MyTex.height));break;
		case 3: StartPoint=new IntVector2((int)((InputDegrees / 90) * MyTex.width),0);break;
		default: 
			StartPoint = new IntVector2 ((int)(MyTex.width * 0.5), (int)(MyTex.height * 0.5));break;
		}

		int CheckLevel = 1;
		bool Escape = true;
		while (Escape) {
			for (int X = -1 * CheckLevel; X <= 1 * CheckLevel; ++X) {
				for (int Y = -1 * CheckLevel; Y <= 1 * CheckLevel; ++Y) {
					if (StartPoint.x + X >= 0 &&
						StartPoint.x + X < MyTex.width &&
						StartPoint.y + Y >= 0 &&
						StartPoint.y + Y < MyTex.height) {
						if (MyTex.GetPixel (StartPoint.x + X, StartPoint.y + Y).Equals (Color.black)) {
							StartPoint = new IntVector2 (StartPoint.x + X, StartPoint.y + Y);
							Escape = false;
						}
					}
				}
			}
			CheckLevel++;
		}
		currCoords.Add (StartPoint);
		StartCoroutine (StartRoot ());
	}
		



	IEnumerator StartRoot () {
		while (true) {
			if (PixelThreshold > 0) {
				for (int i = 0; i < currCoords.Count; i++) {
					currCoords [i] = Branch (currCoords [i]);
				}

				/*if (Random.Range (0, 11) > 9) {
					currCoords.Add (currCoords [Random.Range (0, currCoords.Count)]);
				}*/

				MyTex.Apply ();
			
				GetComponent<SpriteRenderer> ().sprite = Sprite.Create (MyTex,
					new Rect (0, 0, MyTex.width, MyTex.height),
					new Vector2 (0.0f, 0.0f), 12f);
				yield return new WaitForSeconds (0.01f);
			} else {
				break;
			}
		}
	}

	//directions and rules for branching
	IntVector2 Branch(IntVector2 Curr){

		List<IntVector2> TempVals = new List<IntVector2> ();
		TempVals = PixelListTest (Curr, Color.black);
		if (TempVals.Count == 0) {
			TempVals = PixelListTest (Curr, PlayerColor);
		}

		IntVector2 NextPix;
		if (TempVals.Count != 0) {
			NextPix = TempVals [Random.Range (0, TempVals.Count)];
		
			MyTex.SetPixel (NextPix.x, NextPix.y, PlayerColor);
			PixelThreshold--;
			Curr = NextPix;
		}
		return Curr;
	}

	List<IntVector2> PixelListTest(IntVector2 Curr, Color TestingVal)
	{
		List<IntVector2> SucceededVals = new List<IntVector2> ();

		for (int X = -1; X <= 1; ++X) {
			for (int Y = -1; Y <= 1; ++Y) {
				if(Curr.x+X>=0 &&
					Curr.x+X<MyTex.width &&
					Curr.y+Y>=0 &&
					Curr.y+Y<MyTex.height)
				{
					if (MyTex.GetPixel (Curr.x + X, Curr.y + Y).Equals (TestingVal)) {
						SucceededVals.Add (Curr + (new IntVector2 (X, Y)));
					}
				}
			}
		}
		return SucceededVals;
	}

}
