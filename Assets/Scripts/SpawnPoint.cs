
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		// Disable MeshRenderer
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
	}

	void Start () {
		// Destroy this object
		DestroyImmediate (gameObject);
	}
}
