using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;

public class VerbalClueManager : MonoBehaviour 
{
	public VariableStorageBehaviour variableStorage;

	void Start()
	{
		
	}

	[SerializeField] Transform testimonyHolder;
	[SerializeField] GameObject entryPrefab;

	[YarnCommand("AddEntry")]
	public void AddEntry(string text, string imageName)
	{
		Debug.Log("Adding verbal clue!");
		GameObject newEntryGo = Instantiate<GameObject>(entryPrefab);
		TestimonyEntry entryDetails = newEntryGo.GetComponent<TestimonyEntry>();

		// The text may contain a variable, so make sure that the right value is shown instead 
		// of the variable name
		string entryText = YarnUtils.ParseYarnText(variableStorage, text);

		// load the sprite based on the name
		Sprite giverSprite = Resources.Load<Sprite>("Graphics/Portraits/" + imageName);

		if (giverSprite == null)
			throw new System.ArgumentException(imageName + " is not in the Portraits subfolder of the Graphics folder.");
		
		entryDetails.text = entryText;
		entryDetails.giverMugshot = giverSprite;

		newEntryGo.transform.SetParent(testimonyHolder, false);

	}
	
}
