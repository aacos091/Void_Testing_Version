using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUnitManager : MonoBehaviour {
    //Singleton of this class
    public static DialogueUnitManager S;

    public GameObject currentDialogueUnit;
    private Animator currentAnimator;

    public float turnInterval = .02f;

	// Use this for initialization
	void Start () {
        S = this;
	}

    public void SetDialogueUnit(GameObject unitGO)
    {
        currentDialogueUnit = unitGO;
        currentAnimator = currentDialogueUnit.GetComponent<AnimationController>().animator;
    }

    public void StartTalking()
    {
        currentAnimator.SetBool("Talking", true);
        StartCoroutine(TurnToCamera());
    }

    public void StopTalking()
    {
        currentAnimator.SetBool("Talking", false);
    }

    public IEnumerator TurnToCamera()
    {
        currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, .5f);

        yield return new WaitForSeconds(turnInterval);

    }
}
