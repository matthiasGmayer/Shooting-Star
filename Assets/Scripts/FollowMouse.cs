using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    [Range(0.1f, 10f)]
    public float speed;
    public Camera cam;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
        target.z = 0;
        //gameObject.transform.position = gameObject.transform.position - (gameObject.transform.position - target) * Mathf.Pow(0.99f, Time.deltaTime * 10000f / speed);
        gameObject.transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }
}
