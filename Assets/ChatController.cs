using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatController : MonoBehaviour {
    
    public struct ChatMessage
    {
        public string sender;
        public string message;
    }

    private Dictionary<int, string> namesByConnectionId = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIdsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    public int maxMessages = 5;
    private string localPlayerName = "LocalPlayer";

    private List<ChatMessage> messages = new List<ChatMessage>();

    private CustomNetworkControl myNetworkControl;

    public string GetNameFromConnectionId(int connectionId)
    {
        return namesByConnectionId[connectionId];
    }

    public void OnChatMessageReceived(string sender, string message)
    {
        messages.Add(string.Format("[User chat] {0}: {1}", sender, message));
//        messages.Add(message);
    }

    public string GetNameByConnectionId(int connectionId)
    {
        return namesByConnectionId[connectionId];
    }

    public void SetLocalPlayerName(string playerName)
    {
        localPlayerName = playerName;
    }

    internal ChatMessage[] GetMessages()
    {
        return messages.ToArray();
    }

    // Called by the local player only
    internal void AddMessage(string message)
    {
        if (myNetworkControl == null)
        {
            messages.Insert(0, new ChatMessage() { sender = localPlayerName, message = message });
            if (messages.Count > maxMessages)
            {
                messages.RemoveAt(messages.Count - 1);
            }
        }
        else
        {
            // Send through the network
            myNetworkControl.SendChatMessage(message);
        }
    }

    public void ReceiveMessage(string sender, string message)
    {
        messages.Insert(0, (new ChatMessage() { sender = sender, message = message }));
    }

    public void AnnouncePlayer(string playerName)
    {
        print("Pretty sure I'm announcing a player named " + playerName);
        messages.Add(new ChatMessage() { sender = null, message = string.Format("*** Player {0} joined ***", playerName) });
    }

    private void Awake()
    {
        print("Chat's awake");
    }

    // Use this for initialization
    void Start () {
        myNetworkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyDown(KeyCode.Space)) messages.Add("Hello?");
    }

    private string EnsureUnique(string playerName, int connectionId)
    {
        // Base case: Not actually changing the name.
        if (namesByConnectionId.ContainsKey(connectionId) && namesByConnectionId[connectionId] == playerName) return playerName;

        // Somebody else has this name; return an altered form of it.
        int suffix = 0;
        while (connectionIdsByName.ContainsKey(playerName))
        {
            string suffixString = (++suffix).ToString();
            while (playerName.Length + suffixString.Length > maxNameLength) playerName = playerName.Substring(0, playerName.Length - 1);
            playerName += suffixString;
        }

        return playerName;
    }

    // Called by the server only.
    internal string SetPlayerName(string playerName, int connectionId)
    {
        playerName = EnsureUnique(playerName, connectionId);

        if (namesByConnectionId.ContainsKey(connectionId))
        {
            // Remove from both indexes first.
            connectionIdsByName.Remove(namesByConnectionId[connectionId]);
            namesByConnectionId.Remove(connectionId);
        }

        connectionIdsByName.Add(playerName, connectionId);
        namesByConnectionId.Add(connectionId, playerName);

        return playerName;
    }


}
