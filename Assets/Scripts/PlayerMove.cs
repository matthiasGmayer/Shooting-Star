using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : MonoBehaviour {

    [Range(100f, 10000f)]
    public float acceleration;
    [Range(1f, 3f)]
    public float sprintFactor;
    public float shootDelay;
    private new Rigidbody2D rigidbody;
    public GameObject bullet, aim;
    private PlayerCommands playerCommands;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        playerCommands = GetComponent<PlayerCommands>();
	}


    private float shootTime = 0;



    Vector2 move;
    // Update is called once per frame
    void FixedUpdate () {
        shootTime += Time.deltaTime;
        move = new Vector2();
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
        if (Input.GetKey(KeyCode.W))
        {
            move += new Vector2(0, 1);            
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector2(1, 0);
        }
        move.Normalize();
        move *= Time.fixedDeltaTime * acceleration;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= sprintFactor;
        }


        rigidbody.AddForce(move);
	}

    void Fire()
    {
        if (shootTime < shootDelay)
        {
            return;
        }
        shootTime = 0;
        playerCommands.CmdFire(gameObject, bullet, aim);
    }
}
