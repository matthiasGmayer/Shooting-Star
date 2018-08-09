using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    public GameObject trail, shooter;

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
        if (col.gameObject.name.Contains("Player_")) return;
        Destroy();
    }

    public void Destroy()
    {
        if (shooter == null) {
            DestroySelf();
            return;
        }
        shooter.GetComponent<NetworkPlayer>().DestroyBullet(gameObject);
    }

}
