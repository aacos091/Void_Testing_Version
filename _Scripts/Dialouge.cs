using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialouge : MonoBehaviour 
{

	public static bool cook;

	public static bool medic;

	public static bool captain;

	public static bool engineer;

	// Use this for initialization
	void Start () {
		
	}

	void Update () 
	{


	}

	public void OnClick()
	{

		if (CameraController.cook == true) 
		{

			Debug.Log ("caaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

		}

		if (CameraController.medic == true) 
		{

			Debug.Log ("reeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

		}

		//Units.dialogue = false;

	}

}
