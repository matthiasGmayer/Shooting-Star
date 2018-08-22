using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{

    public GameObject weapon;
    private SpriteRenderer spriteRenderer;
    private Transform fire;


    public void Setup(Transform fire)
    {
        this.fire = fire;
        spriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        float rot = gameObject.transform.rotation.eulerAngles.z;
        bool b = rot > 0 && rot < 180;
        weapon.GetComponentInChildren<SpriteRenderer>().flipY = b;
        if (fire != null)
            fire.localPosition = new Vector2(fire.localPosition.x, Mathf.Abs(fire.localPosition.y) * (b ? -1 : 1));

    }
}
