using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Camera cam;

	//UI Buttons
	public GameObject talkButton;
	public GameObject zoomOut;

	//Edge Panning
	public Vector3 pos_;
	public float mDelta = 50;
	public float speed = 5;

	//Checks if the camera is currently zooming to limit movement
	public bool isZooming;

	private bool isZoomed;

	// Property for isZoomed
	public bool IsZoomed
	{
		get { return isZoomed; }
	}

	//Default cam position
	public static Vector3 camDefault;

	//Duration of zoom
	public float zoomDuration = 0.5f;

	public float perspectiveZoomSpeed = 0.5f; 

	int tapCount;

	float doubleTapTimer;

	//Set zooming to false, set UI to false
	void Start() 
	{
		camDefault = Camera.main.transform.position;
		cam = Camera.main;
		isZooming = false;
		talkButton.SetActive (false);
		zoomOut.SetActive (false);
	}
		
	void Update () 
	{

		if (!isZooming) {
			pos_ = transform.position;

			pos_.x = Mathf.Clamp (transform.position.x, -6f, 6f);

			pos_.y = Mathf.Clamp (transform.position.y, -3f, 3f);

			transform.position = pos_;
		}

		if (Camera.main.fieldOfView == 19) 
		{
			isZoomed = true;

			talkButton.SetActive (true);

			zoomOut.SetActive (true);

		} 
		else 
		{

			isZooming = false;

			talkButton.SetActive (false);

			zoomOut.SetActive (false);

		}


		//EDGE PANNING//

		if (!isZooming) {
			if (!Input.GetMouseButton (0)) { 
				//Right
				if (Input.mousePosition.x >= Screen.width - mDelta) {
					transform.position += transform.right * Time.deltaTime * speed;
				}

				//Left
				if (Input.mousePosition.x <= 0 + mDelta) {
					transform.position -= transform.right * Time.deltaTime * speed;
				}



				//Up
				if (Input.mousePosition.y >= Screen.height - mDelta) {
					transform.position += transform.up * Time.deltaTime * speed;
				}

				//Down
				//Up
				if (Input.mousePosition.y <= 0 + mDelta) {
					transform.position -= transform.up * Time.deltaTime * speed;
				}
			}
		}

		//Set Layer Mask
		int layerMask = 1 << LayerMask.NameToLayer("Room");

		//Set Raycast
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);


		if (Input.GetMouseButtonDown (0)) 
		{

			tapCount++;

			if (tapCount > 0) 
			{

				doubleTapTimer += Time.deltaTime;

			}

			if (tapCount >= 2) 
			{
				
				if (Physics.Raycast (ray, out hit, layerMask)) 
				{
				
					if (!isZooming) 
					{

						isZooming = true;

						StartCoroutine (Zoom (hit));

						doubleTapTimer = 0.0f;

						tapCount = 0;

					}

				}

			}
		}
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			tapCount++;
		}
		if (tapCount > 0)
		{
			doubleTapTimer += Time.deltaTime;
		}
		if (tapCount >= 2)
		{

			if (Physics.Raycast (ray, out hit, layerMask))
			{
				if (!isZooming) 
				{
					
					isZooming = true;
					// Calls the smooth Zoom Coroutine

					StartCoroutine (Zoom (hit));

					// Enables the button
					//talkButton.SetActive (true);
					//zoomOut.SetActive (true);

				}

				doubleTapTimer = 0.0f;

				tapCount = 0;


			}

		}

		if (doubleTapTimer > 0.5f)
		{
				
			doubleTapTimer = 0f;

			tapCount = 0;


		}
	
	}

	void FixedUpdate() {

	}
		
	public void Return()
	{
		isZooming = false;
		// Resets camera to default location

		//camera.transform.position = camDefault;

		// Calls the smooth Zoom Out Coroutine

		StartCoroutine (Zoom_Out());

		// Disables the button once again
		talkButton.SetActive(false);
		zoomOut.SetActive (false);

	}

	IEnumerator Zoom(RaycastHit room)
	{

		// Slowly changes the camera to an FOV of 19

		float deltaT = 0f;

		while (deltaT < zoomDuration) 
		{
			isZooming = true;

			deltaT += Time.deltaTime;

			//Adjust this value to change speed of FOV change 

			yield return new WaitForSeconds (0.025f);

			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, 19, deltaT / zoomDuration);

			Vector3 pos = cam.transform.position;

			pos.x = room.transform.position.x;

			pos.y = room.transform.position.y;

			pos.z = -20.5f;

			cam.transform.position = Vector3.Lerp(cam.transform.position, pos, deltaT / zoomDuration);

			//camera.transform.position.x = Mathf.Lerp (transform.position.x, room.transform.position.x, deltaT / zoomDuration);

			isZooming = false;

		}

	}

	IEnumerator Zoom_Out()
	{

		// Slowly changes the camera to an FOV of 110

		float deltaT = 0f;

		while (deltaT < zoomDuration) 
		{
			isZooming = true;

			deltaT += Time.deltaTime;

			//Adjust this value to change speed of FOV change 

			yield return new WaitForSeconds (0.025f);

			Camera.main.fieldOfView = Mathf.Lerp (19, 60, deltaT / zoomDuration);

			//Vector3 pos = camDefault;

			cam.transform.position = Vector3.Lerp(cam.transform.position, camDefault, deltaT / zoomDuration);

			isZooming = false;
		}

	}
}
