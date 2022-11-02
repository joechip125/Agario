using System;
using System.Collections;
using System.Collections.Generic;
using AgarioShared.AgarioShared.Enums;
using UnityEngine;

public class PlayerSelectPosition : MonoBehaviour
{
    private Camera _cam;
    private Plane _plane = new Plane(Vector3.up, 0);
    private string thePlayerName;
    public PlayerCounter PlayerCounter;

    private void Awake()
    {
        _cam = Camera.main;
        PlayerLink.Instance.SetPlayerCounter += SetPlayer;
    }

    private void OnDisable()
    {
        PlayerLink.Instance.SetPlayerCounter -= SetPlayer;
    }

    private void SetPlayer(string playerName, PlayerCounter count)
    {
        if (PlayerLink.Instance.PlayerName != playerName) return;
        PlayerCounter = count;
        thePlayerName = playerName;
    }
    
    
    
    
    private void Update()
    {
        if (PlayerCounter != PlayerLink.Instance.playerNumber) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out var distance))
            {
                PlayerLink.Instance.UpdateLocation(ray.GetPoint(distance), PlayerCounter);
            }
        }
    }
}
