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

    private GameObject myShooter;

    private int maxLifeTime = 10;
    public void Init(GameObject shooter, GameObject targetObject)
    {
        transform.position = shooter.transform.position;
        this.myShooter = shooter;
        Vector3 target = targetObject.transform.position;
        float distance = (target - transform.position).magnitude;
        target += (Vector3)UnityEngine.Random.insideUnitCircle * distance * spray / 10f;
        targetDirection = (target - transform.position).normalized;
        Vector2 diff = (Vector2)(target - transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        GetComponent<Rigidbody2D>().velocity = (Vector3)targetDirection * speed;

        Destroy(gameObject, maxLifeTime);
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == myShooter) return;
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
