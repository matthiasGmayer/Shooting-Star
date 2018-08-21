using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField]
    private GameObject lugger, karabiner, mp40;

    private static Weapons instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static GameObject GetWeapon(Weapon w)
    {
        switch (w)
        {
            case Weapon.karabiner: return instance.karabiner;
            case Weapon.lugger: return instance.lugger;
            case Weapon.mp40: return instance.mp40;
            default: return null;
        }
    }
    public enum Weapon
    {
        lugger,
        karabiner,
        mp40
    }
}
