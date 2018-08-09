using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    Vector2 targetDirection;
    [SerializeField]
    private float spray = 1;
    [SerializeField]
    private float speed = 1;

    private GameObject shooter;

    private int maxLifeTime = 10;
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == shooter) return;
        if (c.gameObject.tag.Equals("H"))
        {
            Hit();
        }
    }

    private void Hit()
    {
        Destroy(gameObject);
    }
}
