using System;
using System.Collections;
using System.Collections.Generic;
using AgarioShared.AgarioShared.Messages;
using UnityEngine;

public class Battler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMesh>() == null) return;
        
        PlayerLink.Instance.SendBattleMessage(new BattleMessage
        {
            otherPlayer = other.GetComponent<PlayerMesh>().PlayerCounter,
            thisPlayer = gameObject.GetComponent<PlayerMesh>().PlayerCounter
        });
    }
    
}
