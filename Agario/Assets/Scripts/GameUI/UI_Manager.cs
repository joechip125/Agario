using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AgarioShared;
using AgarioShared.AgarioShared.Enums;
using AgarioShared.AgarioShared.Messages;
using AgarioShared.Assets.Scripts.AgarioShared.Messages;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class UI_Manager : MonoBehaviour
{
    private TextMeshProUGUI scoreBoard;
    private PlayerCounter playerNumber;

    private int score;
    private int rank;
    private Vector3 position;
 
    void Start()
    {
        Debug.Log(new DeathDictionary().T);
        scoreBoard = GetComponentInChildren<TextMeshProUGUI>();
        scoreBoard.text = "Current score: 0";
        PlayerLink.Instance.ScoreUpdated += NewScore;
        PlayerLink.Instance.UpdateTheRankings += UpdateRankings;
        PlayerLink.Instance.SetPlayerCounter += SetPlayerCounter;
    }
    
    private void OnDisable()
    {
        PlayerLink.Instance.ScoreUpdated -= NewScore;
        PlayerLink.Instance.UpdateTheRankings -= UpdateRankings;
        PlayerLink.Instance.SetPlayerCounter -= SetPlayerCounter;
    }

    private void SetPlayerCounter(string playerName, PlayerCounter counter)
    {
        if (playerName != PlayerLink.Instance.PlayerName) return;
        playerNumber = counter;
    }
    
    private void UpdateRankings(List<string> names)
    {
        int counter = 1;
        foreach (var n in names)
        {
            var text = GetComponentsInChildren<TextMeshProUGUI>()[counter];

            text.text = $"{counter}. {n}";
            
            counter++;
        }
    }
    
    private void NewScore(int theScore, PlayerCounter number)
    {
        if (playerNumber != number) return;
        
        scoreBoard.text = $"Current score: {theScore}";
    }

}
