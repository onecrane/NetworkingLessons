using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.Networking.NetworkManager>().StartHost();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
