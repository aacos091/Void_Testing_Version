using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotater : MonoBehaviour {

	//public Text 			xDeltaText;
	//public Text				mouseStartPosText;
	public float 			rotateSpeed;
	[Tooltip("Threshold to begin rotating object")]
	public float 			diffThreshold = 5;

	// Keeps track of if the left mouse button is held down.
	private bool 			bMouseHeldDown = false;
	private Vector3 		startMousePos;
	private Collider		coll;

	// Use this for initialization
	void Start () {
		coll = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	 	OnMouseClick();
				
		if (bMouseHeldDown)
		{
			print ("held down");
			//Vector3 delta = Camera.main.WorldToViewportPoint (Input.mousePosition) - startMousePos;
			Vector3 delta = Input.mousePosition - startMousePos;
			delta.z = 0;
			float xDelta = delta.x;
			float yDelta = delta.y;
			//xDeltaText.text = "xDelta = " + xDelta;
			//yDeltaText.text = "yDelta = " + yDelta;

			// Handle Rotation 
			// TODO Make this standalone function and account for rotation based on if value is negative or positive (left or right)
			if (xDelta > diffThreshold)
			{
				transform.RotateAround (coll.bounds.center, -Vector3.up, rotateSpeed * Time.deltaTime);
			} 
			if (yDelta > diffThreshold)
			{
				transform.RotateAround (coll.bounds.center, Vector3.right, rotateSpeed * Time.deltaTime);
			}
			if (xDelta < -diffThreshold)
			{
				transform.RotateAround (coll.bounds.center, Vector3.up, rotateSpeed * Time.deltaTime);
			}
			if (yDelta < -diffThreshold)
			{
				transform.RotateAround (coll.bounds.center, -Vector3.right, rotateSpeed * Time.deltaTime);
			}
		}
		#endif
	}

	void OnMouseClick ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			bMouseHeldDown = true;
			// We initialize startMousePos here because this is the position of the mouse when we first left click
			//startMousePos = Camera.main.WorldToViewportPoint (Input.mousePosition);
			startMousePos = Input.mousePosition;
			startMousePos.z = 0;
			//mouseStartPosText.text = "Start Mouse Pos = " + startMousePos.ToString ();
		} 
		else if (Input.GetMouseButtonUp (0))
		{
			startMousePos = Vector3.zero;
			bMouseHeldDown = false;
		}
	}
/*
 * Pseudo code for mouse drag rotation
 *  1) On mouse down grab current x and y position
 *  2) Calculate magnitude of change in position
 *  3) Find the magnitude of the change in x
 *  4) Find the magnitude of the change in y
 * 
 */
}


