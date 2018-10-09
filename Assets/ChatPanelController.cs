using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanelController : MonoBehaviour {

    public Text lblMessages;
    public InputField txtMessage;
    public Button btnSend;
    public Text lblServerIP;

    public int numberOfLines = 5;

    private List<string> displayLines = new List<string>();

    private CustomNetworkControl myNetworkControl;
    private ChatController myChat;

	// Use this for initialization
	void Start () {

        lblMessages.text = string.Empty;

        txtMessage.ActivateInputField();
        txtMessage.Select();

        
        GameObject networkController = GameObject.FindGameObjectWithTag("NetworkController");
        if (networkController != null)
        {
            myNetworkControl = networkController.GetComponent<CustomNetworkControl>();
        }
        if (myNetworkControl.isServer)
        {
            lblServerIP.text = "Server IP: " + myNetworkControl.serverIP;
        } else
        {
            lblServerIP.text = "Connected to: " + myNetworkControl.networkAddress;
        }

        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();

    }

    // Update is called once per frame
    void Update () {
		if (myNetworkControl != null)
        {
            displayLines.Clear();
            for (int i = myChat.messages.Count - 1; i >= 0 && displayLines.Count < numberOfLines; i--)
            {
                displayLines.Add(myChat.messages[i]);
            }
            displayLines.Reverse();
            RefreshMessageDisplay();
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            print("Message display has width " + lblMessages.rectTransform.rect.width);
            // Debug the width of each line
            string trueText = lblMessages.text;
            foreach (string message in displayLines)
            {
                lblMessages.text = message;
                float preferredWidth = lblMessages.preferredWidth;
                print(string.Format("Message [{0}] has preferred width {1}", message, preferredWidth));
            }
            lblMessages.text = trueText;
        }
    }


    public void btnSend_Click()
    {

        string message = txtMessage.text.Trim();
        if (message.Length > 0)
        {
            if (myNetworkControl == null)
            {
                displayLines.Add(message);
                if (displayLines.Count > numberOfLines)
                {
                    displayLines.RemoveAt(0);
                }

                RefreshMessageDisplay();
            }
            else
            {
                myNetworkControl.SendChatMessage(message);
            }





            txtMessage.text = string.Empty;
        }

        txtMessage.ActivateInputField();



    }

    public void RefreshMessageDisplay()
    {
        lblMessages.text = string.Empty;
        for (int i = 0; i < displayLines.Count; i++)
        {
            lblMessages.text += displayLines[i];
            if (i < displayLines.Count - 1) lblMessages.text += "\n";
        }
    }

    public void txtMessage_EndEdit()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            btnSend_Click();
        }
    }

}
