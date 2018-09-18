using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkControl : MonoBehaviour {

    private NetworkManager myNetworkManager;

	// Use this for initialization
	void Start () {
        myNetworkManager = GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartHost()
    {
        myNetworkManager.StartHost();
    }

    public void StartClient()
    {
        myNetworkManager.StartClient();
    }
}
