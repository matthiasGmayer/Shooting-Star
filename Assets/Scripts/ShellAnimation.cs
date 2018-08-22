using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAnimation : MonoBehaviour
{

    public float groundTime;
    public float gravityTime;
    Rigidbody2D rig;
    // Use this for initialization
    void Start()
    {
        rig = GetComponentInChildren<Rigidbody2D>();
        Vector2 vel = Random.insideUnitCircle;
        vel.y += 1;
        rig.velocity = vel;
        rig.angularVelocity = ((Random.value * 2) - 1) * 300;
        Invoke("Ground", gravityTime);
    }

    void Ground()
    {
        Destroy(rig);
        GetComponentInChildren<ActiveSprite>().parent = gameObject;
        Destroy(gameObject, groundTime);
    }
}
