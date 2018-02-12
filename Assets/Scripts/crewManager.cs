using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CrewManager : MonoBehaviour {

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


	void Awake()
	{
		// Need to set the singleton in awake, to make sure that other scripts can access them 
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
