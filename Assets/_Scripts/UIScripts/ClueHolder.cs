﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.Utils;

public class ClueHolder : MonoBehaviour 
{
	public ClueItem clue = null;
	public GameObject textboxPrefab;
	public DialogueUITest dialogueUI;
	public Canvas dialogueCanvas;
	bool clicked = false;
	string textToDisplay
	{
		get 
		{
			if (clue == null)
				return "There is nothing in this " + this.name + ".";
			else 
				return "You found a " + clue.name + " in the " + this.name + ".";
		}
	}

	// Textbox
	GameObject textbox;
	TextboxController tbController;
	bool textboxIsThere = false;
	bool respondToInput
	{
		get 
		{ 	return 	CameraController.S.IsZoomed &&
					!GameController.S.gamePaused && 
				 	!textboxIsThere; 
		}
	}

	void Awake()
	{
		if (dialogueUI == null)
			throw new System.NullReferenceException(this.name + " needs a ref to the dialogue UI.");
		
		if (textboxPrefab == null)
			throw new System.NullReferenceException(this.name + " needs a textbox prefab.");

		if (dialogueCanvas == null)
			throw new System.NullReferenceException(this.name + " needs a ref to the dialogue canvas.");
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseUp()
	{
		if (respondToInput)
		{
			// Create and initialize the textbox
			Debug.Log(this.name + " displaying textbox.");
			GameController.S.RequestGamePause();
			textbox = 				Textbox.Create(textboxPrefab);
			tbController = 			textbox.GetComponent<TextboxController>();
			textboxIsThere = true;

			tbController.ApplyTextSettings(dialogueUI.textSettings);

			// Make sure the textbox shows up
			//**COMMENTING THIS OUT BECAUSE IT IS THROWING ERRORS
			//dialogueUI.needsDialogueCanvas.Add(this.gameObject); 
			
			tbController.rectTransform.SetParent(dialogueCanvas.transform, false);
			
			
			tbController.nameTagText = "System";
			HidePortrait();

			PrepareForTextboxEnd();

			tbController.gameObject.SetActive(false);
			// ^ Need to do this to avoid a weird visual effect. The textbox gets reenabled
			// right before it's time for it to display the text

			StartCoroutine(DisplayTextDelayed(Time.deltaTime * 2));
		}

	}

	void OnMouseDown()
	{
		if (respondToInput)
		{
			clicked = true;
			Debug.Log("Clicked " + this.name);
		}
	}

	void OnMouseExit()
	{
		if (respondToInput)
		{
			clicked = false;
			Debug.Log("Mouse exited " + this.name);
		}
	}

	void HidePortrait()
	{
		Image portrait = tbController.portrait.GetComponent<Image>();
		portrait.color = new Color(0, 0, 0, 0);
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

	void DoneNeedingDialogueCanvas()
	{
		//**COMMENTING THIS OUT BECAUSE IT IS THROWING ERRORS
		//dialogueUI.needsDialogueCanvas.Remove(this.gameObject);
	}

	void PrepareForTextboxEnd()
	{
		// Need to do certain things when the textbox is done displaying text
		tbController.DoneDisplayingText.AddListener( () => Destroy(tbController.gameObject) );
		tbController.DoneDisplayingText.AddListener(DoneNeedingDialogueCanvas);
		tbController.DoneDisplayingText.AddListener( () => textboxIsThere = false);

		//**COMMENTING THIS OUT BECAUSE IT IS THROWING ERRORS.
		//tbController.DoneDisplayingText.AddListener( () => CameraController.S.SetPanning(false) );

		if (clue != null)
			tbController.DoneDisplayingText.AddListener(ShowClueInInspectionWindow);
	}

	void ShowClueInInspectionWindow()
	{

		//**COMMENTING THIS OUT BECAUSE IT IS THROWING ERRORS.
		//ClueItemInspector.S.ShowClueInUI(clue.gameObject);
	}
	
}
