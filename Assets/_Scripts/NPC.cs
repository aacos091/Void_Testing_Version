using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPC : MonoBehaviour 
{
	[SerializeField] string _dialogueNode;
	public string dialogueNode 
	{
		get { return _dialogueNode; }
		set { _dialogueNode = value; }
	}

	DialogueSystem dialogueSystem;

	// Use this for initialization
	void Start () 
	{
		dialogueSystem = DialogueSystem.S;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
}
