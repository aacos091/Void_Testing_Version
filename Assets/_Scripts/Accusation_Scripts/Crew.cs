using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crew : MonoBehaviour {

	//Canvas GameObjects for chatbox
	public GameObject TextBox;
	public GameObject Sprite;
	public GameObject DialogueText;

	//public string name;
	public int loyalty = 0;
	public bool alive = true;
	public bool culprit = false;

    void Awake()
    {
        TextBox = CanvasManager.S.juryDialogueTextBox;
        //TODO: Finds by name! Temp only?
        DialogueText = GameObject.Find(gameObject.name + "_Dialogue");
        foreach (GameObject spriteObj in CanvasManager.S.crewSprites)
        {
            if (spriteObj.name == (gameObject.name + "_Sprite"))
            {
                Sprite = spriteObj;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		DeadCheck ();
	}

	public void DeadCheck() {
		if (alive != true) {
			Destroy (gameObject);
		}
	}

	public void HideText() {
		DialogueText.SetActive (false);
	}

	public void DisplayText() {
		//if (AccusationManager.S.crewObject == gameObject) {
			Debug.LogWarning ("DisplayTextCalled");
			DialogueText.SetActive (true);
		//}
	}
}