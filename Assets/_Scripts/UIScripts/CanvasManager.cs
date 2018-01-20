using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager S;
    //Object that contains UI Canvases
    public GameObject uiParent;

    //public static List<GameObject> canvases;
    public GameObject[] canvases;

    public GameObject dialogueCanvas;
    //TODO: TEMP until real dialogue system is finished/integrated
    public GameObject dialogueTextBox;
    public GameObject[] crewSprites;

    public GameObject[] gatherCrewButtons;
    //TODO: This is a pre-scripted solution to loading DontDestroyOnLoad canvases, and requires PLUGGING ALL CANVASES IN DONTDESTROYONLOAD IN TO THE INSPECTOR before use.
    //Sorta temp, depends on if we can find a dynamic way to find DontDestroyOnLoad objects.
    //public static List <GameObject> accusationCanvases;
    public GameObject[] accusationCanvases;


    //public GameObject targetCanvas;
    //public GameObject currentCanvas;
	private bool activated;
	// Use this for initialization
	void Start () {

	}

    void Awake()
    {
        S = this;
        if (uiParent == null)
            uiParent = gameObject;
        /*
        GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("Canvas");
        for (int i = 0; i < canvasObjects.Length; i++)
            {
                print (canvasObjects[i].name;
                canvases.Add(canvasObjects[i]);
            }
        */

        //TODO: Fix this from being super scripted and falsely hardcoded, need a solution to finding DontDestroyOnLoad objects
        //accusationCanvases = new List<GameObject>();

        //Make sure that the GameObject the UI is attached to is not deleted on load
        DontDestroyOnLoad(uiParent);
    }

    // This function sets the activated bool variable to true if
    // the canvas is currently active in the scene and sets it to false
    // if the canvas is currently inactive
    public void disableCanvas(GameObject currentCanvas)
	{
        /*
		activated = currentCanvas.activeInHierarchy;
		currentCanvas.SetActive(!activated);
        */

		currentCanvas.SetActive (false);

    }

    // This function does the same as the above function but is supposed to activate the
    // target canvas They are named differently so that setting up games UI is easier.
    public void loadCanvas(GameObject targetCanvas)
	{
        if (GameController.S.activeDrawer != null)
            GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        /*
        activated = targetCanvas.activeInHierarchy;
		targetCanvas.SetActive(!activated);
        */
		if (SceneManager.GetActiveScene().name != "Jury_Scene")
			targetCanvas.SetActive (true);
    }

    public void EnableGatherButtons()
    {
        for (int i = 0; i < gatherCrewButtons.Length; i++)
        {
            if (gatherCrewButtons[i] != null)
                gatherCrewButtons[i].SetActive(true);
        }
    }

    //TODO: Fix this from being super scripted and falsely hardcoded, need a solution to finding DontDestroyOnLoad objects
    public void loadAccusationCanvases()
    {
        if (GameController.S.activeDrawer != null)
            GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        foreach (GameObject canvas in accusationCanvases)
            {
            print(canvas.name);
            //activated = canvas.activeInHierarchy;
            canvas.SetActive(true);
            }
    }

	public void disableCanvas()
	{
		uiParent.SetActive (false);

	}

//Version that works with a canvas name
//Should set all Canvases in a single script later on
[YarnCommand("loadCanvasByName")]
    public void loadCanvasByName(string canvasName)
    {
        //GameObject targetCanvas = GameObject.Find(canvasName);

        if (GameController.S.activeDrawer != null)
            GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        for (int i = 0; i < S.canvases.Length; i++)
        {
            print(S.canvases[i].name);

            if (S.canvases[i].name == canvasName)
            { 
            //print("It Worked?");
            //activated = targetCanvas.activeInHierarchy;
            S.canvases[i].SetActive(true);
            }
        }
    }


}