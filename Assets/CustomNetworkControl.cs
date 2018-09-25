using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkControl : NetworkManager {

    public string playerName;

    private ChatController myChat;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Called on the client when connected to the server.
    public override void OnClientConnect(NetworkConnection conn)
    {
        print("Received connect message");
        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();
        client.Send(2001, new StringMessage(playerName));
    }

    public void StartNetworkHost()
    {
        print("Starting host and registering handler, I think");
        StartHost();

        RegisterServerListeners();
        RegisterClientListeners();

    }

    public void StartNetworkClient()
    {
        print("Starting client and registering handler, I think");
        StartClient();

        RegisterClientListeners();
    }

    private void RegisterClientListeners()
    {
        client.RegisterHandler(2002, OnNameAssigned);
        client.RegisterHandler(2003, OnOtherPlayerJoinedGame);
    }

    public void OnOtherPlayerJoinedGame(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Other player joined message recieved, name is " + playerName);
        myChat.AnnouncePlayer(playerName);
    }

    public void OnNameAssigned(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Client player name assigned to " + playerName);
        myChat.SetLocalPlayerName(playerName);
    }
    

    private void RegisterServerListeners()
    {
        NetworkServer.RegisterHandler(2001, OnPlayerNameReceived);
    }

    public void OnPlayerNameReceived(NetworkMessage netMsg)
    {
        string playerName = netMsg.ReadMessage<StringMessage>().value;
        print("Received player name " + playerName);
        playerName = myChat.SetPlayerName(playerName, netMsg.conn.connectionId);
        NetworkServer.SendToClient(netMsg.conn.connectionId, 2002, new StringMessage(playerName));
        NetworkServer.SendToAll(2003, new StringMessage(playerName));
    }
    
    

}
