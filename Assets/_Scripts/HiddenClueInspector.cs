using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.Utils;


/// <summary>
/// Handles inspecting clues hidden inside other objects.
/// </summary>
public class HiddenClueInspector : MonoBehaviour 
{
	public static HiddenClueInspector S;
	[SerializeField] DialogueUITest dialogueUI;
	[SerializeField] Canvas dialogueCanvas;

	
	[SerializeField] TextboxController textboxPrefab;

	// To be set by ClueHolders
	public ClueItem clueToInspect;

	// For displaying text without running a node
	GameObject textbox;
	TextboxController tbController;

	bool textboxIsThere
	{
		get { return textbox != null; }
	}
	string textToDisplay 
	{
		get
		{
			if (clueToInspect != null)
				return "You found a " + clueToInspect.name + "!";
			
			else 
				return "You found nothing in there.";
		}
	}

	void Awake()
	{
		if (S == null)
			S = this;
		else
			Destroy(this.gameObject);

		// Make sure this has the components it needs
		if (dialogueUI == null)
			throw new System.NullReferenceException(this.name + " needs a ref to the dialogue UI.");

		if (dialogueCanvas == null)
			throw new System.NullReferenceException(this.name + " needs a ref to the dialogue canvas.");
	}

	[YarnCommand("InspectObject")]
	public void InspectObject()
	{
		HandleDisplayingTextbox();
		// ^ This will pop up a message about whether there's a clue in the object. 
		// And if there is, the clue will be shown in the inspection window.
	}

	IEnumerator DisplayTextDelayed(float delay)
	{
		// The canvas manager isn't on a canvas, so I need this to wait a frame or two 
		// before the textbox can display text without an error popping up

		yield return new WaitForSeconds(delay);
		tbController.gameObject.SetActive(true);
		tbController.PlaceOnScreen(new Vector2(0.5f, 0.0f));
		// ^For some reason this doesn't work when called before the delay
		
		tbController.DisplayText(textToDisplay);
	}

	void HidePortrait()
	{
		Image portrait = tbController.portrait.GetComponent<Image>();
		portrait.color = new Color(0, 0, 0, 0);
	}

	void StopUsingDialogueCanvas()
	{
		dialogueUI.needsDialogueCanvas.Remove(this.gameObject);
	}

	void HandleDisplayingTextbox()
	{
		// Create and initialize the textbox
		Debug.Log(this.name + " displaying textbox.");
		GameController.S.RequestGamePause();
		textbox = 				Textbox.Create(textboxPrefab.gameObject);
		tbController = 			textbox.GetComponent<TextboxController>();

		tbController.ApplyTextSettings(dialogueUI.textSettings);

		// Make sure the textbox shows up
		dialogueUI.needsDialogueCanvas.Add(this.gameObject); 
		
		tbController.rectTransform.SetParent(dialogueCanvas.transform, false);
		
		tbController.nameTagText = "System";
		HidePortrait();

		PrepareForTextboxEnd();

		tbController.gameObject.SetActive(false);
		// ^ Need to do this to avoid a weird visual effect. Don't worry, the textbox gets reenabled
		// right before it's time for it to display the text.

		StartCoroutine(DisplayTextDelayed(Time.deltaTime * 2));
	}
	void PrepareForTextboxEnd()
	{
		// Need to do certain things when the textbox is done displaying text
		tbController.DoneDisplayingText.AddListener( () => Destroy(tbController.gameObject) );
		tbController.DoneDisplayingText.AddListener(StopUsingDialogueCanvas);
		tbController.DoneDisplayingText.AddListener( () => CameraController.S.SetPanning(false) );

		if (clueToInspect != null)
			tbController.DoneDisplayingText.AddListener(ShowClueInInspectionWindow);
	}

	void ShowClueInInspectionWindow()
	{
		ClueItemInspector.S.ShowClueInUI(clueToInspect.gameObject);
	}
	
}
