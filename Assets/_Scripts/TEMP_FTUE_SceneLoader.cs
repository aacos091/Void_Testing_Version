using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMP_FTUE_SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


    public void LoadSceneOnlyTest(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
