﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMenuController : MonoBehaviour {

    public CustomNetworkControl myCustomNetworkControl;
    private RandomName myRandomName;

    public InputField txtPlayerName;
    public InputField txtServerIp;

	// Use this for initialization
	void Start () {
		if (myCustomNetworkControl == null)
        {
            myCustomNetworkControl = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>();
        }
        myRandomName = GetComponent<RandomName>();
        btnRandomize_Click();


        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string hostName = System.Net.Dns.GetHostName();
        foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(hostName).AddressList)
        {
            sb.AppendLine(ip.ToString());

            if (IsValidIPAddress(ip.ToString()))
            {
                txtServerIp.text = ip.ToString();
                myCustomNetworkControl.serverIP = ip.ToString();
            }

        }
        print(sb.ToString());


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void btnStartServer_Click()
    {
        myCustomNetworkControl.StartNetworkHost();
    }

    public void btnStartClient_Click()
    {
        string ipAddress = txtServerIp.text.Trim();
        if (!IsValidIPAddress(ipAddress))
        {
            txtServerIp.text = string.Empty;
        }
        else
        {
            myCustomNetworkControl.networkAddress = txtServerIp.text;
            myCustomNetworkControl.StartNetworkClient();
        }
    }

    public void btnRandomize_Click()
    {
        txtPlayerName.text = myRandomName.GetRandomName();
    }

    public void txtPlayerName_OnChange()
    {
        myCustomNetworkControl.playerName = txtPlayerName.text;
    }

    private bool IsValidIPAddress(string ipAddress)
    {
        string[] components = ipAddress.Split('.');
        if (components.Length != 4) return false;

        uint a, b, c, d;
        if (!uint.TryParse(components[0], out a)) return false;
        if (!uint.TryParse(components[1], out b)) return false;
        if (!uint.TryParse(components[2], out c)) return false;
        if (!uint.TryParse(components[3], out d)) return false;

        if (a > 223) return false;
        if (b > 255) return false;
        if (c > 255) return false;
        if (d > 255) return false;

        if (a + b == 0) return false;

        return true;

    }

}
