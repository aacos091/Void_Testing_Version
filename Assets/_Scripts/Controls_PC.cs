using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls_PC: MonoBehaviour 
{

	public float dragSpeed = 2;
	public float maxDragSpeed = 3;
	public float minDragSpeed = .5f;

	private Vector3 dragOrigin;

	private float moveSmooth = 0.1f; 

	void Update () 
	{
		#if UNITY_STANDALONE_WIN ||UNITY_EDITOR_WIN
		if (Input.GetMouseButtonDown (0)) 
		{
			dragOrigin = Input.mousePosition;
			return;
		}

		PCCameraControls();
		#endif
	}


	/*********************
	 *  Custom Functions *
	 * ******************/

	void PCCameraControls()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 60f);

			Camera.main.fieldOfView--;

			if (Camera.main.fieldOfView >= 19) {
				if (dragSpeed > minDragSpeed) {
					dragSpeed -= 0.1f;
				}
			}

		} 
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 60f);

			Camera.main.fieldOfView++;

			if (Camera.main.fieldOfView <= 60) {
				if (dragSpeed < maxDragSpeed) {
					dragSpeed += 0.1f;
				}
			}

		}

		if (!Camera.main.GetComponent<CameraController>().isZooming)
		{

			if (!Input.GetMouseButton (0))
				return;

			Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);

			Vector3 move = new Vector3 (pos.x * dragSpeed, pos.y * dragSpeed, 0);

			transform.Translate (-move, Space.World);
		}
	}
}	