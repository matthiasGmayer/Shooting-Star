using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour {

    public GameObject SkinPreview;
    public static string roomName;
    public static bool creatingRoom;
    public new static GameObject gameObject;

    void Awake()
    {
        gameObject = base.gameObject;
    }
    // Use this for initialization
    void Start () {
        int length = Enum.GetNames(typeof(Characters.Character)).Length - 1;
        for (int i = 0; i < length; i++)
        {
            Characters.Character c = (Characters.Character)i;
            GameObject o = Instantiate(SkinPreview, gameObject.transform);
            o.transform.localPosition = new Vector3((i-length/2f)*100, 0, 0);
            o.GetComponent<SkinPreview>().Init(c);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void JoinRoom()
    {
        if(creatingRoom)
            PhotonNetwork.CreateRoom(roomName);
        else
            NetworkLobby.instance.JoinRoom(roomName);
    }
    public void Back(GameObject o)
    {
        if (creatingRoom)
            PhotonNetwork.LeaveRoom();
        NetworkLobby.instance.ToMenuState(o);
    }
}
