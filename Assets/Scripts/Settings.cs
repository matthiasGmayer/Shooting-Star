using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    [SerializeField]
    private bool destroyClientBulletsOnHit;
    public static bool DestroyClientBulletsOnHit
    {
        get
        {
            return instance.destroyClientBulletsOnHit;
        }
    }
    private static Settings instance;
    [SerializeField]
    private int sendRate;
    public static int SendRate
    {
        get
        {
            return instance.sendRate;
        }
    }
    [SerializeField]
    private int sendRateOnSerialize;
    public static int SendRateOnSerialize
    {
        get
        {
            return instance.sendRateOnSerialize;
        }
    }
    [SerializeField]
    private string version;
    public static string Version
    {
        get
        {
            return instance.version;
        }
    }
    [SerializeField]
    private string database;
    public static string Database
    {
        get
        {
            return instance.database;
        }
    }

    [SerializeField]
    private bool skipLogin;
    public static bool SkipLogin
    {
        get
        {
            return instance.skipLogin;
        }
    }
    [SerializeField]
    private float semi3Dprecision;
    public static float Semi3Dprecision
    {
        get
        {
            return instance.semi3Dprecision;
        }
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Debug.Log(PhotonNetwork.connectionState);
    }
}
