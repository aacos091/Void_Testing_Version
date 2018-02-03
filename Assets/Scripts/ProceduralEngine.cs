using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using LitJson;
using Yarn;
using Yarn.Unity;

public class ProceduralEngine : MonoBehaviour {

	// TODO Rethink the names of these. Will they be placed inside of a class that holds the data for this playthroughs mystery?
	private CrewMember 		_culprit;
	private CrewMember		_victim;
	private string 			_murderLocation;
	private string 			_murderMethod;
	private const int 		STRONG_CLUE_RATING = 5;
	private const int 		WEAK_CLUE_RATING = 1;
	private const string	WEAK_CLUE_TAG = "Weak";
	private const string	STRONG_CLUE_TAG = "Strong";
	private List<string>	_crewMembers = new List<string>() {"Cook", "Engineer", "First Mate", "Medic", "Pilot"};		// This List holds the titles of all of the crewmembers. Will be used to determine TruthTeller1 and TruthTeller 2
	private string			_truthTeller1;
	private string			_truthTeller2;

	// TODO These values will change depending on the amount of clues each Culprit can have depending on the Murder location
	// TODO Change AMOUNT_OF_CLUES to represent Strong(1) and Weak(4
	private const int AMOUNT_OF_CLUES_PER_PLAYTHROUGH = 4;
	private const int AMOUNT_OF_STRONG_CLUES_PER_PLAYTHROUGH = 2;
	private const int AMOUNT_OF_WEAK_CLUES_PER_PLAYTHROUGH = 2;

	// This int represents the amount of clues present in each array of clues in the JSON files. 
	//Each strong and weak category should have the same number for this value to work
	[SerializeField]
	private int		CLUES_ARRAY_COUNT = 2;					
	// May have to change this list from string to ClueInfo
	private List<string>	_cluesList;
	private int 			AMOUNT_OF_CLUES;


	[Header("JSON Data files go in their corresponding slots below.")]
	public TextAsset 		crewFinal;
	public TextAsset		locationsFinal;
	public TextAsset		methodsFinal;
	public TextAsset		cluesFinal;
	public TextAsset 		clueDescriptionsFinal;

	[Header("Drop all the SpawnPoints in here")]
	public List<Transform>		spawnPoints;

	public Transform			shipTransform;

	//private StringBuilder	output = new StringBuilder ();

	void Awake ()
	{
		SetInformation ();
		// This function is just here to make sure that all the variables are being set.
		LogInfoToConsole ();
		
	}

	void Start()
	{
		//InitializeVariableStorage ();
		InitializeYarnVariables();
	}
	
	/****************************
	 * 	Self Defined Functions	*
	 * *************************/

	void LogInfoToConsole ()
	{
		string clues = "";
		print ("Culprit: " + _culprit.Title);
		print ("Victim: " + _victim.Title);
		print ("Murder Location: " + _murderLocation);
		print ("Murder Method: " + _murderMethod);
		print ("Truth Teller 1: " + _truthTeller1);
		print ("Truth Teller 2: " + _truthTeller2);

		for (int i = 0; i < _cluesList.Count; i++)
		{
			clues += "Clue " + (i + 1) + ": " + _cluesList [i] + " ";
		}

		print (clues);
	}

	public void SetInformation()
	{
		// Choose culprit and victim and display the info to the Text passed as a parameter
		_culprit = ChooseCulprit ();
		_victim = ChooseVictim ();
		// Choose the murder location and display it in the appropriate Text object
		_murderLocation = ChooseLocation ();
		//locationInfo.text = "The " + CULPRIT.Title + " murdered the " + VICTIM.Title + " in the " + _murderLocation;
		// Choose the murder method and display it in the appropriate Text object
		_murderMethod = ChooseMethod (_murderLocation);
		//methodInfo.text = "The method of murder was: " + _murderMethod;
		// Choose the clues and display them in the appropriate Text object
		ChooseClues();
		// Choose the two crew members that will be telling the truth when interviewed
		ChooseTruthTellers ();
	}

