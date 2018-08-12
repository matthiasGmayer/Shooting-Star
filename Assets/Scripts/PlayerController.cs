using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Range(100f, 10000f)]
    public float acceleration;
    [Range(1f, 3f)]
    public float sprintFactor;
    public float spray;
    public float speed;
    public float shootDelay;
    private new Rigidbody2D rigidbody;
    public GameObject aim;
    public GameObject cam;
    private NetworkPlayer networkPlayer;
    public int health;
    public int maxHealth = 30;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        networkPlayer = GetComponent<NetworkPlayer>();
    }


    private float shootTime = 0;



    Vector2 move;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q)) networkPlayer.Damage(1);

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
        Vector2 start = transform.position;
        Vector2 target = aim.transform.position;
        float distance = (target - start).magnitude;
        target += UnityEngine.Random.insideUnitCircle * distance * spray / 10f;
        networkPlayer.FireBullet(networkPlayer.id, start, target, speed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Bullet_"))
        {

            int shooterId = int.Parse(col.gameObject.name.Split('_')[1]);
            if (networkPlayer.id != shooterId)
            {
                if (enabled)
                {
                    networkPlayer.Damage(1);
                    networkPlayer.DestroyBullet(col.gameObject);
                }
            }
        }
    }
}
