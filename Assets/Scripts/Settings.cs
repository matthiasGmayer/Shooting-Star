using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    public bool destroyClientBulletsOnHit;
    public static Settings instance;
    public int sendRate;
    public int sendRateOnSerialize;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Update()
    {
        Debug.Log(PhotonNetwork.connectionState);
    }
}
