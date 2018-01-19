using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_And_Zoom : MonoBehaviour {
	
	public float speed = 0.025F;

	public float perspectiveZoomSpeed = 0.025f;

	public static float zoomInPan = 0f;

	private Vector3 dragOrigin;

	public float dragSpeed = 2;

	void Update()
	{
		if (Camera.main.fieldOfView < 80) 
		{

			speed = 0.0125f;

		} 
		else 
		{
			speed = 0.025f;
		}


		// PC Controls
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 109f);

			Camera.main.fieldOfView--;

		} 
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) 
		{

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 20f, 109f);

			Camera.main.fieldOfView++;

		}
			
		#if UNITY_ANDROID
		#elif UNITY_IOS
		// Mobile Controls

		if (Input.touchCount == 2) 
		{

			Touch touchZero = Input.GetTouch (0);

			Touch touchOne = Input.GetTouch (1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;

			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;

			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView, 19f, 110f);

		}

		else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) 
		{
			
			Vector3 touchDeltaPosition = Input.GetTouch (0).deltaPosition;

			Vector3 pos_ = transform.position;

			transform.Translate (-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);

			pos_.x = Mathf.Clamp(transform.position.x, -35f, 35f);

			transform.Translate (-touchDeltaPosition.y * speed, -touchDeltaPosition.y * speed, 0);

			pos_.y = Mathf.Clamp(transform.position.y, -15f, 15f);

			transform.position = pos_;
		}

		#endif
	}
		
}
