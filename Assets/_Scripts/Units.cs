﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using cakeslice;

public class Units : MonoBehaviour 
{
	public Units S; // singleton

	public UnityEvent UnitStartedSpeaking { get; protected set; }

	//Array of waypoints
	public Transform[] waypoints;
	public Transform elevatorInsideOne;
	public Transform elevatorInsideTwo;

	//The 'agent' aka the Unit moving
	private NavMeshAgent agent;

	//Random variables
	public int ran;

	//Selected GameObject (Unit only)
	private GameObject mSelectedObject;

	//UI Text of selected unit
	public Text unitNameText;

	//Floor variable
	public int floor;

	public GameObject closestElevator;
	[SerializeField]
	GameObject closestElevatorToWaypoint;

	public Waypoint waypoint = null;

	public bool isAtElevator;

	public GameObject talk;

	public static bool dialogue;

	public bool cook;

	public bool medic;

	public bool captain;

	public bool engineer;

	public GameObject chatManager;

	public cakeslice.Outline[] outlineRef;

	public enum States
	{
		movingToDestination,
		movingToElevator
	}

	[SerializeField]
	States currentState = States.movingToDestination;

	void Awake()
	{
		S = this;
		UnitStartedSpeaking = new UnityEvent();

	}
	void Start () 
	{
		outlineRef = this.GetComponentsInChildren<cakeslice.Outline> ();

		foreach (cakeslice.Outline outl in outlineRef){
			outl.GetComponent<cakeslice.Outline> ().enabled = false;
		}

		isAtElevator = false;
		checkFloor ();
		unitNameText.GetComponent<Text> ().enabled = false;
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update () 
	{

		if (dialogue == true) 
		{
			talk.SetActive (true);
		}
		else
		{
			talk.SetActive (false);
		}

		checkFloor ();

        if (!GameController.S.gamePaused)
        {
            //Process object selection
            if (Input.GetMouseButtonUp(0))
                SelectObjectByMousePos();
        }

        switch (currentState)
        {
            case States.movingToElevator:
                {
                    if (!isAtElevator)
                        walkToNearestElevator();

                    if (isAtElevator)
                    {
                        teleportToClosestElevatorToWaypoint();
                        agent.SetDestination(waypoints[ran].transform.position);
                        currentState = States.movingToDestination;
                    }
                }
                break;

            case States.movingToDestination:
                {
                    if (!agent.hasPath && agent.remainingDistance <= 0)
                        GotoNextPoint();

                }
                break;
        }

	}

	//This checks the floor that the Unit is on. 
	public void checkFloor()
	{
		if (gameObject.transform.position.y <= -4.5)
			floor = 1;
		if (gameObject.transform.position.y >= -4.5 && gameObject.transform.position.y < -3)
			floor = 2;
		if (gameObject.transform.position.y > -3 && gameObject.transform.position.y < -0.635)
			floor = 3;
		if (gameObject.transform.position.y > -0.635)
			floor = 4;
	}

	private void SelectObjectByMousePos()
	{
		int layerMask = 1 << 9;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, layerMask)) 
		{
			if (hit.collider.tag == "Unit") 
			{
				//Get game object
				GameObject rayCastedGO = hit.collider.gameObject;

				//Select object
				this.SelectedObject = rayCastedGO;
                rayCastedGO.GetComponent<AnimationController>().StartTalking();
			}
		} 
		else 
		{

			//dialouge = false;

			if (mSelectedObject != null) {
				mSelectedObject.GetComponentInChildren<cakeslice.Outline> ().enabled = false;
			}
			this.SelectedObject = null;
			unitNameText.GetComponent<Text> ().enabled = false;
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
                //goOld.GetComponent<Renderer>().material = SimpleMat;
				goOld.GetComponentInChildren<cakeslice.Outline>().enabled = false;
			}

