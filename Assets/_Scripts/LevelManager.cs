using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {


    [SerializeField]
    private GameObject Canvas_CaptainsLog;
    //TODO:TEMP-ish
    //public MultiButton[] multiButtons;

    void Awake ()
	{
		DontDestroyOnLoad (this);

		//DESTROY IF MORE THAN ONE COPY EXISTS
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	void Start ()
	{
		SceneManager.activeSceneChanged += ActivateUIElements;
	}

	public void ChangeToScene (string sceneName) {
        //TODO: This is a temp command to destroy any Inspection clones that are still up
		if (CanvasClueObject.S != null) {
			if (CanvasClueObject.S.cloneClue != null)
				Destroy (CanvasClueObject.S.cloneClue);
		}
		SceneManager.LoadScene(sceneName);
        if (sceneName == "Jury_Scene_Test")
        {
            if (Canvas_CaptainsLog != null)
            {
                Canvas_CaptainsLog.SetActive(true);
                Canvas_CaptainsLog.SetActive(false);
            }

        }

    }

	void ActivateUIElements (Scene previousScene, Scene newScene)
	{
		if (newScene.name == "Jury_Scene_Test")
		{
			print ("Scene Changed");
			print ("I'm serious, it changed");
			// Collect all of the UI elements that will hold "entries" of the clues you have found
			ClueManager._builtCluesMenu = GameObject.FindGameObjectsWithTag ("ClueEntry");
			// Activate each ClueEntry Object of the menu
			for (int i = 0; i < ClueManager._builtCluesMenu.Length; i++)
			{
				ClueManager._builtCluesMenu [i].SetActive (false);
			}
		}
        /*
        for (int i = 0; i < multiButtons.Length; i++)
        {
            if (multiButtons[i] != null)
            {
                multiButtons[i].ChangeButton(newScene.name);
            }
        }
        */
	}
}
