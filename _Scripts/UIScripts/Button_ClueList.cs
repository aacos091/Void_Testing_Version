using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ClueList : MonoBehaviour
{
    //ClueInfo struct stored on this button, needs to be worked out as a reference
    public ClueInfo clueInfo;
    private ClueItemInspector clueItemInspector;
    private ClueManager clueManager;

    public Text nameText;
    public Text descriptionText;
    public Image clueImageDisplay;

    public Sprite clueIconSprite;
    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        clueItemInspector = ClueItemInspector.S;
        clueManager = ClueManager.S;
        nameText = clueItemInspector.clueListInspectorName;
		descriptionText = clueItemInspector.clueListInspectorDescription;
        clueImageDisplay = clueItemInspector.clueImageDisplay;
        clueIconSprite = Resources.Load("Sprites\\" + clueInfo.clueName, typeof(Sprite)) as Sprite;
    }

    // Update is called once per frame
    public void SelectClue()
    {
        //Sets the ONLY currentClue in ClueManager Singleton
        ClueManager.currentClue = clueInfo;

        if (nameText != null)
        {
            nameText.text = clueInfo.clueName;
        }
        if (descriptionText != null)
        {
            descriptionText.text = clueInfo.description;
        }
        if (clueImageDisplay != null)
        {
            //Ray's example
            //testImage.sprite = Resources.Load("Sprites\\Sink", typeof(Sprite)) as Sprite;

            clueImageDisplay.sprite = Resources.Load("Sprites\\" + clueInfo.clueName, typeof(Sprite)) as Sprite;
            print(clueInfo.clueName);

        }
    }
}
