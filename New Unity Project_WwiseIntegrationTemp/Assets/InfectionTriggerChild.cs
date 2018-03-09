using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionTriggerChild : MonoBehaviour {

	public AudioClip soundHumanDeath;
	private AudioSource source;


	public void Explode(Collider2D CustomShape)
	{
		source = GetComponent<AudioSource> ();
		source.PlayOneShot (soundHumanDeath);
		//explode
		//Potential for differing colliders
		ContactFilter2D CustomFilter = new ContactFilter2D();
		Collider2D[] AllCollisions= new Collider2D[20];
		Physics2D.OverlapCollider (CustomShape, CustomFilter, AllCollisions);
		 //= Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y),Radius);

		for (int i = 0; i < AllCollisions.Length; ++i) {
			if (AllCollisions [i] != null) {
				if (AllCollisions [i].gameObject != gameObject &&
				   (AllCollisions [i].tag == "Human" || AllCollisions [i].tag == "Technology")) {
					AllCollisions [i].GetComponent<Infected> ().SendMessage ("SetInfected", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
