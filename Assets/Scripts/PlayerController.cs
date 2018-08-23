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
    public GameObject weapon, shell;
    private GameObject weaponChild;
    private Transform bulletSpawn, shellSpawn;
    public Collider2D bulletCollider;
    private GameObject weaponFire;
    private RotateWeapon rotateWeaponScript;
    private NetworkPlayer networkPlayer;
    public Animator animator;
    public Vector2 animationMove = new Vector2();
    public bool controlled = false;
    public Vector3 armPosition;
    private ActiveSprite armActiveSprite, weaponActiveSprite, weaponFireActiveSprite;
    public GameObject playerRenderer;
    private Vector3 renderPosition;
    private float spray, speed, shootDelay, reloadTime;
    private AudioSource fireSound;
    private int damage, magazinSize, shootCount = 0;
    public Weapons.Weapon currentWeapon;
    // Use this for initialization
    void Start()
    {
        renderPosition = playerRenderer.transform.localPosition;
        rigidbody = GetComponent<Rigidbody2D>();
        armPosition = arm.transform.localPosition;
        armActiveSprite = arm.GetComponentInChildren<ActiveSprite>();
        rotateWeaponScript = arm.GetComponentInChildren<RotateWeapon>();
        SetWeapon(Weapons.Weapon.lugger);
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
            if (Input.GetKey(KeyCode.Alpha1)) CheckWeapon(Weapons.Weapon.lugger);
            if (Input.GetKey(KeyCode.Alpha2)) CheckWeapon(Weapons.Weapon.karabiner);
            if (Input.GetKey(KeyCode.Alpha3)) CheckWeapon(Weapons.Weapon.mp40);
            //if (Input.GetKey(KeyCode.Q)) networkPlayer.Damage(1, 0);

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

    public void CheckWeapon(Weapons.Weapon w)
    {
        if (w == currentWeapon) return;
        else
        {
            currentWeapon = w;
            SetWeapon(w);
        }
    }

    int animLast;
    float animTime;
    private float animLength = 0.512f;
    private static readonly float animOffset = 0.01f;
    private static readonly Vector2[][] animVectors = new Vector2[][]
    {
        new Vector2[]{new Vector2(0,0), new Vector2(animOffset, animOffset), new Vector2(0,0), new Vector2(-animOffset, animOffset) },
        new Vector2[]{new Vector2(0,0), new Vector2(0,0), new Vector2(animOffset, 0), new Vector2(animOffset, 0)},
        new Vector2[]{new Vector2(0,0), new Vector2(0,0), new Vector2(-animOffset, 0), new Vector2(-animOffset, 0)},
        new Vector2[]{ new Vector2(0, 0), new Vector2(animOffset, -animOffset), new Vector2(0, 0), new Vector2(-animOffset, -animOffset) },
        new Vector2[]{ new Vector2(0, 0), new Vector2(0, -animOffset / 10f), new Vector2(0, 0), new Vector2(0, animOffset / 10f) }
    };
    void Update()
    {
        animTime += Time.deltaTime;
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
        if (animLast != i)
        {
            animLast = i;
            animTime = 0;
        }
        int t = Mathf.FloorToInt((animTime % animLength) / animLength * 4);
        playerRenderer.transform.localPosition = renderPosition + (Vector3)animVectors[i][t];
    }

    void SetArmOffset(int o)
    {
        armActiveSprite.offset = o;
        weaponActiveSprite.offset = o + 1;
        weaponFireActiveSprite.offset = o + 2;
    }

    public void SetWeapon(Weapons.Weapon w)
    {
        Destroy(weaponChild);
        weaponChild = Instantiate(Weapons.GetWeapon(w), weapon.transform);
        weaponActiveSprite = weaponChild.GetComponentInChildren<ActiveSprite>();
        weaponActiveSprite.parent = gameObject;
        Transform renderer = weaponChild.transform.Find("Renderer");
        bulletSpawn = renderer.Find("BulletSpawn");
        shellSpawn = renderer.Find("ShellSpawn");
        weaponFire = renderer.Find("Fire").gameObject;
        weaponFireActiveSprite = weaponFire.GetComponentInChildren<ActiveSprite>();
        weaponFireActiveSprite.parent = gameObject;
        Weapon ws = weaponChild.GetComponent<Weapon>();
        rotateWeaponScript.Setup(weaponFire.transform);
        speed = ws.Speed;
        shootDelay = ws.ShootDelay;
        spray = ws.Spray;
        damage = ws.Damage;
        reloadTime = ws.ReloadTime;
        magazinSize = ws.MagazinSize;
        shootCount = 0;
        fireSound = weaponChild.GetComponentInChildren<AudioSource>();
        Debug.Log(fireSound.name);
    }

    public void FireSound()
    {
        fireSound.Play();
    }

    void Fire()
    {
        if(magazinSize < shootCount)
        {
            shootCount = 0;
            shootTime = -reloadTime;
        }
        if (shootTime < shootDelay)
        {
            return;
        }
        shootCount++;
        shootTime = 0;
        Vector2 start = bulletSpawn.transform.position;
        Vector2 target = aim.transform.position;
        float distance = (target - start).magnitude;
        target += UnityEngine.Random.insideUnitCircle * distance * spray / 10f;
        networkPlayer.FireBullet(networkPlayer.id, start, target, speed, damage);
    }

    public void FireAnim()
    {
        if (weaponFire != null)
        {
            weaponFire.SetActive(true);
            GameObject s = Instantiate(shell);
            s.transform.position = shellSpawn.position;
            s.GetComponentInChildren<ActiveSprite>().parent = gameObject;
            Invoke("StopFireAnim", 0.05f);
        }
    }

    void StopFireAnim()
    {
        weaponFire.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.name.Contains("Bullet_"))
        {
            if (!bulletCollider.IsTouching(col)) return;
            string[] info = col.gameObject.name.Split('_');
            int shooterId = int.Parse(info[1]);
            if (networkPlayer.id != shooterId)
            {
                if (controlled)
                {
                    networkPlayer.DestroyBullet(col.gameObject);
                    networkPlayer.Damage(int.Parse(info[3]), shooterId);
                }
            }
        }
    }
}
