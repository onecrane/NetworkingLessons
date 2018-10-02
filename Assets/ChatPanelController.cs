using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanelController : MonoBehaviour {

    public Text lblMessages;
    public InputField txtMessage;
    public Button btnChatSend;

    private ChatController myChat;
    private bool panelReactivatedLastFrame = false;

	// Use this for initialization
	void Start () {
        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();
	}
	
	// Update is called once per frame
	void Update () {

        ChatController.ChatMessage[] chatMessages = myChat.GetMessages();
        lblMessages.text = "I";
        int lines = myChat.maxMessages;

        List<string> messageLines = new List<string>();

        foreach (ChatController.ChatMessage chatMessage in chatMessages)
        {
            if (chatMessage.sender != null)
            {
                messageLines.Add(string.Format("{0}: {1}", chatMessage.sender, chatMessage.message));
            }
            else
            {
                messageLines.Add(chatMessage.message);
            }
        }
        while (messageLines.Count < lines) messageLines.Add(string.Empty);

        string unifiedMessages = string.Empty;
        messageLines.Reverse();
        foreach (string messageLine in messageLines)
        {
            unifiedMessages += messageLine + "\n";
        }

        lblMessages.text = unifiedMessages;

        if (panelReactivatedLastFrame)
        {
            txtMessage.MoveTextEnd(true);
            panelReactivatedLastFrame = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isEnabled = GetComponent<Image>().enabled;

            GetComponent<Image>().enabled = !isEnabled;

            lblMessages.enabled = !isEnabled;

            txtMessage.enabled = !isEnabled;
            txtMessage.GetComponent<Image>().enabled = !isEnabled;
            txtMessage.transform.Find("Placeholder").GetComponent<Text>().enabled = !isEnabled;
            txtMessage.transform.Find("Text").GetComponent<Text>().enabled = !isEnabled;

            btnChatSend.enabled = !isEnabled;
            btnChatSend.GetComponent<Image>().enabled = !isEnabled;
            btnChatSend.transform.Find("Text").GetComponent<Text>().enabled = !isEnabled;

            if (!isEnabled)
            {
                txtMessage.Select();
                txtMessage.ActivateInputField();
                panelReactivatedLastFrame = true;
            }
            else
            {
                txtMessage.DeactivateInputField();
            }
        }
	}

    public void txtMessage_OnEndEdit()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Treat it like clicking Send
            btnChatSend_OnClick();
        }
    }

    public void btnChatSend_OnClick()
    {
        string message = txtMessage.text.Trim();
        if (message.Length > 0)
        {
            myChat.AddMessage(message);
            txtMessage.text = string.Empty;
            txtMessage.Select();
            txtMessage.ActivateInputField();
        }
    }

}
