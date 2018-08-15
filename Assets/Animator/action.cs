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
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.A)) x = -1;
        else if (Input.GetKey(KeyCode.W)) y = 1;
        else if (Input.GetKey(KeyCode.S)) y = -1;
        else if (Input.GetKey(KeyCode.D)) x = 1;

            animator.SetFloat("x", x);
        animator.SetFloat("y",y);
    }
}
