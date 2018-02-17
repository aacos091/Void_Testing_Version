using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour {

    [SerializeField]
    private GameObject unitObj;
    private NavMeshAgent navMeshAgent;
    //private 

	// Use this for initialization
	void Start () {
        navMeshAgent = unitObj.GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
