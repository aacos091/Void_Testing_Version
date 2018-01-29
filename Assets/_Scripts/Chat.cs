using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour 
{
	public GameObject dialogueCanvas;
	public Text dialogue;
	public string unitName;


	public void startChat(){
		dialogueCanvas.SetActive (true);
		//dialogue = dialogueCanvas.GetComponentInChildren<Text> ();
		dialogue.text = "Hello! I'm " + unitName + "!";
	}

	public void closeChat(){
		dialogueCanvas.SetActive (false);
	}

}