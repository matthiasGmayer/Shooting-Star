using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    public Behaviour[] ComponentsToDisable;
    public Camera cam;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            foreach (Behaviour b in ComponentsToDisable) b.enabled = false;
            cam.enabled = false;
        }
        else
            cam.transform.parent = null;
            
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
