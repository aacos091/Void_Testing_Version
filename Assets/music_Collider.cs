using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class music_Collider : MonoBehaviour 
{
	
	Quaternion rotate;

	private bool cam;

	private void OnTriggerEnter(Collider other)
	{

		if (other.tag == "MainCamera")
		{

			cam = true;

		}

	}

	private void OnTriggerExit(Collider other)
	{

		if (other.tag == "MainCamera")
		{

			cam = false;

		}
		
	}

	void Start()
	{
		
		cam = false;
		
		rotate = transform.rotation;
		
	}

	void Update () 
	{

		transform.rotation = rotate;

		if (GetComponentInParent<NavMeshAgent>().speed > 0 && cam)
		{
			
			GetComponent<StudioEventEmitter>().SendMessage("Play");
			
		}
		
		if (GetComponentInParent<NavMeshAgent>().speed < 0 || !cam)
		{
			
			GetComponent<StudioEventEmitter>().SendMessage("Stop");
			
		}
		
	}

	public void Cam()
	{

		cam = false;
		
	}
	
}
