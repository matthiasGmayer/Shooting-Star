using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;
    public float speed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = player.transform.position;
        target.z = gameObject.transform.position.z;
        //gameObject.transform.position = gameObject.transform.position - (gameObject.transform.position - target) * Mathf.Pow(0.99f, Time.deltaTime * 10000f / speed);
        gameObject.transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }
}
