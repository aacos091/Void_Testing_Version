using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager S;
    //Object that contains UI Canvases
    public GameObject uiParent;

    public GameObject captainsLogCanvas;
    public GameObject HUDCanvas;
    public GameObject dialogueCanvas;
    public GameObject juryDialogueCanvas;
    //TODO: TEMP until real dialogue system is finished/integrated
    public GameObject juryDialogueTextBox;
    public GameObject[] crewSprites;

    public GameObject[] gatherCrewButtons;
    //TODO: This is a pre-scripted solution to loading DontDestroyOnLoad canvases, and requires PLUGGING ALL CANVASES IN DONTDESTROYONLOAD IN TO THE INSPECTOR before use.
    //Sorta temp, depends on if we can find a dynamic way to find DontDestroyOnLoad objects.
    //public static List <GameObject> accusationCanvases;
    public GameObject[] accusationCanvases;


    //public GameObject targetCanvas;
    //public GameObject currentCanvas;
    private bool activated;

    //Need to access camera, disable its controls, and move it away from the game when UI is active

    private GameObject mainCam;
    private CameraController camController;

    //To position camera away from the scene when inspecting clues
    public Vector3 lastCamPos;
    public Vector3 camUIPos = new Vector3(0, 25, -25);
    public bool camSetAtUIPos = false;
    
    [SerializeField]
    private GameObject dialogueRunnerObj;
    DialogueRunner dialogueRunner;

    void Awake()
    {
        S = this;
        if (uiParent == null)
            uiParent = gameObject;

        camController = CameraController.S;
        //dialogueRunner = DialogueRunner.S;
        dialogueRunner = dialogueRunnerObj.GetComponent<DialogueRunner>();

        print("br");


        //TODO: Fix this from being super scripted and falsely hardcoded, need a solution to finding DontDestroyOnLoad objects
        //accusationCanvases = new List<GameObject>();
        //Make sure that the GameObject the UI is attached to is not deleted on load
        DontDestroyOnLoad(uiParent);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Sorta TEMP: reopens HUD when dialogue is done
        if (dialogueCanvas != null)
        {
            if (dialogueCanvas.activeSelf && !dialogueRunner.isDialogueRunning)
            {
                if (mainCam == null)
                    mainCam = Camera.main.gameObject;
                lastCamPos = mainCam.transform.position;

                // old canvas-disabling and enabling
                //disableCanvas(dialogueCanvas);
                //loadCanvas(HUDCanvas);
                DisableCanvas(dialogueCanvas.GetComponent<Canvas>());
                LoadCanvas(HUDCanvas.GetComponent<Canvas>() );

            }
        }
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

        currentCanvas.SetActive(false);

    }

    
    public void disableCanvas()
    {
        uiParent.SetActive(false);
    }

    public void DisableCanvas(Canvas canvas)
    {
        // Thought it'd be better to have a disable method that just changes the canvas' 
        // CanvasGroup component (I added one to the Dialogue and HUD canvases).

        CanvasGroup cGroup = canvas.GetComponent<CanvasGroup>();

        if (cGroup == null)
            throw new System.NullReferenceException(canvas.name + " does not have a Canvas Group component attached to it.");

        cGroup.alpha = 0;
        cGroup.blocksRaycasts = false;
        cGroup.interactable = false;
    }

    public void LoadCanvas(Canvas canvas)
    {
        // Like the other LoadCanvas, but changes the canvas' CanvasGroup component instead of just 
        // enabling it 

        CanvasGroup cGroup = canvas.GetComponent<CanvasGroup>();

        if (cGroup == null)
            throw new System.NullReferenceException(canvas.name + " does not have a Canvas Group component attached to it.");

        cGroup.alpha = 1;
        cGroup.blocksRaycasts = true;
        cGroup.interactable = true;

        //First, cancels out of any zoom coroutine
        if (camController == null)
            camController = CameraController.S;
        if (camController.currentCoroutine != null)
        {
            //StopCoroutine(camController.currentCoroutine);
        }

        //Return to normal game view
        if (canvas.gameObject == HUDCanvas)
        {
            if (GameController.S.gamePaused)
                GameController.S.RequestGameResume();
            //Alex Code
            Camera.main.GetComponent<CameraController>().enabled = true;
            Camera.main.GetComponent<Controls_Mobile>().enabled = true;
            Camera.main.GetComponent<Controls_PC>().enabled = true;
            MoveCamBack();
        }
        else
        {
            if (!GameController.S.gamePaused)
                GameController.S.RequestGamePause();
            Camera.main.GetComponent<CameraController>().enabled = false;
            Camera.main.GetComponent<Controls_Mobile>().enabled = false;
            Camera.main.GetComponent<Controls_PC>().enabled = false;
            if (canvas.gameObject != dialogueCanvas)
            {
                MoveCamAway();
            }
        }

        //		if (GameController.S.activeDrawer != null)
        //			GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        /*
        activated = targetCanvas.activeInHierarchy;
		targetCanvas.SetActive(!activated);
        */

        if (SceneManager.GetActiveScene().name != "Jury_Scene")
        {
            cGroup.gameObject.SetActive(true);
        }

    }

    // This function does the same as the above function but is supposed to activate the
    // target canvas They are named differently so that setting up games UI is easier.
    public void loadCanvas(GameObject targetCanvas)
    {
        //First, cancels out of any zoom coroutine
        if (camController == null)
            camController = CameraController.S;
        if (camController.currentCoroutine != null)
        {
            StopCoroutine(camController.currentCoroutine);
        }

        //Return to normal game view
        if (targetCanvas == HUDCanvas)
        {
            if (GameController.S.gamePaused)
                GameController.S.RequestGameResume();
            //Alex Code
            Camera.main.GetComponent<CameraController>().enabled = true;
            Camera.main.GetComponent<Controls_Mobile>().enabled = true;
            Camera.main.GetComponent<Controls_PC>().enabled = true;
            MoveCamBack();
        }
        else
        {
            if (!GameController.S.gamePaused)
                GameController.S.RequestGamePause();
            Camera.main.GetComponent<CameraController>().enabled = false;
            Camera.main.GetComponent<Controls_Mobile>().enabled = false;
            Camera.main.GetComponent<Controls_PC>().enabled = false;
            if (targetCanvas != dialogueCanvas)
            {
                MoveCamAway();
            }
        }

        //		if (GameController.S.activeDrawer != null)
        //			GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        /*
        activated = targetCanvas.activeInHierarchy;
		targetCanvas.SetActive(!activated);
        */

        if (SceneManager.GetActiveScene().name != "Jury_Scene")
        {
            targetCanvas.SetActive(true);
            
        }
            
    }

    

    //Version that works with a canvas name
    //Should set all Canvases in a single script later on
    //	[YarnCommand("loadCanvasByName")]
    //	public void loadCanvasByName(string canvasName)
    //	{
    //		//GameObject targetCanvas = GameObject.Find(canvasName);
    //
    //		if (GameController.S.activeDrawer != null)
    //			GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
    //	}

    public void MoveCamAway()
    {
        if (mainCam == null)
            mainCam = Camera.main.gameObject;
        lastCamPos = mainCam.transform.position;
        mainCam.transform.position = camUIPos;
        camSetAtUIPos = true;
    }
    public void MoveCamBack()
    {
        if (mainCam == null)
            mainCam = Camera.main.gameObject;
        mainCam.transform.position = lastCamPos;
        camSetAtUIPos = false;
    }

    public void enableGatherButtons()
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
        //        if (GameController.S.activeDrawer != null)
        //            GameController.S.activeDrawer.GetComponent<DrawerController>().ResetDrawer();
        foreach (GameObject canvas in accusationCanvases)
        {
            print(canvas.name);
            //activated = canvas.activeInHierarchy;
            canvas.SetActive(true);
        }
    }
}