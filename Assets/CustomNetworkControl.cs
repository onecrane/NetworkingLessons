using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkControl : NetworkManager {
    
    public class ChatMessage : MessageBase
    {
        public string sender;
        public string message;
    }

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
        client.RegisterHandler(3000, OnChatMessageReceived);
    }

    public void OnChatMessageReceived(NetworkMessage netMsg)
    {
        ChatMessage chatMessage = netMsg.ReadMessage<ChatMessage>();
        myChat.ReceiveMessage(chatMessage.sender, chatMessage.message);
    }

    internal void SendChatMessage(string message)
    {
        client.Send(3001, new StringMessage(message));
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
        NetworkServer.RegisterHandler(3001, OnPlayerSentChatMessage);
    }

    public void OnPlayerSentChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        string sender = myChat.GetNameFromConnectionId(netMsg.conn.connectionId);

        if (message.Length > 80) message = message.Substring(0, 80);
        NetworkServer.SendToAll(3000, new ChatMessage() { sender = sender, message = message });
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
