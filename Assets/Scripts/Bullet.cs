using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    public GameObject trail, shooter;

    private int shooterId;

    void Start()
    {
        Invoke("Destroy", 10);
    }

    public void DestroySelf()
    {
        trail.transform.parent = null;
        trail.GetComponent<ShutDown>().Shutdown();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player")) return;
        if (shooter == null||col.gameObject.name.Equals(shooter.name)) return;
        Destroy();
    }

    public void Destroy()
    {
        if (shooter == null)
        {
            DestroySelf();
            return;
        }
        else if (PhotonNetwork.player.ID == int.Parse(shooter.name.Split('_')[1]))
            shooter.GetComponent<NetworkPlayer>().DestroyBullet(gameObject);
        else if(Settings.DestroyClientBulletsOnHit)
            DestroySelf();
    }

}