	// This method is run first in the Procedural Engine initial setup
	public CrewMember ChooseCulprit ()
	{
		// For the vertical slice of our game there can only be two possible Culprits (First Mate or Pilot). So we limit the choices to those
		// two here in this function. The values passed in here is the ID value that corresponds to each crew member in the JSON File crew.json
		// For culprit, the ID's are 3 and 5


		JsonData culpritData = JsonMapper.ToObject (crewFinal.text);
		//int randomID = Random.Range (0, 6);
		int randomID = ChooseID (3, 5);

		CrewMember void_Culprit = new CrewMember ();
		// Assign the randomID to _culpritID to ensure that we have an ID to check against
		void_Culprit.ID = randomID;

		// Assign the Title property of void_Culprit by drilling into the JsonData Object and extracting the "Title" entry for our random crew member
		void_Culprit.Title = culpritData ["Crew"] [randomID] ["Title"].ToString ();
		// This is the Culprit so we set it's bool of isCulprit to true
		void_Culprit.IsCulprit = true;

		// Clear the JsonData object
		culpritData.Clear();

		// Remove the culprit from the _crewMembers List
		_crewMembers.Remove (void_Culprit.Title);

		// Return the CrewMember object that holds the culprit's data
		return void_Culprit;

	}

	public CrewMember ChooseVictim ()
	{
		// For the vertical slice of our game there can only be two possible Victims (Engineer and Cook). So we limit the choices to those
		// two here in this function. The values passed in here is the ID value that corresponds to each crew member in the JSON File crew.json
		// The ID's are 1 and 2

		JsonData victimData = JsonMapper.ToObject (crewFinal.text);
		// int randomID = Random.Range (0, 6);
		int randomID = ChooseID (1, 2);
		// We would need to include a check here if we were to open the game up to randomly choose a victim from the
		// entire crew list. The check would ensure that the same ID as _culpritID is not assigned

		CrewMember void_Victim = new CrewMember ();
		void_Victim.Title = victimData ["Crew"] [randomID] ["Title"].ToString ();

		// Assign the chosen Victim's Title to _victimTitle. This will be used to find the appropriate Location in the json file.
		//void_Victim.title = _victimTitle;

		// Clear the JsonData Object
		victimData.Clear();

		// Remove the Victim from the _crewMembers List
		_crewMembers.Remove (void_Victim.Title);

		// Return the CrewMember object that holds the victim's data
		return void_Victim;
	}


	public string ChooseLocation ()
	{
		
		JsonData locationData = JsonMapper.ToObject (locationsFinal.text);

		// For now only two rooms are available and they are the only entry located on either the Cook or the Engineer in the json file.
		// so randomRoomID will equal 0
		int randomRoomID = 0;

		// Return the location found according to the value stored in _victimTitle;
		//string location = locationData ["Victim"] [0] ["Engineer"] [0].ToString();
		string location = locationData ["Victim"][_victim.Title][randomRoomID].ToString();

		// Clear the JsonData Object
		locationData.Clear();

		return location;
	}


	// ChooseMethod chooses the Murder method based on what the location is.
	public string ChooseMethod (string location)
	{
		JsonData methodData = JsonMapper.ToObject (methodsFinal.text);

		// Pick a random number between 0 an 1 because right now each room only has two possible methods
		int index = ChooseID (0, 1);
		// TODO Review this. Is this a modular approach to picking methods based off of the location room chosen?
		string method = methodData ["Location"] [_murderLocation] [index].ToString ();

		// Clear the JsonData Object
		methodData.Clear ();

		return method;
	}

