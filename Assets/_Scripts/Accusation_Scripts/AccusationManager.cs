﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AccusationManager : MonoBehaviour {

    public static AccusationManager S;
    public Text cluesLeftHeading;
    public Text cluesLeftText;
    public Button finalizeAccusationButton;
    public int cluesLeftToChoose = 3;
    public string selectedCrew;
    public Text endText;
    //public Button replay;
    public GameObject crewObject; //Testing

    public bool ____________________;

    private float counter = 0;
    private float tempCounter = 0;
    private float maxIndex = 20;
    public GameObject[] crew;
    public float juryVote = 0;

    //This is for controlling OnAwake
    public bool crewSet;

    public GameObject[] crewDialogue;

	public GameObject resultsScreen;

    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else if (S != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
        //For now, needs to be inactive so that it can find things OnEnable
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start() {
        /*
		//finalizeAccusationButton = GameObject.Find ("FinalizeAccusationButton").GetComponent<Button> ();
		finalizeAccusationButton.gameObject.SetActive (false);
		cluesLeftHeading.gameObject.SetActive (false);
		cluesLeftText.text = cluesLeftToChoose.ToString ();
		cluesLeftText.gameObject.SetActive (false);
		endText.gameObject.SetActive (false);
		replay.gameObject.SetActive (false);
        */
        crew = GameObject.FindGameObjectsWithTag("Crew");

	    for (int z = 0; z < crew.Length; z++)
	    {
		    
		    crew[z].GetComponentInChildren<cakeslice.Outline>().enabled = false;
		    
	    }
	    
    }

    private void OnEnable()
    {
        crew = GameObject.FindGameObjectsWithTag("Crew");
    }

    // Update is called once per frame
    void Update() {

        RaycastForCrew();

        cluesLeftText.text = cluesLeftToChoose.ToString();
    }

    public void Talk()
    {
        for (int i = 0; i < crew.Length; i++)
        {
            if (crew != null)
                if (crew[i] == crewObject)
                {
                    foreach (GameObject dialogueObj in crewDialogue)
                        if (dialogueObj.name == crew[i].name + "_Dialogue")
                        {
                            crew[i].GetComponent<Crew>().DialogueText = dialogueObj;
                            break;
                        }
                    crew[i].GetComponent<Crew>().DisplayText();
                }
        }
    }

    // Use This function to disable all of the Accusation UI elements so that once the player presses
    // the finalizeAccusationButton they will be able to see who they have convinced and who they have not
    /*
	public void TurnOffAccusationUIElements()
	{
		// Turn off all of the clueEntry UI elements in the scene
		foreach (GameObject clueEntry in ClueManager._builtCluesMenu)
		{
			clueEntry.gameObject.SetActive (false);
		}

		// Turn off the Clues Left to Accuse heading and Clues Left number text
		cluesLeftText.gameObject.SetActive (false);
		cluesLeftHeading.gameObject.SetActive (false);

		// Turn off the visibility of the FinalizeAccusationButton
		finalizeAccusationButton.gameObject.SetActive (false);

	}
    */
    public void resetSelected() {
        selectedCrew = ""; //Resets which crew was selected
	    
	    for (int z = 0; z < crew.Length; z++)
	    {
		    
		    crew[z].GetComponentInChildren<cakeslice.Outline>().enabled = false;
		    
	    }
	    
    }

    //For Buttons to use: Clear ClueManager's _cluesToPresent so that the player can select a new set of Clues for accusation
    public void ResetCluesToPresent()
    {
        //Temp, should make number to choose a variable
        cluesLeftToChoose = 3;
        UI_ButtonManager.S.RemoveButtonsFromAccusation();
        // thisfinalizeAccusationButton.enabled = false;

        finalizeAccusationButton.gameObject.SetActive(false);
        ClueManager._cluesToPresent = new List<ClueInfo>();
    }

    /*
	public void DetermineCrewMembersVerdicts ()
	{
		Debug.Log ("TESTING FUNCTIONALITY");
		//grab all crew members
		//cycle through them
		//do the roll
		//grab the renderer an change the color based on the rol

		foreach (ClueInfo clue in ClueManager._cluesToPresent) {
			counter += clue.rating;//add all presented clue ratings into counter
		}

		Debug.Log ("Default Loyalty at: " + counter);
		maxIndex -= counter;//subtract counter from max index

		foreach (GameObject crewMember in crew) {
			//Checks list for game objects
			if (crewMember != null && crewMember.name != selectedCrew) {
				Renderer rend = crewMember.GetComponent<Renderer> ();
				tempCounter = counter;//temporary counter that resets to default counter

				float roll = Random.Range (0.0f, maxIndex);//get a roll between 0 and new maxIndex
				print ("Random Roll: " + roll);
				tempCounter += roll;//add to counter
				roll = 0;

				//add to every crew member
				Debug.Log ("Loyalty after calculations: " + tempCounter + " for " + crewMember);

				if (tempCounter >= 16) { //basing this on a 20 sided die like DnD
					rend.material.color = Color.green;
					juryVote++;
				} else {
					rend.material.color = Color.red;
					juryVote--;
				}
				//Checks which crew was selected so they don't vote
			} else if (crewMember != null && crewMember.name == selectedCrew) {
				Debug.Log ("You can't vote for your self" + selectedCrew);
			} else {
				Debug.Log ("Nothing happens");
			}
		}
	}
    */

    public void DetermineCrewMembersVerdicts()
    {
        Debug.Log("TESTING FUNCTIONALITY");
        //grab all crew members
        //cycle through them
        //do the roll
        //grab the renderer an change the color based on the roll

        foreach (ClueInfo clue in ClueManager._cluesToPresent)
        {
            counter += clue.rating;//add all presented clue ratings into counter
        }

        Debug.Log("Default Loyalty at: " + counter);
        maxIndex -= counter;//subtract counter from max index

        StartCoroutine(ShowCrewMemberVerdicts());
    }

    IEnumerator ShowCrewMemberVerdicts()

    {
        foreach (GameObject crewMember in crew)
        {
            yield return new WaitForSeconds(.6f);
            //Checks list for game objects
            if (crewMember != null && crewMember.name != selectedCrew)
            {
                Renderer rend = crewMember.GetComponent<Renderer>();
                tempCounter = counter;//temporary counter that resets to default counter

                float roll = Random.Range(0.0f, maxIndex);//get a roll between 0 and new maxIndex
                print("Random Roll: " + roll);
                tempCounter += roll;//add to counter
                roll = 0;

                //add to every crew member
                Debug.Log("Loyalty after calculations: " + tempCounter + " for " + crewMember);

                if (tempCounter >= 16)
                { //basing this on a 20 sided die like DnD

	                crewMember.GetComponentInChildren<cakeslice.Outline>().enabled = true;

	                crewMember.GetComponentInChildren<cakeslice.Outline>().color = 1;
	                
                    //rend.material.color = Color.green; //COLOR CHANGE
                                                       //Voting convinced
                    crewMember.GetComponentInChildren<Vote_Text>().VoteConvinced();

                    juryVote++;
                }
                else
                {
	                
	                crewMember.GetComponentInChildren<cakeslice.Outline>().enabled = true;

	                crewMember.GetComponentInChildren<cakeslice.Outline>().color = 2;
	                
                    //rend.material.color = Color.red; //COLOR CHANGE
                                                     //Voting unconvinced
                    crewMember.GetComponentInChildren<Vote_Text>().VoteUnconvinced();

                    juryVote--;
                }
                //Checks which crew was selected so they don't vote
            }
            else if (crewMember != null && crewMember.name == selectedCrew)
            {
                Debug.Log("You can't vote for your self" + selectedCrew);
            }
            else
            {
                Debug.Log("Nothing happens");
            }
        }
        yield return new WaitForSeconds(.4f);
        EndScreen();
    }

    public void EndScreen() 
	{
		
		if (juryVote >= 1) 
		{
			
			AudioSource success = GameObject.Find ("Success").GetComponent<AudioSource> ();

			success.Play ();

			endText.gameObject.SetActive (true);

		} 
		else 
		{

			AudioSource fail = GameObject.Find ("Failed").GetComponent<AudioSource> ();

			fail.Play ();

			endText.text = "ACCUSATION FAILED!";

			endText.gameObject.SetActive (true);

		}

		resultsScreen.GetComponent<ResultsScreen>().DisplayResultsScreen ();
		Debug.Log ("This was done!");

	}

	void RaycastForCrew() {
		/*if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Selecting crew");

			Vector3 mousePosition;
			RaycastHit hit;
			Ray ray;

			//Update ray
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			mousePosition = Input.mousePosition;

			//The raycast works but in scene view it points towards canvas
			Debug.DrawRay (transform.position, mousePosition, Color.green);*/
		
		if (Input.GetMouseButtonDown (0)) {
			// Create variables to store mouse position, raycast hit info and actual Ray info.
			Vector3 mousePosition;
			RaycastHit hit;
			Ray ray;
            
			// Set mousePosition to equal the x and y coordinates of where the user clicked the mouse
			mousePosition = Input.mousePosition;
			
			// Manually set the mousePosition's transform z-position;
			mousePosition.z = Camera.main.transform.position.z + 100.0f;
           
			// Transform the pixel coordinates of mousePosition to world coordinates
			Vector3 mouseWorldCoordinates = Camera.main.ScreenToWorldPoint (mousePosition);

			// Calculate the direction vector that the ray we cast from the camera will face
			Vector3 rayDirection = mouseWorldCoordinates - Camera.main.transform.position; 

			//Create a new Ray variable that starts at the camera's position and faces towards the direction we calculated in 'rayDirection'
			ray = new Ray(Camera.main.transform.position, rayDirection);

			// Visualization of the Raycast to make it easier to Debug
			Debug.DrawRay (Camera.main.transform.position,rayDirection, Color.green, 1.0f);

			if (Physics.Raycast (ray, out hit, 100.0f)) {
				if (hit.collider.tag == "Crew" && selectedCrew == "") 
				{

					
					AudioSource select = GameObject.Find ("Select").GetComponent<AudioSource> ();

					select.Play ();

					// Activate the sprite and corresponding TextBox to be visible when the crew member is clicked on.
					Debug.Log (hit.collider.gameObject.name + " was selected by Raycast");
                    CanvasManager.S.juryDialogueCanvas.SetActive(true);

					crewObject = hit.collider.gameObject;

					crewObject.GetComponentInChildren<cakeslice.Outline>().enabled = true;
					
					//Activate Sprites
					crewObject.GetComponent<Crew>().Sprite.SetActive (true);
					//Activate TextBox
					crewObject.GetComponent<Crew> ().TextBox.SetActive (true);
					//Saves selected crew for dialogue purposes
					selectedCrew = crewObject.name;
				}
			}
		}
	}
}
