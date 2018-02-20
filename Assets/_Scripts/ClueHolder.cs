using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.TextboxSystem;

public class ClueHolder : MonoBehaviour 
{
	[Header("Set dynamically")]
	public ClueItem clue;

	[Header("Set in the inspector")]
	[SerializeField] Canvas dialogueCanvas;
	[SerializeField] DialogueUITest dialogueUI;
	[SerializeField] TextboxController textboxPrefab;
	TextboxController textbox;
	bool textboxIsThere = false;
	string message;

	bool isZoomed
	{
		get { return CameraController.S.IsZoomed; }
	}

	void OnMouseDown()
	{
		Debug.Log("Mouse was down on " + this.name + "!");
		if (!textboxIsThere && isZoomed)
		{
			if (clue == null)
			{
				message = "There is nothing in this " + this.name + ".";
				SetupTextbox();
			}
			else
			{
				message = "You found a " + clue.name + " in this " + this.name;
				SetupTextbox();
			}

			textbox.DisplayText(message);
		}
	}

	void SetupTextbox()
	{

		GameObject tbObject = Textbox.Create(	textboxPrefab.gameObject, 
												dialogueUI.textSettings.linesPerTextbox, 
												dialogueUI.textSettings.textSpeed);

		textbox = tbObject.GetComponent<TextboxController>();

		CanvasManager.S.loadCanvas(dialogueCanvas.gameObject);
		CanvasManager.S.dialogueCanvasNeeded = true;
		Canvas.ForceUpdateCanvases();
		textbox.rectTransform.SetParent(dialogueCanvas.transform, false);
		
		// Put the textbox on the bottom center of the screen
		textbox.PlaceOnScreen(new Vector2(0.5f, 0));
		textbox.ApplyTextSettings(dialogueUI.textSettings);

		// Make it have no portrait, and let the user know its the system talking
		textbox.portrait.gameObject.SetActive(false);
		textbox.nameTagText = "System";

		// Need the textbox to go away when it's done displaying text
		textbox.DoneDisplayingText.AddListener( () => Destroy(textbox.gameObject) );
		textbox.DoneDisplayingText.AddListener( () => CanvasManager.S.dialogueCanvasNeeded = false);
		textboxIsThere = false;
	}

	
	
}
