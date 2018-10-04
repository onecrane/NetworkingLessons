using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisablePlayerControlsOnRemotes : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (!GetComponent<NetworkIdentity>().isClient && !GetComponent<NetworkIdentity>().isServer) return;

        if (!GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            GetComponent<FirstPersonControl>().enabled = false;
            GetComponent<FireControls>().enabled = false;
        }

    }
    
}
