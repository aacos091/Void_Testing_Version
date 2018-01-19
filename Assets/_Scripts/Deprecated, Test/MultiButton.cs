using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiButton : MonoBehaviour {

    //TODO: VERY TEMP!
    public GameObject clueButton;
    public GameObject juryButton;

    // Use this for initialization
    void Start () {
		
	}
	
	public void ChangeButton (string sceneName) {

        //TODO: Change the name to "Clue_Scene" once all connections are fixed
        if (sceneName == "Clue_Scene_Will_T")
        {
            juryButton.SetActive(false);
            clueButton.SetActive(true);
        }
        else if (sceneName == "Jury_Scene")
        {
            clueButton.SetActive(false);
            juryButton.SetActive(true);
        }
	}
}
