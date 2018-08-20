using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour {

    public GameObject weapon;

	// Update is called once per frame
	void Update () {
        float rot = gameObject.transform.rotation.eulerAngles.z;
        weapon.GetComponentInChildren<SpriteRenderer>().flipY = rot > 0 && rot < 180;

	}
}
