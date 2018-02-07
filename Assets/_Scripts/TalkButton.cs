using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

public class TalkButton : MonoBehaviour 
{
	public static TalkButton S { get; protected set; }
	DialogueSystem dialogueSystem;
	DialogueRunner dialogueRunner;
	Button button;
	public Button.ButtonClickedEvent OnClick { get { return button.onClick; } }
	
	[SerializeField] string _dialogueNode;
	public string dialogueNode
	{
		get { return _dialogueNode; }
		set { _dialogueNode = value; }
	}

	void Awake()
	{
		button = GetComponent<Button>();
		S = this;
		OnClick.AddListener(MakeUnitTalk);

	}

	void Start()
	{
		dialogueSystem = DialogueSystem.S;
		dialogueRunner = dialogueSystem.dialogueRunner;
	}

	void MakeUnitTalk()
	{
		if (!string.IsNullOrEmpty(dialogueNode))
			dialogueRunner.StartDialogue(dialogueNode);
		else 
			Debug.LogWarning("Tried to make nc talk without a node to pass to yarnspinner.");
	}

	
}
