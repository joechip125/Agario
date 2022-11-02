using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class UDPScript : MonoBehaviour
{
    private TMP_InputField field;
    private TextMeshProUGUI text;
    private UnityEngine.UI.Button sendButton;
    private string theInput;

    private void Awake()
    {
        field = GetComponentInChildren<TMP_InputField>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        sendButton = GetComponentInChildren<UnityEngine.UI.Button>();

        sendButton.onClick.AddListener(SendMessage);
        field.onSubmit.AddListener(InputFieldValue);
        field.onEndEdit.AddListener(InputFieldValue);
        
    }

    private void OnDisable()
    {
        field.onSubmit.RemoveAllListeners();
        field.onEndEdit.RemoveAllListeners();
        sendButton.onClick.RemoveAllListeners();
    }

    private void InputFieldValue(string input)
    {
        theInput = input;
    }

    private void SendMessage()
    {
        UdpClient client = new UdpClient();
        IPEndPoint epSend = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        var epReceive = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
        client.EnableBroadcast = true;
        
        var bytes = ConvertStringToByteArray(theInput);
        client.Send(bytes, bytes.Length, epSend);

        var data = client.Receive(ref epSend);
        text.text = ConvertBytesToString(data);
    }
    
    
    private string ConvertBytesToString(byte[] bytes)
    {
        var numBytes = bytes.Count(x => x != 0);
        var outString = Encoding.UTF8.GetString(bytes, 0, numBytes);
            
        return outString;
    }
        
    private byte[] ConvertStringToByteArray(string theString)
    {
        var count = Encoding.UTF8.GetByteCount(theString);
        var byteArray = new byte[count];
        byteArray = Encoding.UTF8.GetBytes(theString);
            
        return byteArray;
    }
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
