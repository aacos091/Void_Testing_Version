using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour {

	public static ResultsScreen S;
	protected Scene defaultScene;
	public GameObject ClueManagerGO;
	public GameObject LevelManagerGO;
	public GameObject AccusationManagerGO;
	public GameObject resultsScreenParentGO;
	public GameObject UIGO;
	public GameObject juryCanvas;
	public GameObject HUD_Canvas;
	public GameObject proceduralManagerGO;
	public GameObject juryButton;

	void Awake() {
		
		//Make sure that the GameObject this is attached to is not deleted on load
		DontDestroyOnLoad(gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(resultsScreenParentGO);
		}

		defaultScene = gameObject.scene;

	}

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayResultsScreen() {
		this.gameObject.SetActive (true);
	}

	public void ResetButton() {

		/*
		SceneManager.MoveGameObjectToScene (ClueManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (LevelManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (AccusationManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (UIGO, defaultScene);
		SceneManager.MoveGameObjectToScene (gameObject, defaultScene);
		*/

		SceneManager.LoadScene ("Final_Ship (TestimonyStuff)");
	}

	void OnLevelWasLoaded(){
		this.gameObject.SetActive (false);
		juryCanvas.SetActive (false);
		HUD_Canvas.SetActive (true);
		juryButton.SetActive (false);
	}
}
