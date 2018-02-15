using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TeaspoonTools.Utils;

public class CrewManager : MonoBehaviour 
{

	public Text crewName;
	public Text crewDesc;

	public GameObject imgExpand;

	//public float maxSize;
	public float sizeChangeTime;
	public float waitTime;

	RectTransform buttonRect;

	public Dictionary<GameObject, ButtonState> buttonsManaged = new Dictionary<GameObject, ButtonState> ();

	public float expansionMultiplier = 2f;

	public GameObject shrinkButton;

	public static CrewManager S;

	IEnumerator sizeChangeCoroutine = null;

	[SerializeField] float baseButtonHeight = 300;	

	void Awake()
	{
		// Need to set the singleton in Awake, to make sure that other scripts can access them 
		// after their Awake functions are done
		if (S == null)
			S = this;
		else
			Destroy (this);

	}

	// Use this for initialization
	void Start () {
		
	}

	public void loadImage(Sprite Crew) {
		imgExpand.GetComponent<Image> ().sprite = Crew;
	}

	public void loadName (Text names) {

		crewName.text = names.text;

	}

	public void loadText (Text info) {

		crewDesc.text = info.text;
	}

	public void expandNews (GameObject btn) {

		if (!buttonsManaged.ContainsKey (btn)) 
			// register this new button, and assume it's at a normal state
			buttonsManaged.Add(btn, ButtonState.normal);

		if (buttonsManaged[btn] != ButtonState.expanded && sizeChangeCoroutine == null)
		{
			sizeChangeCoroutine = Expand(btn);
			StartCoroutine (sizeChangeCoroutine);
		}
	}

	public void shrinkNews(GameObject btn)
	{
		if (!buttonsManaged.ContainsKey (btn)) 
			// register this new button, and assume it's at a normal state
			buttonsManaged.Add(btn, ButtonState.normal);
		
		if (buttonsManaged[btn] != ButtonState.normal && sizeChangeCoroutine == null)
		{
			sizeChangeCoroutine = Shrink(btn);
			StartCoroutine (sizeChangeCoroutine);
		}
	}

	
	IEnumerator Expand(GameObject button)
	{
		Debug.Log ("Expanding button!");

		buttonRect = button.GetComponent<RectTransform> ();
		float timer = 0;
		float originalHeight = buttonRect.rect.height;
		float targetHeight = originalHeight * expansionMultiplier;
		float currentHeight = originalHeight;

		float frameRate = 1f / Time.deltaTime;
		float framesUntilCompletion = frameRate * sizeChangeTime;

		Vector2 lerpedSize;

		while (currentHeight < targetHeight) 
		{
			currentHeight = Mathf.Lerp (originalHeight, targetHeight, timer / framesUntilCompletion);
			lerpedSize = new Vector2(buttonRect.rect.width, currentHeight);
			buttonRect.sizeDelta = lerpedSize;
			timer++;
			yield return null;
		}

		buttonsManaged [button] = ButtonState.expanded;
		sizeChangeCoroutine = null;
		//shrinkButton.SetActive (true);

	}

	IEnumerator Shrink(GameObject button)
	{

		Debug.Log ("Shrinking button");
		buttonRect = button.GetComponent<RectTransform> ();
		float timer = 0;
		float originalHeight = buttonRect.rect.height;
		float targetHeight = originalHeight / expansionMultiplier;
		float currentHeight = originalHeight;

		float frameRate = 1f / Time.deltaTime;
		float framesUntilCompletion = frameRate * sizeChangeTime;

		Vector2 lerpedSize;

		while (currentHeight > targetHeight) 
		{
			currentHeight = Mathf.Lerp (originalHeight, targetHeight, timer / framesUntilCompletion);
			lerpedSize = new Vector2(buttonRect.rect.width, currentHeight);
			buttonRect.sizeDelta = lerpedSize;
			timer++;
			yield return null;
		}

		buttonsManaged [button] = ButtonState.normal;
		sizeChangeCoroutine = null;
		
		//shrinkButton.SetActive(false);

	}

	public void ExpandTestimony(TestimonyEntry entry)
	{
		if (!buttonsManaged.ContainsKey (entry.gameObject)) 
			// register this new button, and assume it's at a normal state
			buttonsManaged.Add(entry.gameObject, ButtonState.normal);

		if (buttonsManaged[entry.gameObject] != ButtonState.expanded && sizeChangeCoroutine == null)
		{
			sizeChangeCoroutine = ExpandTestimonyCoroutine(entry);
			StartCoroutine (sizeChangeCoroutine);
		}
	}

	public void ShrinkTestimony(TestimonyEntry entry)
	{
		if (!buttonsManaged.ContainsKey (entry.gameObject)) 
			// register this new button, and assume it's at a normal state
			buttonsManaged.Add(entry.gameObject, ButtonState.normal);

		if (buttonsManaged[entry.gameObject] != ButtonState.normal && sizeChangeCoroutine == null)
		{
			sizeChangeCoroutine = ShrinkTestimonyCoroutine(entry);
			StartCoroutine (sizeChangeCoroutine);
		}
	}

