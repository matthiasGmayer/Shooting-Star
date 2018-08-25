using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;

public class NetworkLobby : PunBehaviour
{

    public InputField usernameField;
    public InputField passwordField;
    public GameObject loginPanel, roomPanel, createRoomPanel, inGamePanel, characterPanel, statPanel, background, player;
    public GameObject[] objectsToSpawn;
    private GameObject[] menuStates;
    public GameObject room;
    public GameObject sliderFillArea;
    public InputField createRoomNameField;
    public InputField createRoomPasswordField;
    public Slider createRoomMaxPlayers;

    public GameObject myPlayer;
    public int myId;
    public Dictionary<int, string> playerNames = new Dictionary<int, string>();
    public Dictionary<int, int> playerKills = new Dictionary<int, int>();
    public Dictionary<int, int> playerDeaths = new Dictionary<int, int>();

    public static NetworkLobby instance;


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        PhotonNetwork.sendRate = Settings.SendRate;
        PhotonNetwork.sendRateOnSerialize = Settings.SendRateOnSerialize;
        if (Settings.SkipLogin)
        {
            Connect();
            ToMenuState(roomPanel);
        }
        DontDestroyOnLoad(gameObject);
        menuStates = new GameObject[] { loginPanel, roomPanel, createRoomPanel, inGamePanel, characterPanel, statPanel };
    }

    void Update()
    {
        if (PhotonNetwork.inRoom)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inGamePanel.SetActive(!inGamePanel.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                statPanel.SetActive(!statPanel.activeSelf);
            }
        }

    }

    public void Connect()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(Settings.Version);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

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


    List<GameObject> roomList = new List<GameObject>();
    public void RefreshRooms()
    {
        foreach (var room in roomList)
        {
            Destroy(room);
        }
        roomList.Clear();
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        int i = 0;
        foreach (RoomInfo roomInfo in rooms)
        {
            GameObject room = Instantiate(this.room, sliderFillArea.transform);
            roomList.Add(room);
            room.transform.localPosition = new Vector3(0, 50 * i++, 0);
            room.GetComponent<ManageRoom>().Init(roomInfo);
        }
    }
    void Reset()
    {
        playerNames.Clear();
        playerDeaths.Clear();
        playerKills.Clear();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        playerNames.Remove(otherPlayer.ID);
        playerDeaths.Remove(otherPlayer.ID);
        playerKills.Remove(otherPlayer.ID);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        foreach (var g in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(g);
        }
    }

    public void PlayAsGuest()
    {
        Connect();
    }

    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
        //background.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ToMenuState(roomPanel);
        myPlayer.GetComponent<NetworkPlayer>().Destroy();
        Reset();
        //Destroy(GameObject.Find("PlayerCamera"));
        //background.SetActive(false);
    }
    public void ToMenuState(GameObject o)
    {
        foreach (GameObject ob in menuStates)
        {
            if (ob == o) continue;
            ob.SetActive(false);
        }
        if (o != null)
            o.SetActive(true);
        if (o == roomPanel)
            Invoke("RefreshRooms", 0.01f);
    }

    public override void OnDisconnectedFromPhoton()
    {
        ToMenuState(loginPanel);
        base.OnDisconnectedFromPhoton();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    public override void OnJoinedLobby()
    {
        ToMenuState(roomPanel);
        base.OnJoinedLobby();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ToMenuState(null);
        Spawn();
    }

    public override void OnCreatedRoom()
    {

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { RoomProperties.password, createRoomPasswordField.text }
        };
        PhotonNetwork.room.SetCustomProperties(properties);
        PhotonNetwork.room.MaxPlayers = (int)createRoomMaxPlayers.value;
        base.OnCreatedRoom();
        SetupStage();
    }
    private void Spawn()
    {
        myId = PhotonNetwork.player.ID;
        string name = Database.Name;
        if ("".Equals(name)) name = "Guest" + myId;
        myPlayer = PhotonNetwork.Instantiate(player.gameObject.name, Vector3.zero, Quaternion.identity, 0, new object[] { myId, name, (int)Characters.selectedAnimation });
    }

    private void SetupStage()
    {
        string[] names = new string[objectsToSpawn.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = objectsToSpawn[i].name;
        }

        Sprite sprite = background.GetComponent<SpriteRenderer>().sprite;
        float width = background.transform.localScale.x / sprite.pixelsPerUnit * sprite.texture.width;
        float height = background.transform.localScale.y / sprite.pixelsPerUnit * sprite.texture.height;
        width -= width / 2;
        height -= height / 2;
        for (int i = 0; i < 100; i++)
        {
            Vector3 position = new Vector3((UnityEngine.Random.value * 2 - 1) * width, (UnityEngine.Random.value * 2 - 1) * height, 0);
            int name = (int)(UnityEngine.Random.value * names.Length);
            PhotonNetwork.InstantiateSceneObject(names[name], position, Quaternion.identity, 0, null);
        }
    }

    public enum RoomProperties
    {
        password
    }

    public void CreateRoom()
    {
        string name = createRoomNameField.text;
        if (name == null || name.Equals("")) return;
        CharacterMenu.roomName = name;
        CharacterMenu.creatingRoom = true;
        ToMenuState(characterPanel);
    }
}
