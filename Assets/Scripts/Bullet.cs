using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    Vector2 targetDirection;
    public float spray;
    public float speed;


	// Use this for initialization
	void Start () {
	}

    private int maxLifeTime = 10;
    public void Init(Transform t)
    {
        Vector3 target = t.position;
        float distance = (target - transform.position).magnitude;
        target += (Vector3)Random.insideUnitCircle * distance * spray / 10f;
        targetDirection = (target - transform.position).normalized;
        Vector2 diff = (Vector2)(target - transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        Destroy(gameObject, maxLifeTime);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += (Vector3)targetDirection * speed;

    }
}
