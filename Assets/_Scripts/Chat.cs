using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class Chat : MonoBehaviour 
{
	public GameObject dialogueCanvas;
	public Text dialogue;
	public string unitName;

	[SerializeField] DialogueRunner dialogueRunner;
	[SerializeField] DialogueUITest dialogueUI;


	void Awake()
	{
		if (dialogueRunner == null || dialogueUI == null)
			throw new System.Exception(this.name + " needs its dialogueRunner and dialogueUI fields set in the inspector!");
	}
	
	public void StartChat(string nodeName)
	{
		/*
		Starts dialogue with an NPC, running the node with the passed nodeName.
		*/
		dialogueRunner.StartDialogue(nodeName);


	}
	public void startChat()
	{
		Debug.Log(this.name + " startChat()");
		dialogueCanvas.SetActive (true);
		//dialogue = dialogueCanvas.GetComponentInChildren<Text> ();
		dialogue.text = "Hello! I'm " + unitName + "!";
	}

	public void closeChat()
	{
		// Note from Gabriel: This function won't be necessary, since the textboxes close 
		// themselves once they're done displaying text
		Debug.Log("Closed chat!");
		dialogueCanvas.SetActive (false);
	}

}