using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour, IPunObservable
{

    public GameObject cam, audioObject;
    public GameObject bullet, healthBar;
    public int id;
    public int health, maxHealth = 100, kills, deaths;
    public UnityEngine.UI.Text nameText;
    private string Name
    {
        get
        {
            return nameText.text;
        }
    }
    public UnityEngine.UI.Text damageText;
    private PlayerController playerController;


    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        if (photonView.isMine)
        {
            //Camera.main.enabled = false;
            cam.SetActive(true);
            //aim.SetActive(true);
            playerController.controlled = true;
            audioObject.GetComponent<AudioListener>().enabled = true;
            cam.transform.parent = null;
        }
        id = (int)photonView.instantiationData[0];
        nameText.text = (string)photonView.instantiationData[1];
        NetworkLobby.instance.playerNames.Add(id, Name);
        NetworkLobby.instance.playerKills.Add(id, 0);
        NetworkLobby.instance.playerDeaths.Add(id, 0);
        Characters.Character c = (Characters.Character)(int)photonView.instantiationData[2];
        GetComponentInChildren<Animator>().runtimeAnimatorController = Characters.GetAnimation(c);
        playerController.arm.GetComponentInChildren<SpriteRenderer>().sprite = Characters.GetArm(c);
        gameObject.name = "Player_" + id;
    }


    void Update()
    {

        damageText.text = health.ToString();
        float size = (float)health / maxHealth * 3;
        healthBar.transform.localPosition = new Vector3(-1.5f + (size) * 0.5f, 0, 0);
        healthBar.transform.localScale = new Vector3(size, 1, 1);
    }

    private Vector3 correctPlayerPos;
    private float correctPlayerRot;

    private int bulletId = 0;
    public void FireBullet(int id, Vector2 start, float rot, float speed, int damage)
    {
        photonView.RPC("GenerateBullet", PhotonTargets.All, id, bulletId++, start, rot, speed, damage, PhotonNetwork.time);
    }

    [PunRPC]
    void DestroyBulletRCP(string name)
    {
        GameObject g = GameObject.Find(name);
        if (g == null) return;
        Bullet b = g.GetComponent<Bullet>();
        b.DestroySelf();
    }
    public void DestroyBullet(GameObject gameObject)
    {
        if (!PhotonNetwork.connected) return;
        DestroyBulletRCP(gameObject.name);
        photonView.RPC("DestroyBulletRCP", PhotonTargets.Others, gameObject.name);
    }

    [PunRPC]
    void SetWeaponRPC(int weapon)
    {
        playerController.SetWeapon((Weapons.Weapon)weapon);
    }

    public void SetWeapon(Weapons.Weapon w)
    {
        photonView.RPC("SetWeaponRPC", PhotonTargets.All, (int)w);
    }

    [PunRPC]
    void GenerateBullet(int shooter, int bulletId, Vector2 start, float rot, float speed, int damage, double time)
    {
        GameObject bullet = Instantiate(this.bullet);

        bullet.name = "Bullet_" + shooter + "_" + bulletId + "_" + damage;
        bullet.GetComponent<Bullet>().shooter = gameObject;

        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot);
        bullet.transform.position = start;
        Rigidbody2D rig = bullet.GetComponent<Rigidbody2D>();
        rot -= 90;
        rot *= Mathf.Deg2Rad;
        rig.velocity = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)) * speed;
        if (shooter != PhotonNetwork.player.ID)
        {
            rig.MovePosition(bullet.transform.position + (Vector3)rig.velocity * (float)(PhotonNetwork.time - time));
        }

        playerController.FireAnim();
        playerController.FireSound();

    }

    [PunRPC]
    void DestroyPlayerRCP(int id)
    {
        Destroy(GameObject.Find("Player_" + id));
    }

    public void Destroy()
    {
        photonView.RPC("DestroyPlayerRCP", PhotonTargets.Others, id);
        DestroySelf();
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void AddKillRPC(int id)
    {
        if (NetworkLobby.instance.myId == id)
            NetworkLobby.instance.myPlayer.GetComponent<NetworkPlayer>().kills++;
        //GameObject.Find("Player_" + id).GetComponent<NetworkPlayer>().kills++;
    }

    public void Die(int shooter)
    {
        transform.position = new Vector3();
        health = maxHealth;
        deaths++;
        photonView.RPC("AddKillRPC", PhotonTargets.All, shooter);
    }
    void DestroySelf()
    {
        Destroy(cam);
        Destroy(gameObject);
    }

    public void Damage(int amount, int shooter)
    {
        health -= amount;
        if (health <= 0) Die(shooter);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);
            stream.SendNext(kills);
            stream.SendNext(deaths);
            stream.SendNext(playerController.animationMove);
            stream.SendNext(playerController.arm.transform.rotation);
            stream.SendNext(playerController.currentWeapon);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            kills = (int)stream.ReceiveNext();
            deaths = (int)stream.ReceiveNext();
            playerController.animationMove = (Vector2)stream.ReceiveNext();
            playerController.arm.transform.rotation = (Quaternion)stream.ReceiveNext();
            playerController.CheckWeapon((Weapons.Weapon)stream.ReceiveNext());
        }
        NetworkLobby.instance.playerKills[id] = kills;
        NetworkLobby.instance.playerDeaths[id] = deaths;
    }
}