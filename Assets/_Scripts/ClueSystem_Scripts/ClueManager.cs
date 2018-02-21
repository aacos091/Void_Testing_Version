using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class ClueManager : MonoBehaviour
{

    // List to hold all of the clues gathered so far
    public static List<ClueItem> _cluesCollected;
    public static ClueManager S;            // Singleton of this class;
    public static List<ClueInfo> _clueInfoOfCollected;
    public static GameObject[] _builtCluesMenu;
    public static Dictionary<string, int> _dictClueNameToIndex; // Dictionary to store which index of _clueInfoOfCollected corresponds to each clueName;
    public static List<ClueInfo> _cluesToPresent;       // clues that will be presented during accusation
    public static ClueInfo currentClue; //The clue that is being investigated

    private int _cluesToSelect;
    private Text _cluesCollectedText;
    private LayerMask _clueLayer;
    private Image clueImage;
    public GameObject clueEntry;
    private static List<string> _nameOfCluesToPresent;  // name of the clues to present. Will be used to search corresponding clue information

    public GameObject yarnVarRefObj;
    ExampleVariableStorage yarnVarRef;
    int yarnIndex = 1;

    //[SerializeField]
    //private bool touchControlled;

    void Awake()
    {

        yarnVarRef = yarnVarRefObj.GetComponent<ExampleVariableStorage>();


        // Populate the Singleton with the followint if and else if statements
        if (S == null)
        {
            S = this;
        }
        else if (S != null)
        {
            Destroy(this);
        }
        //Make sure that the GameObject this is attached to is not deleted on load
        DontDestroyOnLoad(gameObject);

		//DESTROY IF MORE THAN ONE COPY EXISTS
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

    }

    // Use this for initialization
    void Start()
    {
        _cluesCollected = new List<ClueItem>();
        _clueInfoOfCollected = new List<ClueInfo>();
        _cluesToPresent = new List<ClueInfo>();

        // Set the LayerMask _clueLayer to the appropriate layer.
        _clueLayer = 1 << LayerMask.NameToLayer("Clue");

        //DELETE
        //clueEntry = GameObject.FindGameObjectWithTag ("ClueEntry");
    }

    public void PopulateYarnVars()
    {
        for (int i = 0; i < _clueInfoOfCollected.Count; i++)
        {
            //Checking if clueInfo we're loading has a tag of "Clue" (set in ClueItem), and making sure it has NOT been added to Yarn's list yet.
            if (_clueInfoOfCollected[i].isClue && !_clueInfoOfCollected[i].addedToYarn)
            {
                // Set the Clue Name in Yarn
                string yarnClueVariableName = "$Clue" + yarnIndex.ToString();
                string yarnClueFoundVariableName = yarnClueVariableName + "Found";
                // Add the Clue#Owner1 and Clue#Owner2 variables
                string yarnClueOwner1VariableName = yarnClueVariableName + "Owner1";
                string yarnClueOwner2VariableName = yarnClueVariableName + "Owner2";
                string yarnClueLocationVariableName = yarnClueVariableName + "Location";
                foreach (ExampleVariableStorage.DefaultVariable c in yarnVarRef.defaultVariables)
                {
                    //TODO make this a switch statement
                    Yarn.Value yarnVal;

                    // Checks for the yarn variable name (Clue#) and sets the appropriate value
                    if (c.name == yarnClueVariableName)
                        c.value = _clueInfoOfCollected[i].clueName;
                    
                        
                    // Checks for the yarn variable name related to Clue#Found and sets it to the appropriate value
                    if (c.name == yarnClueFoundVariableName)
                        c.value = "true";

                        
                    // Checks if the yarn variable name related to Clue#Owner1 and sets it to the appropriate value
                    if (c.name == yarnClueOwner1VariableName)
                        //c.value = "Cook";
                        c.value = _clueInfoOfCollected[i].clueOwner1;
                    
                   
                    // Checks for the yarn variable name related to Clue#Owner2 and sets it to the appropriate value
                    if (c.name == yarnClueOwner2VariableName)
                        c.value = _clueInfoOfCollected[i].clueOwner2;
                    
                        
                    // Checks for the yarn variable name related to Clue#Location and sets it to the appropriate value
                    if (c.name == yarnClueLocationVariableName)
                        c.value = _clueInfoOfCollected[i].location;

                    if (c.value == "true")
                        yarnVarRef.SetValue(c.name, new Yarn.Value(true));
                    else 
                        yarnVarRef.SetValue(c.name, new Yarn.Value(c.value));
                        
                    
                    
                }
               // Yarn.Value yarnClueValue = new Yarn.Value(_clueInfoOfCollected[i].clueName);
               // yarnClueValue.variableName = yarnClueVariableName;
                //yarnVarRef.SetValue(yarnClueValue.variableName, yarnClueValue);

                //// Set the ClueFound variable in Yarn
                //string yarnClueFound = yarnClueVariableName + "Found";
                //Yarn.Value yarnClueFoundValue = new Yarn.Value("true");
                //yarnClueValue.variableName = yarnClueFound;
                //yarnVarRef.SetValue(yarnClueFoundValue.variableName, yarnClueFoundValue);

                yarnIndex++;
                _clueInfoOfCollected[i] = new ClueInfo(_clueInfoOfCollected[i], true); // FUCK THIS LINE!!!!!!
                
            }
        }
    }

    [YarnCommand("AddClue")]
    public void AddToClueLog(string clueName)
    {
        //AddClue(ref ClueDatabase.S.GetClueInfo("clueName"));
    }

    //Adds ClueItem and its ClueInfo passed from ClueItemInspector's raycasted ClueItem
    public void AddClue(ref ClueItem hitClue)
    {
        //Check if this ClueItem has already been found
        if (!hitClue.isCollected)
        {

            hitClue.isCollected = true;

            // Objects are not staying in this list between scenes

            _cluesCollected.Add(hitClue);
            ClueItem objectHit = hitClue;
            ClueInfo objectInfo = new ClueInfo(ref hitClue);
            _clueInfoOfCollected.Add(objectInfo);

            print("Last collected clue: " + _clueInfoOfCollected[_clueInfoOfCollected.Count - 1].clueName);

            PopulateYarnVars();

            //Call for CanvasManager to enable "Gather Crew" buttons ONLY when enough clues are collected
            //TODO: Make sure this works even if cluesLeftToChoose changes. Might want to make a default int for the clues to get
            if (_cluesCollected.Count == AccusationManager.S.cluesLeftToChoose)
            {
                CanvasManager.S.enableGatherButtons();
            }
            //Creates a new button in the Clue Inventory UI
            if (UI_ButtonManager.S != null)
            {
                GameObject button = UI_ButtonManager.S.CreateButton(ref hitClue); ;
                Button_ClueList buttonInfo = button.AddComponent<Button_ClueList>();
                Button buttonScript = button.GetComponent<Button>();
                //Be careful, setup dependent.
                Image clueIcon = button.GetComponent<Image>();

                //Button creation

                //Add Components and parameters that are specific to Clue Item
                buttonInfo.clueInfo = objectInfo;
                buttonScript.onClick.AddListener(buttonInfo.SelectClue);
                clueIcon.sprite = Resources.Load("Sprites\\" + objectInfo.clueName, typeof(Sprite)) as Sprite;

            }
            //Gives the player a notification that the clue was added

            if (UI_ButtonManager.S.addedClueNotification.transform.parent.gameObject.activeSelf)
                UI_ButtonManager.S.addedClueNotif(true);
        }
        else if (UI_ButtonManager.S.addedClueNotification.transform.parent.gameObject.activeSelf)
            UI_ButtonManager.S.addedClueNotif(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (ClueItem c in _cluesCollected)
            {
                print("Item: " + c.name + ", ");
            }

            foreach (ClueInfo c in _clueInfoOfCollected)
            {
                print("Info: " + c.clueName);
            }
        }
    }

}
