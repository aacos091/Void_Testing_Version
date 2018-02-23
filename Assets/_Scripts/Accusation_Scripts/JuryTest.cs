using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuryTest : MonoBehaviour {

	private Light spotLight;
	private Vector3 startingPos;

	void Start ()
	{
		// SpotLight settings for "Interrogation"
		spotLight = GameObject.Find ("JuryLight").GetComponent<Light> ();
		startingPos = new Vector3 (-6f, 3.45f, -12f);
	}

	/*public void Judgement() {
		Renderer rend = GetComponent<Renderer> ();

		float roll = Random.Range (0.0f, 10.0f);

		print ("They rolled a " + roll);

		if (roll <= 6) {
			rend.material.color = Color.green;
		} else {
			rend.material.color = Color.red;
		}
	}*/

	/*void OnMouseDown() {
		Judgement ();
	}*/
/*
	void OnMouseOver ()
	{
		//print ("Over " + this.gameObject.name);
		Vector3 targetLightPosition = startingPos;
		targetLightPosition.x = this.transform.position.x;
		spotLight.transform.position = targetLightPosition;
	}*/
}
