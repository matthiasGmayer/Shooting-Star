using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDown : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shutdown()
    {
        Invoke("DestroyMyself", 0.05f);
    }
    void DestroyMyself()
    {
        GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject, 1);
    }
}
