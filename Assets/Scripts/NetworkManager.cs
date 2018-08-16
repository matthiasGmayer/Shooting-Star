using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : PunBehaviour {

    [SerializeField]
    private GameObject player;
    public GameObject barrel, background,menu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);
        }
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

    public void Disconnect()
    {
        SceneManager.LoadScene("Lobby");
        PhotonNetwork.LeaveRoom();
    }
}
