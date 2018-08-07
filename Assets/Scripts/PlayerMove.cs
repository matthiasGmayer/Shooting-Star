using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [Range(100f, 10000f)]
    public float acceleration;
    private new Rigidbody2D rigidbody;
    public GameObject bullet;
    public GameObject aim;
    public float shootDelay;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}


    private float shootTime = 0;
    private void Fire()
    {
        if(shootTime < shootDelay)
        {
            return;
        }
        shootTime = 0;
        GameObject b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.GetComponent<Bullet>().Init(aim.transform);
    }


    Vector2 move;
    // Update is called once per frame
    void Update () {
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
        move *= Time.deltaTime * acceleration;


        rigidbody.AddForce(move);
	}
}
