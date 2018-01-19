using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenableItem : MonoBehaviour {

	public GameObject partThatOpens;
	public Vector3 openingRotations;

	public float animationTime = 		3.00f;

	bool inAnimation = 					false;
	bool isOpen = 						false;
	public bool requireZoom = 			false; // whether the camera should zoom into this or not when you click on it
	public float zoomTime = 			1f;
	Vector3 baseRotations;

	Vector3 originalCamPosition;
	Quaternion originalCamRotation;

	void Awake()
	{
		baseRotations = partThatOpens.transform.localRotation.eulerAngles;
		originalCamPosition = Camera.main.transform.position;
		originalCamRotation = Camera.main.transform.rotation;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isOpen && Input.GetKeyUp(KeyCode.Escape))
			Close ();
	}

	public void Open()
	{
		if (!inAnimation && !isOpen)
			StartCoroutine (OpeningAnimation());
	}

	public void Close()
	{
		if (!inAnimation && isOpen)
			StartCoroutine (ClosingAnimation ());
	}

	IEnumerator OpeningAnimation()
	{
		Debug.Log ("Opening!");
		StartCoroutine (ZoomInCoroutine ());
		inAnimation = true;
		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float framesToPass = frameRate * animationTime;

		Vector3 newRotations = baseRotations;
		Vector3 targetRotation = baseRotations + openingRotations;

		yield return null;

		while (timer < framesToPass) 
		{
			newRotations = Vector3.Lerp (baseRotations, targetRotation, timer / framesToPass);
			partThatOpens.transform.localRotation = Quaternion.Euler (newRotations);
			timer++;
			yield return null;
		}

		partThatOpens.transform.localRotation = Quaternion.Euler(targetRotation);
	
		inAnimation = false;
		isOpen = true;
	}

	IEnumerator ClosingAnimation()
	{
		Debug.Log ("Closing!");
		StartCoroutine (ZoomOutCoroutine ());
		inAnimation = true;
		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float framesToPass = frameRate * animationTime;

		Vector3 closedRotations = partThatOpens.transform.localRotation.eulerAngles;
		Vector3 targetRotation = closedRotations - openingRotations;
		Vector3 newRotations;

		yield return null;

		while (timer < framesToPass) 
		{
			newRotations = Vector3.Lerp (closedRotations, targetRotation, timer / framesToPass);
			partThatOpens.transform.localRotation = Quaternion.Euler (newRotations);
			timer++;
			yield return null;
		}

		partThatOpens.transform.localRotation = Quaternion.Euler(targetRotation);

		inAnimation = false;
		isOpen = false;
	}


	void OnMouseOver()
	{
		if (!isOpen && Input.GetMouseButtonUp (0))
			Open ();
		
	}
		
	IEnumerator ZoomInCoroutine()
	{
		StartCoroutine (MakeCameraFaceObject ());
		yield return StartCoroutine (MoveCloseToObject ());

	}

	IEnumerator ZoomOutCoroutine()
	{
		yield return StartCoroutine (MoveToOriginalPosition ());
	}

	IEnumerator MakeCameraFaceObject()
	{
		Quaternion cameraRotation = Camera.main.transform.rotation;
		Vector3 targetRotationVector = partThatOpens.transform.position - Camera.main.transform.position;
		
		float frameRate = 				1f / Time.deltaTime;
		float timer = 					0;
		float framesToPass = 			frameRate * zoomTime;


		while (timer < framesToPass) 
		{
			Camera.main.transform.rotation = Quaternion.Lerp (	Camera.main.transform.rotation, 
																Quaternion.LookRotation (targetRotationVector), 
																timer / framesToPass);
			timer++;
			yield return null;
		}

		yield return null;
	}

	IEnumerator MoveCloseToObject()
	{
		// move towards the object, stop at a few meters away

		Vector3 targetPos = partThatOpens.transform.position;
		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float framesToPass = frameRate * zoomTime;

		while ((targetPos - Camera.main.transform.position).magnitude > 3f) 
		{
			Camera.main.transform.position = Vector3.Lerp (	Camera.main.transform.position, 
															targetPos, 
															Time.deltaTime);
			timer++;
			yield return null;
		}

		yield return null;
	}

	IEnumerator MoveToOriginalPosition()
	{
		yield return null;

		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float framesToPass = frameRate * zoomTime;

		Vector3 baseCameraPos = Camera.main.transform.position;

		while (Camera.main.transform.position != originalCamPosition) 
		{
			Camera.main.transform.position = Vector3.Lerp (	baseCameraPos, 
															originalCamPosition, 
															timer / framesToPass);
			timer++;
			yield return null;
		}
	}
}
