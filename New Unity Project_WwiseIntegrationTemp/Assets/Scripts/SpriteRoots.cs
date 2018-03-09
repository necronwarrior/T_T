using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRoots : MonoBehaviour {

	public struct IntVector2
	{
		public int x;
		public int y;
		public IntVector2(int newX=0, int newY=0)
		{
			x = newX;
			y = newY;
		}

		public static IntVector2 operator +(IntVector2 c1, IntVector2 c2)
		{
			return new IntVector2 (c1.x + c2.x, c1.y + c2.y);
		}

		public static IntVector2 operator -(IntVector2 c1, IntVector2 c2)
		{
			return new IntVector2 (c1.x - c2.x, c1.y - c2.y);
		}
	}

	public Vector2 branchLength;
	Texture2D MyTex;
	Sprite rep;
	List<IntVector2> currCoords;
	// Use this for initialization
	void Start () {
		MyTex = Instantiate (GetComponent<SpriteRenderer> ().sprite.texture);

		GetComponent<SpriteRenderer> ().sprite = Sprite.Create (MyTex,
			new Rect(0,0,MyTex.width, MyTex.height),
			new Vector2 (0.0f, 0.0f),12f);
		currCoords = new List<IntVector2>();
		currCoords.Add(new IntVector2(0,0));
		//StartCoroutine (StartRoot ());
	}

	public void Begin()
	{
		StartCoroutine (StartRoot ());
	}
		
	IEnumerator StartRoot () {
		while (true) {
			
			for (int i = 0; i < currCoords.Count; i++) {
				currCoords [i] = Branch (currCoords [i]);
			}

			if (Random.Range (0, 11) > 9) {
				currCoords.Add (currCoords [Random.Range (0, currCoords.Count)]);
			}


			MyTex.Apply ();
			GetComponent<SpriteRenderer> ().sprite = Sprite.Create (MyTex,
				new Rect(0,0,MyTex.width, MyTex.height),
				new Vector2 (0.0f, 0.0f),12f);
			yield return new WaitForSeconds (0.01f);
		}
	}

	IntVector2 Branch(IntVector2 Curr){
		
		IntVector2 TempDir;
		TempDir = directionAlgo ();
		if (MyTex.GetPixel (Curr.x, Curr.y) == Color.black) {
			MyTex.SetPixel (Curr.x, Curr.y, Color.green);
		}

		for (int i = 0; i < Random.Range(branchLength.x, branchLength.y); ++i) {
			Curr += TempDir;
			if (MyTex.GetPixel (Curr.x, Curr.y) == Color.black) {
				MyTex.SetPixel (Curr.x, Curr.y, Color.green);
			} else {
				Curr -= (TempDir+TempDir);
				if (MyTex.GetPixel (Curr.x, Curr.y) == Color.black) {
					MyTex.SetPixel (Curr.x, Curr.y, Color.green);
				}
			}
		}
		return Curr;
	}

	IntVector2 directionAlgo()
	{
		IntVector2 Dir = new IntVector2();
		Dir.x = Random.Range (Dir.x-1, Dir.x + 2);
		Dir.y = Random.Range (Dir.y-1, Dir.y + 2);

		return Dir;
	}
}
