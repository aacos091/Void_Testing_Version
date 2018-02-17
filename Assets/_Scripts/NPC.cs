using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

public class NPC : MonoBehaviour 
{
	[SerializeField] string _dialogueNode;
	[SerializeField] int _loyalty = 0;
	public string dialogueNode 
	{
		get { return _dialogueNode; }
		set { _dialogueNode = value; }
	}

	public int loyalty 
	{
		get { return _loyalty; }
		set { _loyalty = value; }
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

	[YarnCommand("SetLoyalty")]
	public void SetLoyalty (string amount)
	{
		switch (amount)
		{
			case "one":
				loyalty += 1;
				break;
			case "One":
				loyalty += 1;
				break;
			case "minusOne":
				loyalty -= 1;
				break;
			case "minusone":
				loyalty -= 1;
				break;
			default:
				break;
		}
	}
	
}
