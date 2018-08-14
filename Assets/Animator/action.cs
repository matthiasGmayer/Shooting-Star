using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class action : MonoBehaviour {
    private Animator animator;

    // Use this for initialization
    void Start () {
       animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        animator = gameObject.GetComponent<Animator>();
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("standing", false);
            animator.SetBool("right",false);
            animator.SetBool("left",true);
            animator.SetBool("down",false);
            animator.SetBool("up",false);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("standing", false);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            animator.SetBool("down", false);
            animator.SetBool("up", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("standing", false);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            animator.SetBool("down", true);
            animator.SetBool("up", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("standing", false);
            animator.SetBool("right", true);
            animator.SetBool("left", false);
            animator.SetBool("down", false);
            animator.SetBool("up", false);
        }
        else
        {
            animator.SetBool("standing", true);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            animator.SetBool("down", false);
            animator.SetBool("up", false);
        }
    }
}
