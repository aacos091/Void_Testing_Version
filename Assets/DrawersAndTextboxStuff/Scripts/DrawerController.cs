using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DrawerController : MonoBehaviour 
{
	public Vector3 slideMovement;

	public float slideTime = 2f;
    [SerializeField]
    bool isSlidedOut = false;
	static bool sliding = false;

	Vector3 originalPos;

	public string textStartNode;
	GameController gameController;

	static int drawersSlidableAtOnce = 1;
	static int drawersSlided = 0;

    //Temp until choice boxes are integrated properly with Yarn
    //Many behaviors are set to work or trigger differently if the drawer contains a clue, so that closing the drawer is UI-based instead.
    public GameObject inspectChoiceCanvas;
    public GameObject clueInDrawer;

	// Use this for initialization
	void Awake () 
	{
		originalPos = transform.position;	
	}

	void Start()
	{
		gameController = GameController.S;
		DialogueUITest dialogueUI = DialogueRunner.S.dialogueUI as DialogueUITest;
        if (clueInDrawer == null)
		dialogueUI.EndedDialogue.AddListener (this.SlideIn);
		//dialogueUI.EndedDialogue.AddListener (() => DrawerController.sliding = false);
		////Debug.Log ("SlideIn listener added!");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) && !ClueItemInspector.S.inspectingItem) 
		{
			////Debug.Log ("Left clicked!");
			if (!isSlidedOut)
				SlideOut ();
            /*
			else if (clueInDrawer == null)
				SlideIn ();
                */
		}
	}

	public void SlideOut()
	{
        if (!sliding && !gameController.gamePaused && drawersSlided < drawersSlidableAtOnce) 
		{

            gameController.activeDrawer = gameObject;
            //////////TEMP--------------TEMP///////////
            ///REMOVE WHEN PAUSE IS IMPLEMENTED////
            gameController.RequestGamePause ();
			if (!isSlidedOut)
				StartCoroutine (SlideOutCoroutine ());
		}
	}

	public void SlideIn()
	{
        if (
            //!gameController.gamePaused && 
            !sliding && this.isSlidedOut)
			StartCoroutine (SlideInCoroutine ());
	}

    public void ResetDrawer()
    {
        if (isSlidedOut)
        {
            gameController.RequestGameResume();
            sliding = false;
            isSlidedOut = false;
            transform.position = originalPos;
            gameController.activeDrawer = null;
            drawersSlided--;
        }
    }

    IEnumerator SlideInCoroutine()
	{
        //Debug.Log ("Sliding in!");

        sliding = true;
		float frameRate = 1f / Time.smoothDeltaTime;
		float timer = 0;
		float framesToPass = frameRate * slideTime;

		Vector3 targetPos = transform.localPosition - slideMovement;

		while (timer < framesToPass) 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, timer / framesToPass);
			timer++;
			yield return null;
		}

		transform.localPosition = targetPos;
		isSlidedOut = false;
        //////////TEMP--------------TEMP///////////
        ///REMOVE WHEN PAUSE IS IMPLEMENTED////
        gameController.RequestGameResume();
        gameController.activeDrawer = null;

        sliding = false;
		drawersSlided--;

        yield return null;
	}

	IEnumerator SlideOutCoroutine()
	{
        //Debug.Log ("Sliding out!");

        this.isSlidedOut = true;
        sliding = true;
		float frameRate = 1f / Time.smoothDeltaTime;
		float timer = 0;
		float framesToPass = frameRate * slideTime;

		Vector3 targetPos = transform.localPosition + slideMovement;

		while (timer < framesToPass) 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, timer / framesToPass);
			timer++;
			yield return null;
		}
		RunDialogue ();
		transform.localPosition = targetPos;
        drawersSlided++;
        sliding = false;

        yield return null;
	}

	void RunDialogue()
	{
		
		DialogueRunner.S.StartDialogue (textStartNode);
	}

    [YarnCommand("InspectChoice")]
    public void InspectChoice()
    {
        StartCoroutine(ChoicePopup());      
    }
    IEnumerator ChoicePopup()
    {
        yield return new WaitForSeconds(.5f);

        if (!inspectChoiceCanvas.activeSelf)
        {
            inspectChoiceCanvas.SetActive(true);
        }
    }

    public void InspectDrawerClue()
    {
        if (clueInDrawer != null)
            ClueItemInspector.S.SetCurrentClue(clueInDrawer);
    }

}
