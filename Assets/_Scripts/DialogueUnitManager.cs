using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUnitManager : MonoBehaviour {
    //Singleton of this class
    public static DialogueUnitManager S;

    public GameObject currentDialogueUnit;
    private Animator currentAnimator;
    private AnimationController currentAnimController;


    public float turnInterval = .02f;
    public Coroutine currentCoroutine;

	// Use this for initialization
	void Start () {
        S = this;
	}

    public void SetDialogueUnit(GameObject unitGO)
    {
        currentDialogueUnit = unitGO;
        currentAnimController = currentDialogueUnit.GetComponent<AnimationController>();
        currentAnimator = currentAnimController.animator;
    }

    public void StartTalking()
    {
        StartCoroutine(TurnToCamera());
        currentAnimController.FreezeNavMesh();
        currentAnimator.SetBool("Talking", true);
    }

    public void StopTalking()
    {
        currentAnimController.UnFreezeNavMesh();
        currentAnimator.SetBool("Talking", false);
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        StartCoroutine(TurnBackFromCamera());

    }

    public IEnumerator TurnToCamera()
    {
        bool rotated = false;

        if (currentDialogueUnit.transform.localRotation.y < 0 && currentDialogueUnit.transform.localRotation.y > -360)
        {
         
            while (currentDialogueUnit.transform.localRotation.y > -180)
            {
                currentDialogueUnit.transform.rotation = Quaternion.RotateTowards(currentDialogueUnit.transform.rotation, Quaternion.Euler(0, -180, 0), 2);
                yield return new WaitForSeconds(turnInterval);
 
            }
 
            rotated = true;
            StopCoroutine(currentCoroutine);
        }
        else if (!rotated)
        {
            
            while (currentDialogueUnit.transform.localRotation.y < 180)
            {
                currentDialogueUnit.transform.rotation = Quaternion.RotateTowards(currentDialogueUnit.transform.rotation, Quaternion.Euler(0, 180 ,0), 2);
                yield return new WaitForSeconds(turnInterval);
            }

            rotated = true;
            StopCoroutine(currentCoroutine);
        }
    }

    public IEnumerator TurnBackFromCamera()
    {
       
        bool rotated = false;

        if (currentDialogueUnit.transform.localRotation.y < 0 && currentDialogueUnit.transform.localRotation.y > -360)
        {
            //currentDialogueUnit.transform.rotation = Quaternion.Euler(0, -180, 0);
            // yield return new WaitForSeconds(turnInterval);


            while (currentDialogueUnit.transform.localRotation.y > -180)
            {
                //currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, -2);
                currentDialogueUnit.transform.rotation = Quaternion.RotateTowards(currentDialogueUnit.transform.rotation, Quaternion.Euler(0, -180, 0), 2);
                yield return new WaitForSeconds(turnInterval);

            }

            rotated = true;
            StopCoroutine(currentCoroutine);
        }
        else if (!rotated)
        {
            //currentDialogueUnit.transform.rotation = Quaternion.Euler(0, 180, 0);
            //yield return new WaitForSeconds(turnInterval);


            while (currentDialogueUnit.transform.localRotation.y < 180)
            {
                //currentDialogueUnit.transform.RotateAround(currentDialogueUnit.transform.position, Vector3.up, 2);
                currentDialogueUnit.transform.rotation = Quaternion.RotateTowards(currentDialogueUnit.transform.rotation, Quaternion.Euler(0, 180, 0), 2);
                yield return new WaitForSeconds(turnInterval);
            }

            rotated = true;
            StopCoroutine(currentCoroutine);
        }
    }
}
