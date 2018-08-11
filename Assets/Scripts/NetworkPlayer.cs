using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour, IPunObservable
{

    public Camera cam;
    public GameObject aim, bullet, healthBar;
    public int id, health, maxHealth = 30;
    public UnityEngine.UI.Text nameText;
    public UnityEngine.UI.Text damageText;
    // Use this for initialization
    void Awake()
    {
        health = maxHealth;
        id = (int)photonView.instantiationData[0];
        nameText.text = (string)photonView.instantiationData[1];
        gameObject.name = "Player_" + id;
        if (photonView.isMine)
        {
            Camera.main.enabled = false;
            cam.enabled = true;
            aim.SetActive(true);
            GetComponent<PlayerController>().enabled = true;
            cam.transform.parent = null;
        }
    }

    public void SetId()
    {
        photonView.RPC("SetId", PhotonTargets.All, PhotonNetwork.player.ID);
    }

    private Vector3 correctPlayerPos;
    private float correctPlayerRot;

    private int bulletId = 0;
    public void FireBullet(int id, Vector2 start, Vector2 target, float speed)
    {
        photonView.RPC("GenerateBullet", PhotonTargets.All, id, start, target, speed);
    }

    [PunRPC]
    void DestroyBulletRCP(string name)
    {
        GameObject g = GameObject.Find(name);
        if (g == null) return;
        //Destroy(g);
        //return;
        Bullet b = g.GetComponent<Bullet>();
        if (b == null) return;
        b.DestroySelf();
    }
    public void DestroyBullet(GameObject gameObject)
    {
        photonView.RPC("DestroyBulletRCP", PhotonTargets.All, gameObject.name);
    }

    [PunRPC]
    void GenerateBullet(int shooter, Vector2 start, Vector2 target, float speed)
    {
        GameObject bullet = Instantiate(this.bullet);

        bullet.name = "Bullet_" + shooter + "_" + bulletId++;
        bullet.GetComponent<Bullet>().shooter = gameObject;

        Vector2 position = start;
        bullet.transform.position = position;
        Vector2 targetDirection = (target - position).normalized;
        Vector2 diff = target - position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        bullet.GetComponent<Rigidbody2D>().velocity = targetDirection * speed;

        
    }

    [PunRPC]
    void DestroyPlayerRCP(int id)
    {
        Destroy(GameObject.Find("Player_" + id));        
    }

    public void Die()
    {
        photonView.RPC("DestroyPlayerRCP", PhotonTargets.Others, id);
        DestroySelf();
        PhotonNetwork.Disconnect();
    }
    void DestroySelf()
    {
        Destroy(aim);
        Destroy(cam.gameObject);
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health < 0) Die();

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }

    void Update()
    {
        damageText.text = health.ToString();
        float size = (float)health / maxHealth * 3;
        healthBar.transform.localPosition = new Vector3(-1.5f + (size) * 0.5f,0,0);
        healthBar.transform.localScale = new Vector3(size,1,1);
    }
}