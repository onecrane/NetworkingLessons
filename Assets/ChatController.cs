using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    private Dictionary<int, string> namesByConnectionId = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIdsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    public int maxMessages = 5;
    private string localPlayerName = "LocalPlayer";

    public List<string> messages = new List<string>();

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

    // Called by the local player only
    internal void AddMessage(string message)
    {
        if (myNetworkControl == null)
        {
            OnChatMessageReceived(localPlayerName, message);
            if (messages.Count > maxMessages)
            {
                messages.RemoveAt(messages.Count - 1);
            }
            print("Added message, count is " + messages.Count.ToString());
        }
        else
        {
            // Send through the network
            myNetworkControl.SendChatMessage(message);
        }
    }

    public void AnnouncePlayer(string playerName)
    {
        messages.Add(string.Format("*** Player {0} joined ***", playerName));
    }

    private void Awake()
    {
        print("Chat's awake");
    }

    // Use this for initialization
    void Start () {
        GameObject networkControllerObject = GameObject.FindGameObjectWithTag("NetworkController");
        if (networkControllerObject != null)
        {
            myNetworkControl = networkControllerObject.GetComponent<CustomNetworkControl>();
        }
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
