using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public int floor;

	void Start(){
		// Use this for initialization
	}

	void Update(){
		//If waypoints never move during the game, this scripts should probably go in Awake or Start
		if (gameObject.transform.localPosition.y < -4.5)
			floor = 1;
		if (gameObject.transform.localPosition.y >= -4.5 && gameObject.transform.localPosition.y < -3)
			floor = 2;
		if (gameObject.transform.localPosition.y >= -3 && gameObject.transform.localPosition.y <= 0)
			floor = 3;
		if (gameObject.transform.localPosition.y > 0)
			floor = 4;
		
	}
}