	// TODO should this return a list of type ClueInfo? Each can have a boolean stating if it's a key clue, misleading clue or minor clue.
	public void ChooseClues ()
	{
		_cluesList = new List<string> ();
		List<int> indexesChosen = new List<int> ();		// Holds the indexes that we have already used. That way we don't select the same clue twice

		// Holds the random index value we are using to select the clue
		int randomIndex;

		// Create the JSON object to hold the list of clues
		JsonData clueData = JsonMapper.ToObject (cluesFinal.text);

		// In this for loop we are assuming that we are choosing the Strong Valued clues first
		// We first choose the Strong clues and rate them accordingly and then choose the weak clues and rate them accordingly
		for (int i = 0; i < AMOUNT_OF_CLUES_PER_PLAYTHROUGH; i++)
		{
			int clueRating = 0;
			string clueValidity = "";
		
			// The following if else statement sets the correct values for clueRating and clueValidity based on if Strong or Weak clues
			if (i < AMOUNT_OF_STRONG_CLUES_PER_PLAYTHROUGH)
			{
				clueRating = STRONG_CLUE_RATING;
				clueValidity = STRONG_CLUE_TAG; 
			}
			else
			{
				clueRating = WEAK_CLUE_RATING;
				clueValidity = WEAK_CLUE_TAG;
			}

			// Pick a valid index at Random as long as it has not been chosen before
			do {
				randomIndex = Random.Range (0, CLUES_ARRAY_COUNT);
			} while (indexesChosen.Contains (randomIndex));

			// Add value in randomIndex to indexesChosen to ensure that we do not choose this value again
			indexesChosen.Add (randomIndex);

			string clueToAdd = clueData ["Clues"] [_murderLocation] [_culprit.Title] [clueValidity] [randomIndex].ToString ();

			// Add clueToAdd to _clueList
			_cluesList.Add (clueToAdd);
			SpawnClues (clueToAdd, clueRating);

			// Clear the indexesChosen List once we have picked all the strong clues and are going to begin
			// choosing the weak clues
			if (i == AMOUNT_OF_STRONG_CLUES_PER_PLAYTHROUGH - 1)
				indexesChosen.Clear ();
		}
			
	}
		

	void SpawnClues (string cName, int rating)
	{
		// Create the JSON object to hold the clues Descriptions
		JsonData descriptionData = JsonMapper.ToObject (clueDescriptionsFinal.text);

		int spawnIndex = Random.Range (0, spawnPoints.Count);
		GameObject tGO = Instantiate (Resources.Load (cName, typeof(GameObject)) as GameObject, spawnPoints[spawnIndex].position, Quaternion.identity);
		tGO.name = cName;
		tGO.AddComponent<ClueItem> ();
		tGO.GetComponent<ClueItem> ().Rating = rating;
		tGO.GetComponent<ClueItem> ().ItemName = cName;
		tGO.GetComponent<ClueItem> ().Description = descriptionData ["Clues"] [cName] [0].ToString ();

		// Position this clue to it's correct Position
		PositionClue (tGO);

		// Make the clue a child of the Ship Object
		tGO.transform.SetParent (shipTransform);

		// Remove the spawnPoint used from the List of usable spawnPoints
		spawnPoints.RemoveAt (spawnIndex);

		// Clear the JSONData Object
		descriptionData.Clear();

	}


	int ChooseID (int i, int j)
	{
		// If random.value is greater than 0.5 return i if it's not then return j
		return ( Random.value > 0.5 ? i : j);
	}

	void ChooseTruthTellers ()
	{
		int index = 0;
		for (int i = 0; i < 2; i++) 
		{
			// Initialize index with a random number between 0 and Count of the _crewMembers List
			index = Random.Range (0, _crewMembers.Count);

			if (i == 0) 
			{
				_truthTeller1 = _crewMembers [index];
			} else 
			{
				_truthTeller2 = _crewMembers [index];
			}

			// Remove the entry in the _crewMembers List at position [index]
			_crewMembers.RemoveAt (index);
		}
	}

	// Function used to make sure the clues are positioned properly according to the size of their Colliders
	void PositionClue (GameObject go)
	{
		// Set the Offset in the up (y-direction) to be half the size of the collider
		float yOffset = go.GetComponent<Collider> ().bounds.extents.y;
		// Initialize targetPosition to equal the GameObjects position
		Vector3 targetPosition = go.transform.position;
		// Add yOffset to the targetPosition. This is necessary because the object is being set based on the spawnPoints position
		// and this is located at the center of the mesh.
		targetPosition.y += yOffset;

		// Set the final position of the GameObject to match the targetPosition;
		go.transform.position = targetPosition;
	}


