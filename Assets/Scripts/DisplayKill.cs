using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayKill : MonoBehaviour {

    List<GameObject> killTexts = new List<GameObject>();
    GameObject kill;

    public void ShowKill(int shooter, int dead)
    {
        Dictionary<int, string> d = NetworkLobby.instance.playerNames;
        GameObject k = Instantiate(kill, transform);
        k.GetComponent<Text>().text = d[dead] + " was shot by " + d[shooter];
        killTexts.Add(k);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
