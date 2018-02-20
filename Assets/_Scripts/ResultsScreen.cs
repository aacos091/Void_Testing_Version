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
	public GameObject UIGO;

	void Awake() {
		// Populate the Singleton with the following if and else if statements
		if (S == null)
		{
			S = this;
		}
		else if (S != null)
		{
			Destroy(this);
		}
		//Make sure that the GameObject this is attached to is not deleted on load

		DontDestroyOnLoad(gameObject);

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
		SceneManager.MoveGameObjectToScene (ClueManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (LevelManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (AccusationManagerGO, defaultScene);
		SceneManager.MoveGameObjectToScene (UIGO, defaultScene);
		SceneManager.MoveGameObjectToScene (gameObject, defaultScene);

		SceneManager.LoadScene ("Final_Ship (TestimonyStuff)");
	}

	void OnLevelWasLoaded(){
		this.gameObject.SetActive (false);
	}
}
