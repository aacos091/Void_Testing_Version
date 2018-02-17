﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUnitManager : MonoBehaviour {
    //Singleton of this class
    public static DialogueUnitManager S;

    public GameObject currentDialogueUnit;
    private Animator currentAnimator;

    public float turnInterval = .02f;
    public Coroutine currentCoroutine;

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
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    public IEnumerator TurnToCamera()
    {
        /*
        {
            bool rotated = false;
            float rotAmt = 2;

            if (currentDialogueUnit.transform.rotation.y < 0)
            {
                rotAmt *= -1;
            }

            for (float i = currentDialogueUnit.transform.rotation.y; Mathf.Abs(i) < 180; i += rotAmt)
            {
                currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, i);
                yield return new WaitForSeconds(turnInterval);
            }
            rotated = true;
            StopCoroutine(currentCoroutine);
        }
        */
        bool rotated = false;

        if (currentDialogueUnit.transform.rotation.y < 0)
        {
            while (currentDialogueUnit.transform.rotation.y > -180)
            {
                currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, -2);
                yield return new WaitForSeconds(turnInterval);
            }
            rotated = true;
            StopCoroutine(currentCoroutine);
        }
        else if (!rotated)
        {
            while (currentDialogueUnit.transform.rotation.y < 180)
            {
                currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, 2);
                yield return new WaitForSeconds(turnInterval);
            }
            StopCoroutine(currentCoroutine);
        }
    }
}
