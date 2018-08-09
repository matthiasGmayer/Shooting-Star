using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class NetworkManager : PunBehaviour {

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject player;



    // Use this for initialization
    void Start () {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 30;
    }

    public void Connect()
    {
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby");
        mainMenu.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Creating Room");
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Spawn();
    }

    private void Spawn()
    {
        Debug.Log("Attempting Spawn");
        GameObject p = PhotonNetwork.Instantiate(player.gameObject.name, Vector3.zero, Quaternion.identity, 0);
    }





    // Update is called once per frame
    void Update () {
		
	}
}
