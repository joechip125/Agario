using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text.Json;
using System.Threading;
using AgarioShared;
using AgarioShared.AgarioShared.Enums;
using AgarioShared.AgarioShared.Messages;
using TMPro;
using UnityEngine.UI;

public class NetworkHandler : MonoBehaviour
{
    private TcpClient _client;
    private Button startButton;
    Int32 port = 13000;
    IPAddress _localAddress = IPAddress.Parse("127.0.0.1");
    private TMP_InputField _nameField;
    private string _playerName;
    public GameObject startScreen;
    public GameObject spawnablePlayer;
    private StartDictionaryMessage _startDictionary;
    private PlayerSelectPosition _selectPosition;
    public static event Action<string, PlayerCounter> SetMeshName;

    private StartMessage _startMessage;
    public List<PlayerMesh> spawnedPlayers = new();

    private void Awake()
    {
        startButton = startScreen.GetComponentInChildren<Button>();
        startButton.onClick.AddListener(Connect);

        _nameField = startScreen.GetComponentInChildren<TMP_InputField>();
        _nameField.onEndEdit.AddListener(SetPlayerName);
        _nameField.onValueChanged.AddListener(SetPlayerName);
        PlayerLink.Instance.StartMultiAction += OnstartMulti;
        PlayerLink.Instance.StartSingleAction += OnStartSingle;
    }


    private void OnStartSingle(StartMessage message)
    {
        _startMessage = message;
   //    Dispatcher.RunOnMainThread(SpawnOwnedPlayer);
    }

    private void SpawnOwnedPlayer()
    {
        Debug.Log($"spawning player{_startMessage.PlayerName}");
        Debug.Log($"spawning player{PlayerLink.Instance.PlayerName}");
        var spawnPos = new Vector3(0, 0.1f, 0);
        var spawn = Instantiate(spawnablePlayer, spawnPos, Quaternion.identity);
        spawn.GetComponent<PlayerMesh>().playerName = PlayerLink.Instance.PlayerName;
        spawn.GetComponent<PlayerMesh>().PlayerCounter = PlayerLink.Instance.playerNumber;
        spawnedPlayers.Add(spawn.GetComponent<PlayerMesh>());
    }
    
    private void OnstartMulti(StartDictionaryMessage dict)
    {
        _selectPosition.PlayerCounter = PlayerLink.Instance.playerNumber;
        _startDictionary = dict;
        Dispatcher.RunOnMainThread(SpawnOtherPlayers);
    }

    private void SpawnOtherPlayers()
    {
        Debug.Log("Spawning players");
        foreach (var p in _startDictionary.StartMessages)
        {
            if (spawnedPlayers.SingleOrDefault(x => x.PlayerCounter == p.Key) != default)
                continue;
            
            var spawnPos = new Vector3(p.Value.X, p.Value.Y, p.Value.Z);
            var spawn = Instantiate(spawnablePlayer, spawnPos, Quaternion.identity);
            spawn.GetComponent<Movement>().PlayerCounter = p.Value.PlayerCounter;
            spawn.GetComponent<PlayerMesh>().PlayerCounter = p.Value.PlayerCounter;
            spawnedPlayers.Add(spawn.GetComponent<PlayerMesh>());
        }
    }

    private void SetPlayerName(string theName)
    {
        _playerName = theName;
    }
    
    private void OnDisable()
    {
        PlayerLink.Instance.StartMultiAction -= OnstartMulti;
        PlayerLink.Instance.StartSingleAction -= OnStartSingle;
        startButton.onClick.RemoveAllListeners();
        
        _nameField.onEndEdit.RemoveAllListeners();
        _nameField.onValueChanged.RemoveAllListeners();
    }

    private void Connect()
    {
        if (_playerName.Length < 1) return;
        
        startScreen.SetActive(false);
        _client = new TcpClient("127.0.0.1", port);
  
        PlayerLink.Instance.Init(_client, _playerName);
        
        var mess = new StartMessage()
        {
            PlayerName = _playerName
        };
        _selectPosition = gameObject.AddComponent<PlayerSelectPosition>();
        
        
        PlayerLink.Instance.SendMessage(mess);
    }

}
