using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkControl : MonoBehaviour {

    private NetworkManager myNetworkManager;
    List<string> messages = new List<string>();

	// Use this for initialization
	void Start () {
        myNetworkManager = GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartHost()
    {
        print("Starting host and registering handler, I think");
        myNetworkManager.StartHost();
        myNetworkManager.client.RegisterHandler(100, OnHello);
        NetworkServer.RegisterHandler(101, OnHelloServer);
        NetworkServer.RegisterHandler(500, OnHelloServer);
        NetworkServer.RegisterHandler(501, OnHelloServer);
    }

    public void StartClient()
    {
        print("Starting client and registering handler, I think");
        myNetworkManager.StartClient();
        myNetworkManager.client.RegisterHandler(100, OnHello);
        NetworkServer.RegisterHandler(101, OnHelloServer);
    }

    private void OnGUI()
    {
        foreach (string message in messages)
        {
            GUILayout.Label(message);
        }
    }

    public void OnHello(NetworkMessage netMsg)
    {
        string messageValue = netMsg.ReadMessage<StringMessage>().value;
        messages.Add(messageValue);
        print("Something called hello here in NetworkControl, message is " + messageValue);
    }

    public void OnHelloServer(NetworkMessage netMsg)
    {
        string messageValue = netMsg.ReadMessage<StringMessage>().value;
        print("Something called hello (server) here in NetworkControl, message is " + messageValue);
        NetworkServer.SendToAll(100, new StringMessage(netMsg.channelId.ToString() + ": " + messageValue));
    }

}