			//Set material to selected object
			if (mSelectedObject != null) 
			{

				dialogue = true;

				//Set unitNameText to unit name
				unitNameText.text = mSelectedObject.name;

				//Pass unit name to dialogue manager
				chatManager.GetComponent<Chat>().unitName = unitNameText.text.ToString();

				//Set highlight material
				mSelectedObject.GetComponentInChildren<cakeslice.Outline>().enabled = true;

				//if (this.gameObject.name == "Cook" && gameObject.GetComponent<Renderer> ().sharedMaterial == HighlightedMat)
				if (this.gameObject.name == "Cook" && mSelectedObject.GetComponentInChildren<cakeslice.Outline>().enabled == true)
                {

					CameraController.cook = true;

				}

                //if (gameObject.name == "Medic" && gameObject.GetComponent<Renderer> ().sharedMaterial == HighlightedMat) 
				if (gameObject.name == "Medic" && mSelectedObject.GetComponentInChildren<cakeslice.Outline>().enabled == true)
                {

                    CameraController.medic = true;

				}

                //TODO: RETAG "Captain" to "First Mate"!!!!!!!!!!

                //if (gameObject.name == "Captain" && gameObject.GetComponent<Renderer> ().sharedMaterial == HighlightedMat)                 
				if (gameObject.name == "Captain" && mSelectedObject.GetComponentInChildren<cakeslice.Outline>().enabled == true)
                {

                    CameraController.captain = true;

				}
                //if (gameObject.name == "Engineer" && gameObject.GetComponent<Renderer>().sharedMaterial == HighlightedMat)

				if (gameObject.name == "Engineer" && mSelectedObject.GetComponentInChildren<cakeslice.Outline>().enabled == true) 
				{

					CameraController.engineer = true;

				}

				//Set text to true
				unitNameText.GetComponent<Text> ().enabled = true;

				NPC npc = mSelectedObject.GetComponent<NPC>();
				if (npc != null)
				{
					Debug.Log("Clicked on an NPC!");
					TalkButton.S.dialogueNode = npc.dialogueNode;
				}
				

			} else {

				dialogue = false;

				CameraController.cook = false;

				CameraController.medic = false;

				CameraController.captain = false;

				CameraController.engineer = false;

			}

		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Elevator") {
			isAtElevator = true;
		}
	}

	void OnTriggerExit (Collider other){
		if (other.gameObject.tag == "Elevator") {
			isAtElevator = false;
		}
	}


	void GotoNextPoint ()
	{

		int chance = Random.Range (0, 100);

		int near = Random.Range (0, 100);

		if (near == 1) 
		{

			Vector3 nearest = (findClosestWaypointToUnit().transform.position);

			if (findClosestWaypointToUnit().GetComponent<Waypoint> ().isCollidingWithUnit != true) {
				agent.SetDestination (nearest);
			}
	
			currentState = States.movingToDestination;
		}

		if (chance == 1) 
		{

			//Roll a random number to determine which nav point to move to
			ran = Random.Range (0, 7);

			//Set waypoint to reference to waypoint chosen
			waypoint = waypoints [ran].GetComponent<Waypoint> ();

			//Find the closest elevator to that waypoint
			closestElevatorToWaypoint = findClosestElevatorToWaypoint ();
			Debug.Log (closestElevatorToWaypoint.name);

			//Set agent to walk towards that nav point

			if (floor == waypoint.floor) {

				if (waypoints [ran].GetComponent<Waypoint> ().isCollidingWithUnit != true) {
					agent.SetDestination (waypoints [ran].position);
				}
			}

			//If the waypoint is not on the same floor as the unit:
			else if (floor != waypoint.floor) {
				currentState = States.movingToElevator;
			}

		}

	}

	void walkToNearestElevator(){
		closestElevator = findClosestElevator ();
		agent.SetDestination (closestElevator.transform.position);
		if (isAtElevator && agent.remainingDistance <=0) {
			currentState = States.movingToDestination;
		}
	}

	void teleportToClosestElevatorToWaypoint()
	{

		if (isAtElevator) 
		{

			gameObject.SetActive (false);

			Invoke ("Elevator", 5);

		}

	}


	public GameObject findClosestElevator()
	{
		GameObject[] elevators;
		elevators = GameObject.FindGameObjectsWithTag ("Elevator");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject elevator in elevators) {
			Vector3 diff = elevator.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = elevator;
				distance = curDistance;
			}
		}
		return closest;
	}

	public GameObject findClosestWaypointToUnit()
	{
		GameObject[] waypoints;
		waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject waypoint in waypoints) {
			Vector3 diff = waypoint.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = waypoint;
				distance = curDistance;
			}
		}
		return closest;
	}

	public GameObject findClosestElevatorToWaypoint()
	{
		GameObject[] elevators;
		elevators = GameObject.FindGameObjectsWithTag ("Elevator");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = waypoint.transform.position;
		foreach (GameObject elevator in elevators) {
			Vector3 diff = elevator.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = elevator;
				distance = curDistance;
			}
		}
		return closest;
	}

	void Elevator()
	{

		transform.position = closestElevatorToWaypoint.transform.position;

		gameObject.SetActive (true);

	}


}