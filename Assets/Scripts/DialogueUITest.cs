using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Yarn.Unity;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.TextboxSystem.Utils;
using TeaspoonTools.Utils;

public class DialogueUITest : DialogueUIBehaviour {

	[HideInInspector]
	public UnityEvent StartedDialogue 		= 		new UnityEvent();
	[HideInInspector]
	public UnityEvent EndedDialogue 		= 		new UnityEvent();

	StringBuilder textToShow = new StringBuilder(); //not including choices
	List<string> dialogueChoices = new List<string>();

	GameObject textbox = null;
	TextboxController textboxController = null;
	GameObject optionWindow;

	[SerializeField] GameObject textboxPrefab;
	[SerializeField] GameObject optionWindowPrefab;
	[SerializeField] Canvas dialogueCanvas;
    public TextSettings textSettings;
	
	bool readingDialogue = false;

	DialogueRunner dialogueRunner;
	Yarn.Options optionContainer;

	string nameTagText = "";
	Sprite portrait = null;

	/// A delegate (ie a function-stored-in-a-variable) that
	/// we call to tell the dialogue system about what option
	/// the user selected
	private Yarn.OptionChooser SetSelectedOption;

	void Awake()
	{
		dialogueRunner = 		FindObjectOfType<DialogueRunner> ();

		StartedDialogue = 		new UnityEvent ();
		EndedDialogue = 		new UnityEvent ();
		StartedDialogue.AddListener( () => Debug.Log(this.name + ": started displaying text!"));
		EndedDialogue.AddListener( () => Debug.Log(this.name + ": Dialogue ended!"));
	}

	public override IEnumerator DialogueStarted ()
	{
		StartedDialogue.Invoke ();
		return base.DialogueStarted ();
	}

	public override IEnumerator DialogueComplete ()
	{
        //Debug.Log("Ended dialogue!");
		EndedDialogue.Invoke ();
		return base.DialogueComplete ();
	}

	public override IEnumerator RunLine (Yarn.Line line)
	{
        //Debug.Log(this.name + ": Running a line!");
		
		// Add the lines to the text to show. Note that grouping up the 
		// lines like this is why I added a paused flag to DialogueRunner
		if (readingDialogue)
			textToShow.Append(line.text.Replace('\n', ' '));
		
		yield return null;
	}

	public override IEnumerator RunCommand (Yarn.Command command)
	{
		/*
		 * Running the custom commands made for the textbox system, like those that 
		 * define what is in a textbox.
		 */

		switch (command.text) 
		{
		case "Textbox":
			// clear the text to show so it can be read
			//Debug.Log("At start of a textbox!");
			readingDialogue = true;
			textToShow = new StringBuilder ();
			CreateTextbox();
			break;

		case "/Textbox":
			// its time to display the text read up to this point
			//Debug.Log(this.name + ": At end of a textbox!");
			dialogueRunner.paused = true;
			readingDialogue = false;
			GetTextDisplayed ();
			break;

		}

		string imageName;

        // The following can't be handled with a switch statement, so...
        bool textboxCommand = command.text.ToLower().Contains("textbox");
        bool nameCommand = command.text.ToLower().Contains("name|");
        bool portraitCommand = command.text.ToLower().Contains("portrait|");

		string commandText = command.text.ToLower();

        if (commandText.Contains ("name|")) 
		{
			// for reading in nametags
			if (textboxController.nameTag != null)
				nameTagText = command.text.Remove (0, "name|".Length);
			else
				throw new System.InvalidOperationException (this.name + ": Tried to set nametag text for a textbox with no name tag!");
		}

		else if (commandText.Contains ("portrait|")) 
		{
			// for choosing which portrait to show
			imageName = command.text.Remove(0, "portrait|".Length);

			if (textboxController.portrait != null)
				portrait = Resources.Load<Sprite> ("Graphics/Portraits/" + imageName);
			else
				throw new System.InvalidOperationException (this.name + ": Tried to set a portrait for a textbox with no portrait!");

		}

		else if (commandText.Contains("nametagmove|"))
		{
			// move the nametag to the left or right edge
			TextboxNametag nameTag;
			if (textboxController.nameTag != null)
				nameTag = textboxController.nameTag;
			else 
				throw new System.NullReferenceException("Yarn command sets nametag when there is no nametag on the textbox prefab.");

			string placeToMove = commandText.Substring("nametagmove|".Length);
			RectTransform rectTrans = textboxController.rectTransform;
			RectTransform tagRect = nameTag.rectTransform;
			float edgeX = 0;
			Vector2 prevPivot = tagRect.pivot;
			Vector2 newPivot = prevPivot;
			Vector2 newPos = tagRect.anchoredPosition;

			if (placeToMove == "rightedge")
			{
				
				edgeX = rectTrans.RightEdgeX();
				newPivot.x = 1;
				
			}
			else if (placeToMove == "leftedge")
			{
				edgeX = rectTrans.LeftEdgeX();
				newPivot.x = 0;
			}
			else
			{
				string errorMessage = "Invalid argument for changing nametag position: " + placeToMove;
				errorMessage += "\nPlease change it to LeftEdge or RightEdge.";
				throw new System.ArgumentException(errorMessage);
			}
				

			tagRect.pivot = newPivot;
			newPos.x = edgeX;
			tagRect.anchoredPosition = newPos;
			//tagRect.pivot = prevPivot;

		}

        bool someUnaccountedCommand = !textboxCommand && !nameCommand && !portraitCommand;

        if (someUnaccountedCommand)
            Debug.LogWarning("Unaccounted command: " + command.text);
			
		yield return null;
	}

