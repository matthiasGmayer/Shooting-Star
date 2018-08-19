using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{

    [SerializeField]
    private RuntimeAnimatorController kevin, birgit, brad, johnson, zombie;
    public static Character selectedAnimation;

    [SerializeField]
    private Sprite kevinSprite, birgitSprite, bradSprite, johnsonSprite, zombieSprite;

    public static Sprite GetSprite(Character a)
    {
        switch (a)
        {
            case Character.kevin:
                return instance.kevinSprite;
            case Character.birgit:
                return instance.birgitSprite;
            case Character.brad:
                return instance.bradSprite;
            case Character.johnson:
                return instance.johnsonSprite;
            case Character.zombie:
                return instance.zombieSprite;
            default: return null;
        }
    }
    public static RuntimeAnimatorController GetAnimation(Character a)
    {
        switch (a)
        {
            case Character.kevin:
                return instance.kevin;
            case Character.birgit:
                return instance.birgit;
            case Character.brad:
                return instance.brad;
            case Character.johnson:
                return instance.johnson;
            case Character.zombie:
                return instance.zombie;
            default: return null;
        }
    }

    public enum Character
    {
        kevin, birgit, brad, johnson, zombie
    }

    private static Animations instance;
    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
