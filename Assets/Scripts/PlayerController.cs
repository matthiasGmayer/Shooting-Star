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
    public GameObject arm;
    public GameObject weapon;
    private GameObject bulletSpawn;
    private NetworkPlayer networkPlayer;
    public Animator animator;
    public Vector2 animationMove = new Vector2();
    public int health;
    public int maxHealth = 30;
    public bool controlled = false;
    public Vector3 armPosition;
    private ActiveSprite armActiveSprite;
    public ActiveSprite weaponActiveSprite;
    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        armPosition = arm.transform.localPosition;
        armActiveSprite = arm.GetComponentInChildren<ActiveSprite>();
        weaponActiveSprite = weapon.GetComponentInChildren<ActiveSprite>();
        bulletSpawn = weapon.transform.Find("Renderer").Find("BulletSpawn").gameObject;
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
        if (controlled)
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
            animationMove = new Vector2(move.x, move.y);

            move *= Time.fixedDeltaTime * acceleration;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= sprintFactor;
            }
            rigidbody.AddForce(move);
            arm.transform.right = aim.transform.position - arm.transform.position;
            arm.transform.Rotate(new Vector3(0, 0, 90));
        }
        animator.SetFloat("x", animationMove.x);
        animator.SetFloat("y", animationMove.y);
    }
    void Update()
    {
        int offset = 1;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("up"))
        {
            arm.transform.localPosition = new Vector3(-armPosition.x, armPosition.y, 0);
            offset = -2;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("left"))
        {
            arm.transform.localPosition = new Vector3(0, armPosition.y, 0);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("right"))
        {
            arm.transform.localPosition = new Vector3(0, armPosition.y, 0);
        }
        else
        {
            arm.transform.localPosition = armPosition;
        }
        SetArmOffset(offset);
    }

    void SetArmOffset(int o)
    {
        armActiveSprite.offset = o;
        weaponActiveSprite.offset = o + 1;
    }

    void Fire()
    {
        if (shootTime < shootDelay)
        {
            return;
        }
        shootTime = 0;
        Vector2 start = bulletSpawn.transform.position;
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
                if (controlled)
                {
                    networkPlayer.Damage(1);
                    networkPlayer.DestroyBullet(col.gameObject);
                }
            }
        }
    }
}
