using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    [Range(1f, 1000f)]
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        target.z = 0;
        gameObject.transform.position = gameObject.transform.position - (gameObject.transform.position - target) * Mathf.Pow(0.99f, Time.deltaTime * 10000f / speed);
    }
}
