using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		float Wid = this.GetComponent<RectTransform> ().rect.width;

		this.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Wid, 500);
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
