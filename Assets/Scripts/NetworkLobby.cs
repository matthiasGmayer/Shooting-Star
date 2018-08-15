using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;

public class NetworkLobby : PunBehaviour {

    public InputField usernameField;
    public InputField passwordField;
    public GameObject loginPanel, roomPanel, createRoomPanel;
    public GameObject room;
    public GameObject sliderFillArea;
    public InputField createRoomNameField;
    public InputField createRoomPasswordField;
    public Slider createRoomMaxPlayers;

    void Start()
    {
        PhotonNetwork.sendRate = Settings.SendRate;
        PhotonNetwork.sendRateOnSerialize = Settings.SendRateOnSerialize;
        DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(Settings.Version);
    }

    //public void LoginThreaded()
    //{
    //    new System.Threading.Thread(Login).Start();
    //}

    public void Login()
    {
        Database.Login(usernameField.text, passwordField.text);
        if (Database.LoggedIn)
        {
            Connect();
        }
        else
        {
            passwordField.text = null;
        }
    }

    public void RefreshRooms()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        int i = 0;
        foreach (RoomInfo roomInfo in rooms)
        {
            GameObject room = Instantiate(this.room, sliderFillArea.transform);
            room.transform.position = new Vector3(0, 50 * i++, 0);
            room.GetComponent<ManageRoom>().Init(roomInfo);
        }
    }

    public void PlayAsGuest()
    {
        Connect();
    }

    public void ToLogin()
    {
        loginPanel.SetActive(true);
        roomPanel.SetActive(false);
        createRoomPanel.SetActive(false);
    }
    public void ToRooms()
    {
        loginPanel.SetActive(false);
        roomPanel.SetActive(true);
        createRoomPanel.SetActive(false);
    }
    public void ToCreateRoom()
    {
        loginPanel.SetActive(false);
        roomPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public override void OnDisconnectedFromPhoton()
    {
        ToLogin();
        base.OnDisconnectedFromPhoton();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    public override void OnJoinedLobby()
    {
        ToRooms();
        base.OnJoinedLobby();
    }

    public override void OnCreatedRoom()
    {

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add(RoomOptions.password, createRoomPasswordField.text);
        PhotonNetwork.room.SetCustomProperties(properties);
        PhotonNetwork.room.MaxPlayers = (int)createRoomMaxPlayers.value;
        base.OnCreatedRoom();
    }

    public enum RoomOptions
    {
        password
    }

    public void CreateRoom()
    {
        SceneManager.LoadScene("Standard");
        PhotonNetwork.CreateRoom(createRoomNameField.text);
    }
}
