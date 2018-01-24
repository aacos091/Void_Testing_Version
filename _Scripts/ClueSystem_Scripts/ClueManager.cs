using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class ClueManager : MonoBehaviour {

	// List to hold all of the clues gathered so far
	public static List<ClueItem> _cluesCollected;
	public static ClueManager S;			// Singleton of this class;
	public static List<ClueInfo> _clueInfoOfCollected;
	public static GameObject[] _builtCluesMenu;
	public static Dictionary<string, int> _dictClueNameToIndex;	// Dictionary to store which index of _clueInfoOfCollected corresponds to each clueName;
	public static List<ClueInfo> _cluesToPresent;       // clues that will be presented during accusation
    public static ClueInfo currentClue; //The clue that is being investigated

	private int _cluesToSelect;
	private Text _cluesCollectedText;
	private LayerMask _clueLayer;
	private Image clueImage;
	public GameObject clueEntry;
	private static List<string> _nameOfCluesToPresent;  // name of the clues to present. Will be used to search corresponding clue information


    //[SerializeField]
    //private bool touchControlled;

    void Awake()
	{
        //Make sure that the GameObject this is attached to is not deleted on load
		DontDestroyOnLoad (gameObject);	
		// Populate the Singleton with the followint if and else if statements
		if (S == null)
		{
			S = this;
		}
		else if (S != null)
		{
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
		_cluesCollected = new List<ClueItem> ();
		_clueInfoOfCollected = new List<ClueInfo> ();
        _cluesToPresent = new List<ClueInfo>();

        // Set the LayerMask _clueLayer to the appropriate layer.
        _clueLayer = 1 << LayerMask.NameToLayer ("Clue");

		//DELETE
		//clueEntry = GameObject.FindGameObjectWithTag ("ClueEntry");
	}

    // Update is called once per frame
    /*
    void Update() {
    //Touch myTouch = Input.GetTouch(0);

    // Pressing the P key will display all of the clues gathered so far.
    if (
        //myTouch.tapCount == 2 || 
        Input.GetKeyDown(KeyCode.P)
        )
    {
        PrintCluesInInventory();
    }


    // Build Clues
    if (Input.GetKeyDown (KeyCode.B))
    {
        print ("Pressed B");
        BuildCollectedCluesMenu ();
    }
    //TODO Delete this test
    if (Input.GetKeyDown (KeyCode.A))
    {
        foreach (ClueInfo clue in _cluesToPresent)
        {
            print ("Name: " + clue.clueName +
            "\nDescription: " + clue.description +
            "\nRating: " + clue.rating);
        }
    }
}
    */
    // The following function can be deleted. It's here just to test the clues you've selected in the previous scene
    /*
    // The following function can be deleted. It's here just to test the clues you've selected in the previous scene
    public void PrintCluesInInventory ()
    {

        print ("Clues inside of _clueInfoOfCollected List: " + _clueInfoOfCollected.Count);
        // Set the properties for the Textbox you are going to populate in the scene
        _cluesCollectedText = GameObject.Find ("CluesCollected").GetComponent<Text> ();
        //_cluesCollectedText.color = Color.white;
        _cluesCollectedText.fontStyle = FontStyle.Bold;
        // Index used to display the number of each clue item in the list.
        int index = 1;
        _cluesCollectedText.text = "";
        // Cycle through the ClueInfo static list that is holding the information of all the clue's we have collected.
        foreach (ClueInfo clue in ClueManager._clueInfoOfCollected)
        {
            _cluesCollectedText.text +=  index + ") Name: " + clue.clueName + "\n"
                + "Description: " + clue.description + "\n"
                + "Rating: " + clue.rating + "\n\n";
            // Increase the index by 1 to be able to show the number of the clue. Ex: 1), 2), 3) etc
            index++;
        }

    }
    */

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

}



/*
// Function to build the clues we want to select from
public void BuildCollectedCluesMenu ()
{
    _dictClueNameToIndex = new Dictionary<string, int> ();
    // Create the list of _cluesToPresent in order to store the clues that the player will choose as the
    // 3 clues to use for the accusation
    _cluesToPresent = new List<ClueInfo> ();

    for (int i = 0; i < _clueInfoOfCollected.Count; i++)
    {
        // isActive is toggling on and off based on it's current value.
        bool isActive = !_builtCluesMenu[i].gameObject.activeInHierarchy;
        _builtCluesMenu [i].SetActive (isActive);
        // TODO Remove the image part of this comment: Each corresponding Image is named the same with "Image" appended to it. That way they are easier to find.
        // Example: The image for the Chair clue is named ChairImage
        string clueSpriteName = _clueInfoOfCollected [i].clueName;// + "Image";
        Sprite clueSprite = Resources.Load (clueSpriteName, typeof(Sprite)) as Sprite;
        // In the set up of the ClueEntry UI elements the Image is the 1st child element, the Text is the 2nd child element
        // and the button is the 3rd child element. That is why we pass the arguments 0 and 1 to the GetChild() method
        _builtCluesMenu [i].transform.GetChild(0).GetComponent<Image>().sprite = clueSprite;
        _builtCluesMenu [i].transform.GetChild(1).GetComponent<Text> ().text = "Name: " + _clueInfoOfCollected[i].clueName + "\nDescription: " +_clueInfoOfCollected [i].description;
        // TODO Make this line below clearer.
        // This line chooses the text element of the button that is the 3rd child element of our ClueEntry objects and sets the text. 
        _builtCluesMenu [i].transform.GetChild (2).GetComponent<Button> ().transform.GetChild (0).GetComponent<Text> ().text = "SELECT";
        // Set the text color, font size and fontStyle for the description
        _builtCluesMenu [i].transform.GetChild(1).GetComponent<Text> ().color = Color.white;
        _builtCluesMenu [i].transform.GetChild (1).GetComponent<Text> ().fontSize = 15;
        _builtCluesMenu [i].transform.GetChild (1).GetComponent<Text> ().fontStyle = FontStyle.Bold;

        // Add current clue name and corresponding index to the dictionary _dictClueNameToIndex
        _dictClueNameToIndex.Add (clueSpriteName, i);
    }

    // Turn off the _builtCluesMenu "entries that were not populated with data.
    foreach (GameObject entry in _builtCluesMenu)
    {
        // If the Button text has the default value of "Button" then it has not been populated so disable them
        if (entry.transform.GetChild(2).GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text == "Button")
        {
            print ("Text is Empty");
            entry.SetActive (false);
        }
    }

}
}
    */
