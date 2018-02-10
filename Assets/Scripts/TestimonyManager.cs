using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;

public class TestimonyManager : MonoBehaviour 
{

	public VariableStorageBehaviour variableStorage;
	[SerializeField] List<VerbalClueEntry> entries = new List<VerbalClueEntry>();
	CrewManager crewManager;
	void Start()
	{
		crewManager = CrewManager.S;
	}

	[SerializeField] Transform testimonyHolder;
	[SerializeField] GameObject entryPrefab;

	[YarnCommand("AddEntry")]
	public void AddEntry(string text, string imageName)
	{
		Debug.Log("Adding testimony!");
		GameObject newEntryGo = Instantiate<GameObject>(entryPrefab, entryPrefab.transform.position, Quaternion.identity);
		VerbalClueEntry entry = newEntryGo.GetComponent<VerbalClueEntry>();

		// The text may contain a variable, so make sure that the right value is shown instead 
		// of the variable name
		string entryText = YarnUtils.ParseYarnText(variableStorage, text);

		// Load the sprite based on the name
		Sprite giverSprite = Resources.Load<Sprite>("Graphics/Portraits/" + imageName);

		if (giverSprite == null)
			throw new System.ArgumentException(imageName + " is not in the Portraits subfolder of the Graphics folder.");
		
		entry.text = entryText;
		entry.giverMugshot = giverSprite;

		newEntryGo.transform.SetParent(testimonyHolder, false);

		// Make it expand when clicked, go back to normal when clicked while expanded
		Button entryButton = newEntryGo.GetComponent<Button>();
		crewManager.buttonsManaged[newEntryGo] = ButtonState.normal;

		entryButton.onClick.AddListener( () => 
		{
			if (crewManager.EntryNormal(newEntryGo))
				crewManager.ExpandNews(newEntryGo);
			else 
				crewManager.ShrinkNews(newEntryGo);

		});


		entries.Add(entry);

	}
	
}
