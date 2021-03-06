using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;

public class CameraController : MonoBehaviour {

	//Singleton of this class
	public static CameraController S;

	public Camera cam;

	//UI Buttons
	public GameObject talkButton;
	public GameObject zoomOut;

	//Edge Panning
	public Vector3 pos_;
	public float mDelta = 10;
	public float speed = 5;

	//Checks if the camera is currently zooming to limit movement
	public bool isZooming;

	private bool isZoomed;

	// Property for isZoomed
	public bool IsZoomed
	{
		get { return isZoomed; }
	}
	//We can cancel out of this coroutine if necessary by assigning coroutines to this
	public Coroutine currentCoroutine;

	//Default cam position
	public static Vector3 camDefault;

	//Duration of zoom
	public float zoomDuration = 0.5f;

	public float perspectiveZoomSpeed = 0.5f;

    public float zoomFOV = 15;

	public static bool cook;

	public static bool medic;

	public static bool captain;

	public static bool engineer;

	int tapCount;

	int clickCount;

	float doubleTapTimer;

	public Controls_PC pcRef;

    public bool mayPanAround { get; protected set; }

	//Set zooming to false, set UI to false
    void Start()
    {

		camDefault = Camera.main.transform.position;
		cam = Camera.main;
		isZooming = false;
		talkButton.SetActive (false);
		zoomOut.SetActive (false);
		pcRef = GetComponent<Controls_PC> ();
		
	}

	void Awake()
	{
		// Populate the Singleton with the followint if and else if statements
		if (S == null)
		{
			S = this;
		}
		else if (S != null)
		{
			Destroy(this);
		}

		//DESTROY IF MORE THAN ONE COPY EXISTS
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

	}

	void Update () 
	{
		
		if (!isZooming) {
			pos_ = transform.position;

			pos_.x = Mathf.Clamp (transform.position.x, -18f, 18f);

			pos_.y = Mathf.Clamp (transform.position.y, -6f, 6f);

			transform.position = pos_;
		}

		if (Camera.main.fieldOfView == zoomFOV) 
		{
			isZoomed = true;

			//talkButton.SetActive (true);

			zoomOut.SetActive (true);

		} else {
			isZoomed = false;

			//isZooming = false;

			talkButton.SetActive (false);

			zoomOut.SetActive (false);

		}


		//EDGE PANNING//
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			EdgePanning();
		#endif

		//Set Layer Mask
		int layerMask = 1 << LayerMask.NameToLayer("Room");

		//Set Raycast
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) 
		{
			tapCount++;

		}

		if (tapCount > 0) 
		{
			doubleTapTimer += Time.deltaTime;
		}

		if (tapCount >= 2) {

			if (Physics.Raycast (ray, out hit, layerMask)) {
				if (hit.transform.gameObject.tag == "Room") {
					if (!isZooming) {
						isZooming = true;

						// Calls the smooth Zoom Coroutine
						currentCoroutine = StartCoroutine (Zoom (hit));
					}
					doubleTapTimer = 0.0f;
					tapCount = 0;
				}
			}
		}

		if (Input.GetMouseButtonDown (0)) 
		{

			clickCount++;

			if (clickCount >= 2) {

				if (Physics.Raycast (ray, out hit, layerMask)) {

					if (hit.transform.gameObject.tag == "Room") {

						if (!isZooming) {

							Debug.Log ("Okay, buddy. You just clicked the " + hit.transform.gameObject.name + "!");

							isZooming = true;

							currentCoroutine = StartCoroutine (Zoom (hit));

							//Debug.Log("Timer = " + doubleTapTimer);

							doubleTapTimer = 0.0f;

							clickCount = 0;

						}

					}
				}
			}
		}

		if (clickCount > 0) {

			doubleTapTimer += Time.deltaTime;

		}

		if (doubleTapTimer > 0.5f) 
		{
			doubleTapTimer = 0f;
			tapCount = 0;
			clickCount = 0;
		}

	}

    public void SetPanning(bool val)
    {
        // Need this to be visible in the inspector's button events
        mayPanAround = val;
    }

    /*********************
	 *  Custom Functions *
	 * ******************/

    void EdgePanning ()
	{
		if (!isZooming) {
			if (!Input.GetMouseButton (0)) { 
				//Right
				if (Input.mousePosition.x >= Screen.width - mDelta) {
					if (transform.position.x < 12) {
						transform.position += transform.right * Time.deltaTime * speed;
					}
				}

				//Left
				if (Input.mousePosition.x <= 0 + mDelta) {
					if (transform.position.x > -12) {
						transform.position -= transform.right * Time.deltaTime * speed;
					}
				}



				//Up
				if (Input.mousePosition.y >= Screen.height - mDelta) {
					if (transform.position.y < 6) {
						transform.position += transform.up * Time.deltaTime * speed;
					}
				}

				//Down
				if (Input.mousePosition.y <= 0 + mDelta) {
					if (transform.position.y > -6) {
						transform.position -= transform.up * Time.deltaTime * speed;
					}
				}
			}
		}
	}

	public void Return()
	{
		isZooming = false;
		// Resets camera to default location

		//camera.transform.position = camDefault;

		// Calls the smooth Zoom Out Coroutine

		currentCoroutine = StartCoroutine (Zoom_Out());

		// Disables the button once again
		talkButton.SetActive(false);
		zoomOut.SetActive (false);

	}

	IEnumerator Zoom(RaycastHit room)
	{

		// Slowly changes the camera to an FOV of 19

		float deltaT = 0f;

		while (deltaT < zoomDuration && cam.fieldOfView > zoomFOV) 
		{

			isZooming = true;

			while (pcRef.dragSpeed >= 0.5) {
				pcRef.dragSpeed -= 0.1f;
			}

			deltaT += Time.deltaTime;

			//Adjust this value to change speed of FOV change 

			yield return new WaitForSeconds (0.025f);

			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, zoomFOV, deltaT / zoomDuration);

			Vector3 pos = cam.transform.position;

			pos.x = room.transform.position.x;

			pos.y = room.transform.position.y;

			pos.z = -20.5f;

			cam.transform.position = Vector3.Lerp(cam.transform.position, pos, deltaT / zoomDuration);

			//camera.transform.position.x = Mathf.Lerp (transform.position.x, room.transform.position.x, deltaT / zoomDuration);

		}

		isZooming = false;

	}

	IEnumerator Zoom_Out()
	{
		
		// Slowly changes the camera to an FOV of 110

		float deltaT = 0f;

		while (deltaT < zoomDuration) 
		{
			isZooming = true;

			while (pcRef.dragSpeed <= 3) {
				pcRef.dragSpeed += 0.1f;
			}

			deltaT += Time.deltaTime;

			//Adjust this value to change speed of FOV change 

			yield return new WaitForSeconds (0.025f);

			Camera.main.fieldOfView = Mathf.Lerp (zoomFOV, 60, deltaT / zoomDuration);

			//Vector3 pos = camDefault;

			cam.transform.position = Vector3.Lerp(cam.transform.position, camDefault, deltaT / zoomDuration);

			isZooming = false;
		}

	}

}