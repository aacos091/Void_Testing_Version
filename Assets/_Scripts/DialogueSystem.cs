using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;
using System.Text;

public class DialogueSystem : MonoBehaviour 
{
	public static DialogueSystem S { get; protected set; }
	
	VariableStorageBehaviour _variableStorage;
	DialogueRunner _dialogueRunner;
	DialogueUITest _dialogueUI;

	public VariableStorageBehaviour variableStorage 
	{ 
		get { return _variableStorage; } 
		protected set { _variableStorage = value; }
	}
	public DialogueRunner dialogueRunner 
	{ 
		get { return _dialogueRunner; } 
		protected set { _dialogueRunner = value; }	
	}
	public DialogueUITest dialogueUI 
	{ 
		get { return _dialogueUI; } 
		protected set { _dialogueUI = value; }
	}

	void Awake()
	{
		S = this;
		
		GetDialogueComponents();

		if (variableStorage == null || dialogueRunner == null || dialogueUI == null)
		{
			StringBuilder message = new StringBuilder();
			message.Append(this.name + " needs a variale storage, a dialogue runner, and a  ");
			message.AppendFormat("dialogue UI test component on its game object!" );
		}

	}

	void GetDialogueComponents()
	{
		variableStorage = 		GetComponent<VariableStorageBehaviour>();
		dialogueRunner = 		GetComponent<DialogueRunner>();
		dialogueUI = 			GetComponent<DialogueUITest>();
	}
	
	
}
