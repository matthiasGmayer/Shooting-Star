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
    public Camera mainCamera;
    public GameObject barrel, background;

    public UnityEngine.UI.InputField nameField;
    // Use this for initialization
    void Start () {
        PhotonNetwork.sendRate = Settings.SendRate;
        PhotonNetwork.sendRateOnSerialize = Settings.SendRateOnSerialize;
    }

    public void Connect()
    {
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(Settings.Version);
    }

    public override void OnDisconnectedFromPhoton()
    {
        base.OnDisconnectedFromPhoton();
        mainMenu.SetActive(true);
        mainCamera.enabled = true;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        mainMenu.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }



    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Sprite sprite = background.GetComponent<SpriteRenderer>().sprite;
        float width = background.transform.localScale.x / sprite.pixelsPerUnit * sprite.texture.width;
        float height = background.transform.localScale.y / sprite.pixelsPerUnit * sprite.texture.height;
        width -= width / 2;
        height -= height / 2;
        for (int i = 0; i < 100; i++)
        {
            Vector3 position = new Vector3(UnityEngine.Random.value * width, UnityEngine.Random.value * height, 0);
            PhotonNetwork.InstantiateSceneObject(barrel.name, position, Quaternion.identity, 0, null);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Spawn();
    }

    private void Spawn()
    {
        int id = PhotonNetwork.player.ID;
        string name = Database.Name;
        if ("".Equals(name)) name = "Guest" + id;
        PhotonNetwork.Instantiate(player.gameObject.name, Vector3.zero, Quaternion.identity, 0, new object[] { id, name }).SetActive(true);
    }

}
