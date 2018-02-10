using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOutline : MonoBehaviour 
{
	public GameObject[] startingRooms;
	public GameObject mSelectedObject;

	// Use this for initialization
	void Start () {
		startingRooms = GameObject.FindGameObjectsWithTag ("roomGeometry");

		foreach (GameObject room in startingRooms){
			room.GetComponent<cakeslice.Outline> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		SelectObjectByMousePos ();

		if(Camera.main.fieldOfView < 20) {
			foreach (GameObject room in startingRooms){
				room.GetComponent<cakeslice.Outline> ().enabled = false;
			}
		}
	}

	private void SelectObjectByMousePos()
	{
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) 
		{
			if (hit.collider.tag == "roomGeometry") 
			{

				//Get game object
				GameObject rayCastedGO = hit.collider.gameObject;

				//Select object
				this.SelectedObject = rayCastedGO;

			}

		} 
		else 
		{
			mSelectedObject.GetComponent<cakeslice.Outline> ().enabled = false;
			this.SelectedObject = null;
		}
	}

	public GameObject SelectedObject {
		get {
			return mSelectedObject;
		}
		set {
			//get old game object
			GameObject goOld = mSelectedObject;

			//assign new game object
			mSelectedObject = value;

			//If this object is this same, skip
			if (goOld == mSelectedObject) {
				return;
			}

			//Set material to non-selected object
			if (goOld != null) {
				goOld.GetComponent<cakeslice.Outline> ().enabled = false;
			}

			//Set material to selected object
			if (mSelectedObject != null) {
				//Set highlight material
				mSelectedObject.GetComponent<cakeslice.Outline> ().enabled = true;
			}
		}
	}
}
