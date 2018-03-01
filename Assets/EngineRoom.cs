using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoom : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		
		Debug.Log("In Engine room");
		
	}

	private void OnTriggerExit(Collider other)
	{
		
		Debug.Log("Leaving engine room");
		
	}
}
