using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class RequestServerTime : MonoBehaviour
{
    private TcpClient _client;
    private Button theButton;
    private NetworkStream stream;
    private string adress = "127.0.0.1";
    Int32 port = 13000;
    private TextMeshProUGUI text;
    private string dateTime = "0000";
    

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        
    }


    public void ClickButton()
    {
        _client = new TcpClient(adress, port);
        stream = _client.GetStream();
        var clientReader = new StreamReader(stream);
        var clientWriter = new  StreamWriter(stream);
        clientWriter.AutoFlush = true;
            
        Debug.Log(clientReader.ReadLine());
        clientWriter.WriteLine("time2");
        
        
        var buffer = new byte[1024];
        var output = stream.Read(buffer,0, buffer.Length);
        var usedBytes = buffer.Count(b => b != 0);

        var printThis = Encoding.UTF8.GetString(buffer, 0, usedBytes);

        text.text = $"Current Date And Time: {printThis}";
    }
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
