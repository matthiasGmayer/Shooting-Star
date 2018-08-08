using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCommands : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    [Command]
    public void CmdFire(GameObject shooter, GameObject bullet, GameObject aim)
    {
        GameObject b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.GetComponent<Bullet>().Init(gameObject, aim);
        NetworkServer.Spawn(b);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
