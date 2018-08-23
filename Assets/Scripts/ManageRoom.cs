using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageRoom : MonoBehaviour {

    public Text playerText;
    public Text roomNameText;
    public Text lockedText;
    private GameObject characterMenu;
    private string password;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Init(RoomInfo roomInfo)
    {
        playerText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        roomNameText.text = roomInfo.Name;
        password = (string)roomInfo.CustomProperties[NetworkLobby.RoomProperties.password];
        if(password == null || password.Equals(""))
        {
            lockedText.text = "Unlocked";
        }
        else
        {
            lockedText.text = password;
        }
        characterMenu = NetworkLobby.instance.characterPanel;
    }

    public void JoinRoom()
    {
        Debug.Log("Join");
        CharacterMenu.roomName = roomNameText.text;
        CharacterMenu.creatingRoom = false;
        NetworkLobby.instance.ToMenuState(characterMenu);
    }
}
