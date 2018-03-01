using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_Collider : MonoBehaviour 
{
	
	Quaternion rotate;

	void Start()
	{

		rotate = transform.rotation;
	}

	void Update () 
	{

		transform.rotation = rotate;
		
	}

}
