using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField]
    private float spray, speed, shootDelay;
    [SerializeField]
    private int damage;
    public float Spray { get { return spray; } }
    public float Speed { get { return speed; } }
    public float ShootDelay { get { return shootDelay; } }
    public int Damage { get { return damage; } }
}
