using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ChatController : MonoBehaviour {
    
    private Dictionary<int, string> namesByConnectionId = new Dictionary<int, string>();
    private Dictionary<string, int> connectionIdsByName = new Dictionary<string, int>();
    public int maxNameLength = 20;
    private string localPlayerName;

    public List<string> messages = new List<string>();

    public void SetLocalPlayerName(string playerName)
    {
        localPlayerName = playerName;
    }

    public void AnnouncePlayer(string playerName)
    {
        print("Pretty sure I'm announcing a player named " + playerName);
        messages.Add(string.Format("*** Player {0} joined ***", playerName));
    }

    private void Awake()
    {
        print("Chat's awake");
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) messages.Add("Hello?");
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

    private void OnGUI()
    {
        foreach (string message in messages)
        {
            GUILayout.Label(message);
        }

    }

}
