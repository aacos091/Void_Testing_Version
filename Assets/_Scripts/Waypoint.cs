using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public int floor;
	public bool isCollidingWithUnit;

	void Start(){
		// Use this for initialization
		isCollidingWithUnit = false;
	}

	void Update(){
		if (gameObject.transform.position.y <= -4.5)
			floor = 1;
		if (gameObject.transform.position.y >= -4.5 && gameObject.transform.position.y < -3)
			floor = 2;
		if (gameObject.transform.position.y > -3 && gameObject.transform.position.y < -0.635)
			floor = 3;
		if (gameObject.transform.position.y > -0.635)
			floor = 4;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Unit") {
			isCollidingWithUnit = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Unit") {
			isCollidingWithUnit = false;
		}
	}
}
