using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRefresh : MonoBehaviour {

    public GameObject Stat;
    private List<GameObject> list = new List<GameObject>();

	void OnEnable()
    {
        NetworkLobby n = NetworkLobby.instance;
        int i = 0;
        foreach (var p in n.playerNames)
        {
            int id = p.Key;
            GameObject o = Instantiate(Stat, transform);
            list.Add(o);
            o.GetComponent<StatInit>().Init(p.Value, n.playerKills[id], n.playerDeaths[id], i++);
        }
    }
    void OnDisable()
    {
        foreach (var o in list) Destroy(o);
        list.Clear();
    }
}
