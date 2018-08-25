using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{

    public GameObject weapon;
    // Update is called once per frame
    void Update()
    {
        float rot = gameObject.transform.rotation.eulerAngles.z;
        bool b = rot > 0 && rot < 180;
        weapon.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, b ? -1 : 1);
    }
}
