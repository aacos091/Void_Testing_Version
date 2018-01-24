using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public int floor;

	void Start(){
		// Use this for initialization
	}

	void Update(){
		if (gameObject.transform.position.y < -2)
			floor = 1;
		if (gameObject.transform.position.y > -2 && gameObject.transform.position.y < 1)
			floor = 2;
		if (gameObject.transform.position.y > 1 && gameObject.transform.position.y < 25)
			floor = 3;
	}
}