	/// <summary>
	/// Expands the button so that it's just slightly more than tall enough to show its 
	/// text contents.
	/// </summary>
	IEnumerator ExpandTestimonyCoroutine(TestimonyEntry entry)
	{
		// Get some values needed to calculate whether this entry needs to be expanded
		//Debug.Log("Expanding testimony!");
		RectTransform entryRect = 	entry.rectTransform;

		Text textField = 			entry.textField;
		RectTransform textRect = 	textField.rectTransform;
		float textHeight = 			textRect.rect.height;

		bool needExpansion = 		textHeight >= baseButtonHeight;

		if (!needExpansion)
		{
			Debug.Log("This testimony doesn't need expanding.");
			sizeChangeCoroutine = 	null;
			yield break;
		}

		Debug.Log("This testimony DOES need expanding.");

		// Calculate the height the entry needs to expand to
		float extraSpace = 			baseButtonHeight * 0.1f;
		float expandedHeight = 		textHeight + extraSpace;
		Vector2 newSize = 			entryRect.sizeDelta;

		// And expand it
		
		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float timeToExpand = frameRate * sizeChangeTime;

		while (timer < timeToExpand)
		{
			newSize.y = 			Mathf.Lerp(newSize.y, expandedHeight, timer / timeToExpand);
			entryRect.sizeDelta = 	newSize;
			//Debug.Log("New testimony height: " + entryRect.sizeDelta.y);
			timer++;
			yield return null;
		}

		// Just to make sure
		newSize.y = 				expandedHeight;
		entryRect.sizeDelta = 		newSize;

		// avoid a visual bug
		textRect.ApplyAnchorPreset(TextAnchor.UpperCenter, true, true);

		buttonsManaged[entry.gameObject] = 	ButtonState.expanded;
		sizeChangeCoroutine = 		null;
		Debug.Log("Done expanding! The button state is now: " + buttonsManaged[entry.gameObject]);

	}

	/// <summary>
	/// Shrinks the button so that its height becomes the base height.
	/// </summary>
	IEnumerator ShrinkTestimonyCoroutine(TestimonyEntry entry)
	{
		// Some values we'll need
		Debug.Log("Shrinking testimony!");
		RectTransform entryRect = 		entry.rectTransform;
		float entryHeight = 			entryRect.rect.height;
		Text textField = 				entry.textField;
		RectTransform textRect = 		textField.rectTransform;
		float textHeight = 				textRect.rect.height;

		bool needShrink = 				entryHeight > baseButtonHeight;

		if (!needShrink)
		{
			Debug.Log("This testimony doesn't need shrinking.");
			sizeChangeCoroutine = 		null;
			yield break;
		}

		Debug.Log("This testimony DOES need shrinking.");

		Vector2 newSize = 				entryRect.sizeDelta;

		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float timeToShrink = frameRate * sizeChangeTime;

		while (timer < timeToShrink)
		{
			newSize.y = 				Mathf.Lerp(newSize.y, baseButtonHeight, timer / timeToShrink);
			entryRect.sizeDelta = 		newSize;
			timer++;
			//Debug.Log("New testimony height: " + entryRect.sizeDelta.y);
			yield return null;
		}

		// Just to make sure
		newSize.y = 					baseButtonHeight;
		textRect.sizeDelta = 			newSize;

		// avoid a visual bug
		textRect.ApplyAnchorPreset(TextAnchor.UpperCenter, true, true);

		buttonsManaged[entry.gameObject] = 	ButtonState.normal;
		sizeChangeCoroutine = null;

		Debug.Log("Done shrinking! The button state is now: " + buttonsManaged[entry.gameObject]);

	}
	public bool EntryExpanded(GameObject entry)
	{
		if (buttonsManaged.ContainsKey(entry))
			return buttonsManaged[entry] == ButtonState.expanded;

		return false;
	}

	public bool EntryNormal(GameObject entry)
	{
		if (buttonsManaged.ContainsKey(entry))
			return buttonsManaged[entry] == ButtonState.normal;

		return false;
	}
	/*
	IEnumerator Scale(GameObject O) {

		buttonRect = O.GetComponent<RectTransform> ();
		float timer = 0;
		bool yes = true;

		float frameRate = 1f / Time.deltaTime;

		float framesUntilCompletion = frameRate * growFactor;

		Vector2 lerpedSize;
		float originalHeight = buttonRect.rect.height;
		float targetHeight = originalHeight * 2f;

		while (yes == true) {

			while (maxSize > height) 
			{
				// raise the height a bit each frame until it reaches maxHeight

				height = buttonRect.rect.height;
				//timer += Time.deltaTime;
				lerpedSize = new Vector2(buttonRect.rect.width, Mathf.Lerp(originalHeight, targetHeight, timer / framesUntilCompletion));
				buttonRect.sizeDelta = lerpedSize;
				timer++;
				yield return null;
			}

			yield return new WaitForSeconds (waitTime);

			timer = 0;
			while (300 < height) 
			{
				height = buttonRect.rect.height;
				//timer += Time.deltaTime;
				lerpedSize = new Vector2(buttonRect.rect.width, Mathf.Lerp(targetHeight, originalHeight, timer / framesUntilCompletion));
				buttonRect.sizeDelta = lerpedSize;
				yes = false;
				yield return null;
			}

			//timer = 0;
			yield return new WaitForSeconds (waitTime);
		}
	}
	*/
}