	public override IEnumerator RunOptions (Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser)
	{
		// create the option dialog window, giving it one button for each option
		//Debug.Log("Running options!");
		optionWindow = CreateOptionWindow(optionsCollection);

		SetSelectedOption = optionChooser;

		// until an option is chosen, keep the dialogue runner effectively paused
		// (good thing we don't need to use its paused flag here, eh?)
		while (SetSelectedOption != null)
			yield return null;

		Destroy(optionWindow); // won't need this anymore!

		optionWindow = null;
		yield return null;
	}

	public override IEnumerator NodeComplete (string nextNode)
	{
		return base.NodeComplete (nextNode);
	}
    
	public void SetOption(int selectedOption)
	{
		// Call the delegate to tell the dialogue system that we've
		// selected an option.
		SetSelectedOption (selectedOption);

		// Now remove the delegate so that the loop in RunOptions will exit
		SetSelectedOption = null;
	}

	void GetTextDisplayed()
	{
		// make the textbox visible
		CanvasGroup cGroup = textbox.GetComponent<CanvasGroup>();
		cGroup.alpha = 1;
		cGroup.blocksRaycasts = true;
		
		//Debug.Log ("Text to display, gathered between the textbox tags: " + textToShow.ToString ());

		if (textboxController.nameTag != null)
			textboxController.nameTagText = nameTagText;
		if (textboxController.portrait != null)
			textboxController.portraitSprite = portrait;
		
		print("Right before displaying text, culprit is: " + dialogueRunner.variableStorage.GetValue("$Culprit").AsString);
		string varParsedText = YarnUtils.ParseYarnText(dialogueRunner.variableStorage, textToShow.ToString());
		
		textboxController.DisplayText ( varParsedText);
		
	}

	void ResumeDialogueRunning()
	{
		// signals to the dialogue runner to keep running dialogue for this module
		// to receive
		dialogueRunner.paused = false;
	}

	GameObject CreateOptionWindow(Yarn.Options optionsCollection)
	{
		GameObject optionWindow = 			Instantiate<GameObject> (optionWindowPrefab);
		OptionSetController setController = optionWindow.GetComponent<OptionSetController> ();
		setController.transform.SetParent (dialogueCanvas.transform, false);
		setController.Init (this);
		
		//setController.rectTransform.sizeDelta = new Vector2 ();
		GameObject newButton;

		// populate it with buttons
		string parsedOptString;
		foreach (var optionString in optionsCollection.options) 
		{
			parsedOptString = YarnUtils.ParseYarnText(dialogueRunner.variableStorage, optionString);
			newButton = Instantiate<GameObject>(setController.buttonPrefab);
			setController.AddOption (newButton, parsedOptString);
		}

		// let's put it in the middle-right of the screen
		//setController.rectTransform.ApplyAnchorPreset(TextAnchor.MiddleRight, false, false);
		setController.rectTransform.PositionRelativeToParent(TextAnchor.MiddleRight);

		return optionWindow;
	}

	void CreateTextbox()
	{
		if (textbox == null)
		{
			textbox 			= Textbox.Create (textboxPrefab);
			textboxController 	= textbox.GetComponent<TextboxController> ();
			textboxController.DoneDisplayingText.AddListener (ResumeDialogueRunning);
			EndedDialogue.AddListener (textboxController.Close);
			textbox.transform.SetParent (dialogueCanvas.transform, false);

			

			textboxController.PlaceOnScreen (new Vector2 (0.5f, 0.0f));
			if (textSettings.font != null)
				textboxController.font = textSettings.font;

			textSettings.Initialize(textboxController);
			textboxController.ApplyTextSettings(textSettings);

			

			// Have the textbox be invisible until it is time to show it
			CanvasGroup cGroup = textbox.AddComponent<CanvasGroup>();
			cGroup.alpha = 0;
			cGroup.blocksRaycasts = false;

		}
	}

}
