using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum buttonState
{
    normal,
    shrunk,
    expanded
}

public class UI_ButtonManager : MonoBehaviour
{
    //Singleton: there must only be one UI_ButtonManager in the scene.
    public static UI_ButtonManager S;

    public Text crewName;
    public Text crewDesc;

    public GameObject imgExpand;

    //public float maxSize;
    public float sizeChangeTime; 
    public float waitTime;

    RectTransform buttonRect;

    public Dictionary<GameObject, buttonState> buttonsManaged = new Dictionary<GameObject, buttonState>();

    public float expansionMultiplier = 2f;

    // For creating new Clue buttons and adjusting the size of the list display
    public GameObject buttonPrefab;
    public GameObject buttonGroup;
    private GridLayoutGroup buttonGroupLayout;
    public static List<GameObject> buttonsInGroup;
    public RectTransform scrollingContent;

    //Must be set in Inspector to use: a popup notification with FadingTextBox that activates when a clue is added
    public GameObject addedClueNotification;
    public GameObject addedClueToAccuseNotification;

    private void Start()
    {
        buttonsInGroup = new List<GameObject>();
    }

    // Use this for initialization
    void Awake()
    {
        S = this;
    }

    //Specific to ClueItems for now. Need to make an overload function for other buttons, such as crew.
    public GameObject CreateButton(ref ClueItem clue)
    {
        //Placement params
        RectTransform btnGroupRectTrans = buttonGroup.GetComponent<RectTransform>();
        GridLayoutGroup btnGroupGridLayout = buttonGroup.GetComponent<GridLayoutGroup>();

        //Create button from buttonPrefab
        GameObject button = Instantiate(buttonPrefab, buttonGroup.GetComponent<RectTransform>()) as GameObject;

        //Params and Components that have to be set outside the Prefab
        Button buttonScript;

        buttonScript = button.GetComponent<Button>();

        RectTransform btnRectTrans = button.GetComponent<RectTransform>();


        float rowsRaw =
            ((scrollingContent.rect.height - (btnGroupGridLayout.padding.top + btnGroupGridLayout.padding.top)) /
            (btnGroupGridLayout.cellSize.y + btnGroupGridLayout.spacing.y));
        if (rowsRaw > (Mathf.Ceil(buttonsInGroup.Count / btnGroupGridLayout.constraintCount)))
        {
            rowsRaw += Mathf.Ceil(buttonsInGroup.Count / btnGroupGridLayout.constraintCount);

        }
        int rowsAvailable = Mathf.FloorToInt(rowsRaw);
        //if (Mathf.Abs(btnRectTrans.anchoredPosition.y) >= scrollingContent.rect.height)

        print(rowsAvailable);
        print(buttonsInGroup.Count);
        print(rowsAvailable * btnGroupGridLayout.constraintCount);

        if (buttonsInGroup.Count + 1 > rowsAvailable * btnGroupGridLayout.constraintCount)
        {
            //Updates size of Content in UI
            scrollingContent.sizeDelta = new Vector2(0, scrollingContent.rect.height + btnGroupGridLayout.cellSize.y + btnGroupGridLayout.spacing.y);
        }
        //loadImage(clue.icon);

        //Currently not super secure, because it gets Text in ALL children. Needs to be changed, unless all buttons are set up the same way.
        button.GetComponentInChildren<Text>().text = clue.ItemName;

        buttonsInGroup.Add(button);
        return button;
    }

    //For reordering the clue button and visually flagging it as a clue for accusation
    public void AddButtonToAccusation(ClueInfo clueInfo)
    {
        foreach (GameObject button in buttonsInGroup)
        {
            if (button.GetComponent<Button_ClueList>().clueInfo.clueName == clueInfo.clueName)
            {
                button.GetComponent<Image>().color = new Color (1,.2f, 0);
                return;
            }
        }
    }

    public void RemoveButtonsFromAccusation()
    {
        foreach (GameObject button in buttonsInGroup)
        {
            button.GetComponent<Image>().color = Color.white;
        }
    }

    /*
    //Originally for testing the Anchored Position of new buttons
    IEnumerator Test(RectTransform rect)
    {
        yield return new WaitForSeconds(2f);
        Vector3 anch = rect.anchoredPosition;

        print("Anchored Button Position NOW:" + anch.y);

    }
    */

    public void addedClueNotif(bool addedClue)
    {
        Text notifText = addedClueNotification.GetComponentInChildren<Text>();

        addedClueNotification.SetActive(true);
        if (addedClue)
            notifText.text = ClueItemInspector.S.currentClue.ItemName + " ADDED TO CLUE LIST.";
        else
            notifText.text = " THIS ITEM IS ALREADY IN YOUR CLUE LIST.";
    }

    public void addedClueAccuseNotif(string stringToPrint)
    {
        Text notifText = addedClueToAccuseNotification.GetComponentInChildren<Text>();

        addedClueToAccuseNotification.SetActive(true);
        notifText.text = stringToPrint;
    }

    public void loadImage(Sprite icon)
    {
        imgExpand.GetComponent<Image>().sprite = icon;
    }

    public void loadName(Text names)
    {
        crewName.text = names.text;
    }

    public void expandNews(GameObject btn)
    {

        if (!buttonsManaged.ContainsKey(btn))
            // register this new button, and assume it's at a normal state
            buttonsManaged.Add(btn, buttonState.normal);

        if (buttonsManaged[btn] != buttonState.expanded)
            StartCoroutine(Expand(btn));
    }

    public void shrinkNews(GameObject btn)
    {
        if (!buttonsManaged.ContainsKey(btn))
            // register this new button, and assume it's at a normal state
            buttonsManaged.Add(btn, buttonState.normal);

        if (buttonsManaged[btn] != buttonState.shrunk)
            StartCoroutine(Shrink(btn));
    }

    IEnumerator Expand(GameObject button)
    {
        Debug.Log("Expanding button!");

        buttonRect = button.GetComponent<RectTransform>();
        float timer = 0;
        float originalHeight = buttonRect.rect.height;
        float targetHeight = originalHeight * expansionMultiplier;
        float currentHeight = originalHeight;

        float frameRate = 1f / Time.deltaTime;
        float framesUntilCompletion = frameRate * sizeChangeTime;

        Vector2 lerpedSize;

        while (currentHeight < targetHeight)
        {
            currentHeight = Mathf.Lerp(originalHeight, targetHeight, timer / framesUntilCompletion);
            lerpedSize = new Vector2(buttonRect.rect.width, currentHeight);
            buttonRect.sizeDelta = lerpedSize;
            timer++;
            yield return null;
        }

        buttonsManaged[button] = buttonState.expanded;

    }

    IEnumerator Shrink(GameObject button)
    {

        Debug.Log("Shrinking button");
        buttonRect = button.GetComponent<RectTransform>();
        float timer = 0;
        float originalHeight = buttonRect.rect.height;
        float targetHeight = originalHeight / expansionMultiplier;
        float currentHeight = originalHeight;

        float frameRate = 1f / Time.deltaTime;
        float framesUntilCompletion = frameRate * sizeChangeTime;

        Vector2 lerpedSize;

        while (currentHeight > targetHeight)
        {
            currentHeight = Mathf.Lerp(originalHeight, targetHeight, timer / framesUntilCompletion);
            lerpedSize = new Vector2(buttonRect.rect.width, currentHeight);
            buttonRect.sizeDelta = lerpedSize;
            timer++;
            yield return null;
        }

        buttonsManaged[button] = buttonState.shrunk;
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
