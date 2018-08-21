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
    private new Rigidbody2D rigidbody;
    public GameObject aim;
    public GameObject cam;
    public GameObject arm;
    public GameObject weapon;
    private GameObject weaponChild;
    private GameObject bulletSpawn;
    private NetworkPlayer networkPlayer;
    public Animator animator;
    public Vector2 animationMove = new Vector2();
    public bool controlled = false;
    public Vector3 armPosition;
    private ActiveSprite armActiveSprite;
    private ActiveSprite weaponActiveSprite;
    public GameObject playerRenderer;
    private Vector3 renderPosition;
    private float spray, speed, shootDelay;
    private int damage;
    // Use this for initialization
    void Start()
    {
        renderPosition = playerRenderer.transform.localPosition;
        rigidbody = GetComponent<Rigidbody2D>();
        armPosition = arm.transform.localPosition;
        armActiveSprite = arm.GetComponentInChildren<ActiveSprite>();
        SetWeapon(Weapons.Weapon.mp40);
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
            if (Input.GetKeyDown(KeyCode.Alpha1)) networkPlayer.SetWeapon(Weapons.Weapon.lugger);
            if (Input.GetKeyDown(KeyCode.Alpha2)) networkPlayer.SetWeapon(Weapons.Weapon.karabiner);
            if (Input.GetKeyDown(KeyCode.Alpha3)) networkPlayer.SetWeapon(Weapons.Weapon.mp40);
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

    int last;
    float time;
    private float animLength = 0.512f;
    private static readonly float animOffset = 0.01f;
    private static readonly Vector2[][] animVectors = new Vector2[][]
    {
        new Vector2[]{new Vector2(0,0), new Vector2(-animOffset, animOffset), new Vector2(0,0), new Vector2(animOffset, animOffset) },
        new Vector2[]{new Vector2(0,0), new Vector2(0,0), new Vector2(animOffset, 0), new Vector2(animOffset, 0)},
        new Vector2[]{new Vector2(0,0), new Vector2(0,0), new Vector2(-animOffset, 0), new Vector2(-animOffset, 0)},
        new Vector2[]{ new Vector2(0, 0), new Vector2(animOffset, -animOffset), new Vector2(0, 0), new Vector2(-animOffset, -animOffset) },
        new Vector2[]{ new Vector2(0, 0), new Vector2(0, -animOffset / 10f), new Vector2(0, 0), new Vector2(0, animOffset / 10f) }
    };
    void Update()
    {
        time += Time.deltaTime;
        int offset = 1;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("up"))
        {
            arm.transform.localPosition = new Vector3(-armPosition.x, armPosition.y, 0);
            offset = -2;
            AnimateJiggle(0);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("left"))
        {
            arm.transform.localPosition = new Vector3(0, armPosition.y, 0);
            AnimateJiggle(1);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("right"))
        {
            arm.transform.localPosition = new Vector3(0, armPosition.y, 0);
            AnimateJiggle(2);
        }
        else
        {
            arm.transform.localPosition = armPosition;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("down"))
                AnimateJiggle(3);
            else
                AnimateJiggle(4);
        }
        SetArmOffset(offset);
    }

    void AnimateJiggle(int i)
    {
        if (last != i)
        {
            last = i;
            time = 0;
        }
        int t = Mathf.FloorToInt((time % animLength) / animLength * 4);
        playerRenderer.transform.localPosition = renderPosition + (Vector3)animVectors[i][t];
    }

    void SetArmOffset(int o)
    {
        armActiveSprite.offset = o;
        weaponActiveSprite.offset = o + 1;
    }

    public void SetWeapon(Weapons.Weapon w)
    {
        Destroy(weaponChild);
        weaponChild = Instantiate(Weapons.GetWeapon(w), weapon.transform);
        //weaponChild.transform.parent = weapon.transform;
        weaponActiveSprite = weaponChild.GetComponentInChildren<ActiveSprite>();
        weaponActiveSprite.parent = gameObject;
        bulletSpawn = weaponChild.transform.Find("Renderer").Find("BulletSpawn").gameObject;
        Weapon ws = weaponChild.GetComponent<Weapon>();
        speed = ws.Speed;
        shootDelay = ws.ShootDelay;
        spray = ws.Spray;
        damage = ws.Damage;
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
        networkPlayer.FireBullet(networkPlayer.id, start, target, speed, damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Bullet_"))
        {
            string[] info = col.gameObject.name.Split('_');
            int shooterId = int.Parse(info[1]);
            if (networkPlayer.id != shooterId)
            {
                if (controlled)
                {
                    networkPlayer.Damage(int.Parse(info[2]));
                    networkPlayer.DestroyBullet(col.gameObject);
                }
            }
        }
    }
}
