using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls_PC: MonoBehaviour 
{

	public float dragSpeed = 2;

	private Vector3 dragOrigin;

	private float moveSmooth = 0.1f;

	#if UNITY_STANDALONE_EDITOR
	#elif UNITY_STANDALONE_WIN 

	void Update () 
	{


		if (Input.GetMouseButtonDown (0)) 
		{

			dragOrigin = Input.mousePosition;

			return;

		}

		// PC Controls
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 60f);

			Camera.main.fieldOfView--;

			if (Camera.main.fieldOfView >= 19) {
				dragSpeed -= 0.1f;
			}

		} 
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 60f);

			Camera.main.fieldOfView++;

			if (Camera.main.fieldOfView <= 60) {
				dragSpeed += 0.1f;
			}

		}
			
		if (!Input.GetMouseButton (0)) return;

		Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);

		Vector3 move = new Vector3 (pos.x * dragSpeed, pos.y * dragSpeed, 0);

		transform.Translate (-move, Space.World);

		Vector3 pos_ = transform.position;

		pos_.x = Mathf.Clamp(transform.position.x, -6f, 6f);

		pos_.y = Mathf.Clamp(transform.position.y, -3f, 3f);

		transform.position = pos_;

	#endif
	}
}
