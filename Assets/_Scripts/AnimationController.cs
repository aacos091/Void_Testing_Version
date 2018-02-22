using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour {

    [SerializeField]
    private GameObject unitObj;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

	// Use this for initialization
	void Start () {
        if (animator == null)
            animator = GetComponent<Animator>();
        rb = unitObj.GetComponent<Rigidbody>();
        navMeshAgent = unitObj.GetComponent<NavMeshAgent>();
        animator.SetBool("Talking", false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        animator.SetFloat("VertSpeed", Mathf.Abs(navMeshAgent.velocity.y));
        /*
        if (navMeshAgent.speed == 0)
            animator.SetBool("IsMoving", false);
        else
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", navMeshAgent.speed);
        }
        */
    }


}
