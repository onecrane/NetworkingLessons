using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatController : MonoBehaviour {

    private NetworkManager myNetworkManager;

    private int randomId;
    private int messageSendCount = 0;

	// Use this for initialization
	void Start () {
        print("MyNetworkBehaviour.Start");
        myNetworkManager = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkManager>();
        randomId = Random.Range(0, 100000);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            myNetworkManager.client.Send(500, new StringMessage(string.Format("Msg {0} from client {1}", messageSendCount++, randomId)));
        }
    }
}
