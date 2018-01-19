using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAwake : MonoBehaviour {

	//All of this is attached to Camera
	public Crew _FirstMate;
	public Crew _Pilot;
	public Crew _Cook;
	public Crew _Doctor;
	public Crew _Engineer;

	public int crewCulprit;
	public int crewAlive;

    //Need to flag if the crew had been set once, so that it does not generate again once it loads

	void Awake() {
        if (!AccusationManager.S.crewSet)
        {
            //Rolls for starting victim
            crewAlive = Random.Range(0, 2);
            //Rolls for culprit
            crewCulprit = Random.Range(0, 2);

            //Sets who dies at the start
            if (crewAlive == 1)
            {
                _Cook.alive = false;
                Debug.Log("Cook is dead");
            }
            else
            {
                _Engineer.alive = false;
                Debug.Log("Engineer is dead");
            }

            //Sets who is the culprit
            if (crewCulprit == 1)
            {
                _FirstMate.culprit = true;
                Debug.Log("The culprit is the FirstMate");
            }
            else
            {
                _Pilot.culprit = true;
                Debug.Log("The culprit is the Pilot");
            }
        }
        AccusationManager.S.crewSet = true;

        AccusationManager.S.gameObject.SetActive(true);
    }
}