	void InitializeVariableStorage ()
	{
		VariableStorageBehaviour varStorage = DialogueRunner.S.variableStorage;

		Yarn.Value culprit = new Yarn.Value (Culprit);
		culprit.variableName = "$Culprit";

		// Add the vars to the var storage using SetValue, passing the variable name and whatever kind of value
		// the var is supposed to point to. If the var is a string, pass varName.Yarn Value variable name as the second arg. 

		varStorage.SetValue (culprit.variableName, culprit);

		print ("Yarn Culprit Variable Name: " + culprit.variableName);
		print ("Yarn Culprit Variable Value: " + varStorage.GetValue(culprit.variableName).AsString);
	

	}

	void InitializeYarnVariables ()
    {
        Yarn.Value.Type boolType = Yarn.Value.Type.Bool;
        Yarn.Value.Type stringType = Yarn.Value.Type.String;

		ExampleVariableStorage varStorage = GameObject.Find ("DialogueSystem").GetComponent<ExampleVariableStorage>();
        
        // Go through as many cycles as we need to, in order to ensure that we create the proper yarn variables for each clue in the playthrough ("amountOfVoidClues")
        for (int i = 0; i < AMOUNT_OF_CLUES_PER_PLAYTHROUGH; i++)
        {
            // This integer will be converted to a string and appended to the end of each variable name.
            // E.G Clue1, Clue2, Clue3 etc
            int index = i + 1;
    

			// Make 4 temporary variables and set it to null everytime we begin the loop. 3 strings and one bool
			ExampleVariableStorage.DefaultVariable clueNameTDV = new ExampleVariableStorage.DefaultVariable();
			ExampleVariableStorage.DefaultVariable clueRelatedCrew1TDV = new ExampleVariableStorage.DefaultVariable();
			ExampleVariableStorage.DefaultVariable clueRelatedCrew2TDV = new ExampleVariableStorage.DefaultVariable();
			ExampleVariableStorage.DefaultVariable clueFoundTDV = new ExampleVariableStorage.DefaultVariable();

			// Assign the type of values that will be stored in each DefaultVariable type
			clueNameTDV.type = stringType;
			clueRelatedCrew1TDV.type = stringType;
			clueRelatedCrew2TDV.type = stringType;
			clueFoundTDV.type = boolType;

            //Assign the name for all of the created temporary variables, this is the name that will pop up in Yarn
            clueNameTDV.name = "Clue" + index.ToString();    // Clue1, Clue2 etc the number changes based on the value of i in the for loop
            clueRelatedCrew1TDV.name = "Clue" + index.ToString() + "RelatedCrew1";
            clueRelatedCrew2TDV.name = "Clue" + index.ToString() + "RelatedCrew2";
            clueFoundTDV.name = "Clue" + index.ToString() + "Found";

            // Assign the default values for each variable. This will be changed in ClueManager once each clue is found
            clueNameTDV.value = " ";
            clueRelatedCrew1TDV.value = " ";
            clueRelatedCrew2TDV.value = " ";
            clueFoundTDV.value = "false";

            // Add each of these created Yarn Variables to our List that will hold them all
           	varStorage.defaultVariables.Add (clueNameTDV);
            varStorage.defaultVariables.Add (clueRelatedCrew1TDV);
            varStorage.defaultVariables.Add (clueRelatedCrew2TDV);
            varStorage.defaultVariables.Add (clueFoundTDV);
        }
    }

	

	/***********************************
	 *  PROPERTIES to Grab information *
	 * ********************************/

	// The following are Properties for grabbing important information from this class
	public string Culprit
	{
		get { return _culprit.Title; }
	}

	public string Victim
	{
		get { return _victim.Title; }
	}

	public string MurderLocation
	{
		get { return _murderLocation; }
	}

	public string MurderMethod
	{
		get { return _murderMethod; }
	}

	public string TruthTeller1
	{
		get { return _truthTeller1; }
	}

	public string TruthTeller2
	{
		get { return _truthTeller2; }
	}
}
