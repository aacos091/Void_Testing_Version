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
	public static ProceduralEngine S;		// Singleton Instance of ProceduralEngine

	// TODO Rethink the names of these. Will they be placed inside of a class that holds the data for this playthroughs mystery?
	private CrewMember		_culprit; // Change to crewmember type
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
	private string			_misleadingCrewMember;
    [SerializeField]
    private GameObject      _dialogueSystem;

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
	[SerializeField]private ExampleVariableStorage	yarnVarRef;


	[Header("JSON Data files go in their corresponding slots below.")]
	public TextAsset 		crewFinal;
	public TextAsset		locationsFinal;
	public TextAsset		methodsFinal;
	public TextAsset		cluesFinal;
	public TextAsset 		clueDescriptionsFinal;
	public TextAsset		cluePossibleOwnersFinal;

	[Header("Drop all the SpawnPoints in here")]
	public List<Transform>		spawnPoints;

	public Transform			shipTransform;
	public GameObject			dialogueSystem;

	//private StringBuilder	output = new StringBuilder ();

	void Awake ()
	{
		SetInformation ();
		// This function is just here to make sure that all the variables are being set.
		LogInfoToConsole ();
	}

	void Start()
	{
		yarnVarRef = dialogueSystem.GetComponent <ExampleVariableStorage>();
		//InitializeVariableStorage ();
		InitializeYarnVariables();
	}
	
	/****************************
	 * 	Self Defined Functions	*
	 * *************************/

	void LogInfoToConsole ()
	{
		string clues = "";
		print ("Culprit: " + Culprit);
		print ("Victim: " + Victim);
		print ("Murder Location: " + MurderLocation);
		print ("Murder Method: " + MurderMethod);
		print ("Truth Teller 1: " + TruthTeller1);
		print ("Truth Teller 2: " + TruthTeller2);

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
		// Choose the crew member that will unintentionally point you in the wrong direction
		ChooseMisleadingCrewMember();
	}

	//This method is run first in the Procedural Engine initial setup
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


	//ChooseMethod chooses the Murder method based on what the location is.
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

			//string clueToAdd = clueData ["Clues"] [_murderLocation] [_culprit.Title] [clueValidity] [randomIndex].ToString ();
			string clueToAdd = null;
			if (MurderMethod == "Violence")
				clueToAdd = clueData ["Clues"] [MurderMethod] [Culprit] [clueValidity] [randomIndex].ToString();
			else
				clueToAdd = clueData ["Clues"] [MurderMethod] [MurderLocation] [Culprit] [clueValidity] [randomIndex].ToString();

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
		JsonData clueOwnerData = JsonMapper.ToObject (cluePossibleOwnersFinal.text);

		int spawnIndex = Random.Range (0, spawnPoints.Count);
		GameObject tGO = Instantiate (Resources.Load (cName, typeof(GameObject)) as GameObject, spawnPoints[spawnIndex].position, Quaternion.identity);
		tGO.name = cName;
		tGO.AddComponent<ClueItem> ();
		tGO.GetComponent<ClueItem> ().Rating = rating;
		tGO.GetComponent<ClueItem> ().ItemName = cName;
		tGO.GetComponent<ClueItem> ().Description = descriptionData ["Clues"] [cName] [0].ToString ();
		tGO.GetComponent<ClueItem>().ClueOwner1 = clueOwnerData ["Clues"] [MurderLocation] [MurderMethod] [cName] [0].ToString();
		tGO.GetComponent<ClueItem>().ClueOwner2 = clueOwnerData ["Clues"] [MurderLocation] [MurderMethod] [cName] [1].ToString();
		tGO.GetComponent<ClueItem>().Location = spawnPoints[spawnIndex].GetComponent<SpawnPoint>().Location;
		// Position this clue to it's correct Position
		PositionClue (tGO);

		// Make the clue a child of the Ship Object
		tGO.transform.SetParent (shipTransform);

		// Remove the spawnPoint used from the List of usable spawnPoints
		spawnPoints.RemoveAt (spawnIndex);

		// Clear the JSONData Object
		descriptionData.Clear();
		clueOwnerData.Clear();

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

	void ChooseMisleadingCrewMember ()
	{
		foreach (string c in _crewMembers)
		{
			if (c == Culprit || c == Victim || c == TruthTeller1 || c == TruthTeller2)
			break;
			else
			_misleadingCrewMember = c;
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


	// void InitializeVariableStorage ()
	// {
	// 	VariableStorageBehaviour varStorage = DialogueRunner.S.variableStorage;

	// 	Yarn.Value culprit = new Yarn.Value (Culprit);
	// 	culprit.variableName = "$Culprit";

	// 	// Add the vars to the var storage using SetValue, passing the variable name and whatever kind of value
	// 	// the var is supposed to point to. If the var is a string, pass varName.Yarn Value variable name as the second arg. 

	// 	varStorage.SetValue (culprit.variableName, culprit);

	// 	print ("Yarn Culprit Variable Name: " + culprit.variableName);
	// 	print ("Yarn Culprit Variable Value: " + varStorage.GetValue(culprit.variableName).AsString);
	

	// }

	// Yarn function to change each CrewMention variables.
	 [YarnCommand ("ChangeMention")]
	 public void ChangeMention (string title, string newValue)
	 {
		 //Debug.LogError ("Change Mention called");
		 foreach (ExampleVariableStorage.DefaultVariable c in yarnVarRef.defaultVariables)
		 {
			 string mentionString = "$" + title + "Mention";
			 if (c.name == mentionString)
			 {
				 c.value = newValue;
			 } 	
		 }
	 }

	 // Yarn function to change InterviewB tracking variables
	 [YarnCommand ("UnlockInterviewB")]
	 public void UnlockInterviewB (string varToFind, string newValue)
	 {
		 string valueToCompare = "$" + varToFind;
		 foreach (ExampleVariableStorage.DefaultVariable c in yarnVarRef.defaultVariables)
		 {
			 if (c.name == valueToCompare)
			 {
				 c.value = newValue;
			 }
		 }
	 }
	void InitializeYarnVariables ()
    {
        Yarn.Value.Type boolType = Yarn.Value.Type.Bool;
        Yarn.Value.Type stringType = Yarn.Value.Type.String;

        ExampleVariableStorage varStorage = _dialogueSystem.GetComponent<ExampleVariableStorage>(); //GameObject.Find ("DialogueSystem").GetComponent<ExampleVariableStorage>();

        // Create the base Yarn variables such as $CRIMESCENE, $VICTIM, $SUSPECT, $METHOD, $CRIMELOCATION, $TruthTeller1, 2, $PilotMention, $CookMention etc
        ExampleVariableStorage.DefaultVariable crimeSceneTDV = new ExampleVariableStorage.DefaultVariable();
        ExampleVariableStorage.DefaultVariable victimTDV = new ExampleVariableStorage.DefaultVariable();
        ExampleVariableStorage.DefaultVariable suspectTDV = new ExampleVariableStorage.DefaultVariable();
        ExampleVariableStorage.DefaultVariable murderMethodTDV = new ExampleVariableStorage.DefaultVariable();
        ExampleVariableStorage.DefaultVariable crimeLocationTDV = new ExampleVariableStorage.DefaultVariable();
		// Start of TT and Mention Variables
		ExampleVariableStorage.DefaultVariable truthTeller1TDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable truthTeller2TDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable misleadingCrewTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable cookMentionTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable engineerMentionTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable firstMateMentionTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable medicMentionTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable pilotMentionTDV = new ExampleVariableStorage.DefaultVariable();
		// Create the InterviewB mention variables
		ExampleVariableStorage.DefaultVariable tt1InterviewBOpenTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable tt2InterviewBOpenTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable misleadingInterviewBOpenTDV = new ExampleVariableStorage.DefaultVariable();
		ExampleVariableStorage.DefaultVariable culpritInterviewBOpenTDV = new ExampleVariableStorage.DefaultVariable();

        // Assign the types for the variables
        crimeSceneTDV.type = stringType;
        victimTDV.type = stringType;
        suspectTDV.type = stringType;
        murderMethodTDV.type = stringType;
		// Assign the TT and Mention variable types
		truthTeller1TDV.type = stringType;
		truthTeller2TDV.type = stringType;
		misleadingCrewTDV.type = stringType;
		cookMentionTDV.type = stringType;
		engineerMentionTDV.type = stringType;
		firstMateMentionTDV.type = stringType;
		medicMentionTDV.type = stringType;
		pilotMentionTDV.type = stringType;
		// Assign the InterviewB mention types
		tt1InterviewBOpenTDV.type = stringType;
		tt2InterviewBOpenTDV.type = stringType;
		misleadingInterviewBOpenTDV.type = stringType;
		culpritInterviewBOpenTDV.type = stringType;

        // Assign the names for these variables, this is the names that will be used to access them in Yarn
        crimeSceneTDV.name = "$CRIMESCENE";
        victimTDV.name = "$VICTIM";
        suspectTDV.name = "$SUSPECT";
        murderMethodTDV.name = "$MURDERMETHOD";
		// Assign the names for the TT and Mention Variables
		truthTeller1TDV.name = "$TruthTeller1";
		truthTeller2TDV.name = "$TruthTeller2";
		misleadingCrewTDV.name = "$MisleadingCrew";
		cookMentionTDV.name = "$CookMention";
		engineerMentionTDV.name = "$EngineerMention";
		firstMateMentionTDV.name = "$FirstMateMention";
		medicMentionTDV.name = "$MedicMention";
		pilotMentionTDV.name = "$PilotMention";
		// Assign the names for the InterviewB variables
		tt1InterviewBOpenTDV.name = "$TT1InterviewBOpen";
		tt2InterviewBOpenTDV.name = "$TT2InterviewBOpen";
		misleadingInterviewBOpenTDV.name = "$MisleadingInterviewBOpen";
		culpritInterviewBOpenTDV.name = "$CulpritInterviewBOpen";

        // Assign the values that will be held in these yarn Variables
        crimeSceneTDV.value = MurderLocation;
        victimTDV.value = Victim;
        suspectTDV.value = Culprit;
        murderMethodTDV.value = MurderMethod;
		// Assign the values that will be held in the TT1, TT2 and mention yarn Variables
		truthTeller1TDV.value = TruthTeller1;
		truthTeller2TDV.value = TruthTeller2;
		misleadingCrewTDV.value = MisleadingCrewMember;
		cookMentionTDV.value = "false";
		engineerMentionTDV.value = "false";
		firstMateMentionTDV.value = "false";
		medicMentionTDV.value = "false";
		pilotMentionTDV.value = "false";
		// Assign the values that will be held in InterviewB variables
		tt1InterviewBOpenTDV.value = "false";
		tt2InterviewBOpenTDV.value = "false";
		misleadingInterviewBOpenTDV.value = "false";
		culpritInterviewBOpenTDV.value = "false";


        // Add each of these created Yarn Variables to our List that will hold them all
        varStorage.defaultVariables.Add(crimeSceneTDV);
        varStorage.defaultVariables.Add(victimTDV);
        varStorage.defaultVariables.Add(suspectTDV);
        varStorage.defaultVariables.Add(murderMethodTDV);
		// Adding the TT1, TT2 and mention variables to Yarn
		varStorage.defaultVariables.Add(truthTeller1TDV);
		varStorage.defaultVariables.Add(truthTeller2TDV);
		varStorage.defaultVariables.Add(misleadingCrewTDV);
		varStorage.defaultVariables.Add(cookMentionTDV);
		varStorage.defaultVariables.Add(engineerMentionTDV);
		varStorage.defaultVariables.Add(firstMateMentionTDV);
		varStorage.defaultVariables.Add(medicMentionTDV);
		varStorage.defaultVariables.Add(pilotMentionTDV);
		// Adding the InterviewBOpen variables
		varStorage.defaultVariables.Add(tt1InterviewBOpenTDV);
		varStorage.defaultVariables.Add(tt2InterviewBOpenTDV);
		varStorage.defaultVariables.Add(misleadingInterviewBOpenTDV);
		varStorage.defaultVariables.Add(culpritInterviewBOpenTDV);


        // Now Actually Set the values
        varStorage.SetValue (crimeSceneTDV.name, new Yarn.Value(MurderLocation));
        varStorage.SetValue (victimTDV.name, new Yarn.Value(Victim));
        varStorage.SetValue(suspectTDV.name, new Yarn.Value(Culprit));
        varStorage.SetValue(murderMethodTDV.name, new Yarn.Value(MurderMethod));
		// Setting the TT1, TT2, and Mention variables
		varStorage.SetValue (truthTeller1TDV.name, new Yarn.Value (TruthTeller1));
		varStorage.SetValue (truthTeller2TDV.name, new Yarn.Value (TruthTeller2));
		varStorage.SetValue(misleadingCrewTDV.name, new Yarn.Value (MisleadingCrewMember));
		varStorage.SetValue (cookMentionTDV.name, new Yarn.Value("no"));
		varStorage.SetValue (engineerMentionTDV.name, new Yarn.Value("no"));
		varStorage.SetValue (firstMateMentionTDV.name, new Yarn.Value("no"));
		varStorage.SetValue (medicMentionTDV.name, new Yarn.Value("no"));
		varStorage.SetValue (pilotMentionTDV.name, new Yarn.Value("no"));
		// Setting the values for InterviewB variables
		varStorage.SetValue (tt1InterviewBOpenTDV.name, new Yarn.Value("false"));
		varStorage.SetValue (tt2InterviewBOpenTDV.name, new Yarn.Value("false"));
		varStorage.SetValue (misleadingInterviewBOpenTDV.name, new Yarn.Value("false"));
		varStorage.SetValue (culpritInterviewBOpenTDV.name, new Yarn.Value("false"));

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
			ExampleVariableStorage.DefaultVariable clueLocationTDV = new ExampleVariableStorage.DefaultVariable();

			// Assign the type of values that will be stored in each DefaultVariable type
			clueNameTDV.type = stringType;
			clueRelatedCrew1TDV.type = stringType;
			clueRelatedCrew2TDV.type = stringType;
			clueFoundTDV.type = boolType;
			clueLocationTDV.type = stringType;

            //Assign the name for all of the created temporary variables, this is the name that will pop up in Yarn
            clueNameTDV.name = "$Clue" + index.ToString();    // Clue1, Clue2 etc the number changes based on the value of i in the for loop
            clueRelatedCrew1TDV.name = "$Clue" + index.ToString() + "Owner1";
            clueRelatedCrew2TDV.name = "$Clue" + index.ToString() + "Owner2";
            clueFoundTDV.name = "$Clue" + index.ToString() + "Found";
			clueLocationTDV.name = "$Clue" + index.ToString() + "Location";


            // Assign the default values for each variable. This will be changed in ClueManager once each clue is found
            clueNameTDV.value = " ";
            clueRelatedCrew1TDV.value = " ";
            clueRelatedCrew2TDV.value = " ";
            clueFoundTDV.value = "false";
			clueLocationTDV.value = " ";

            // Add each of these created Yarn Variables to our List that will hold them all
           	varStorage.defaultVariables.Add (clueNameTDV);
            varStorage.defaultVariables.Add (clueRelatedCrew1TDV);
            varStorage.defaultVariables.Add (clueRelatedCrew2TDV);
            varStorage.defaultVariables.Add (clueFoundTDV);
			varStorage.defaultVariables.Add (clueLocationTDV);
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

	public string MisleadingCrewMember
	{
		get { return _misleadingCrewMember; }
	}
}
