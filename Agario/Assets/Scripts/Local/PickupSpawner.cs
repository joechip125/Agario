using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public float SpawnInterval = 6f;
    public float timer;
    public int spawnAmount = 8;
    private int totalSpawned;
    public int maxSpawn = 100;
    public List<Vector3> spawnPoses = new();
    public static event Action RequestMoreSpawns; 

    void Start()
    {
        PlayerLink.Instance.OnSpawnPickups += BeginSpawning;
    }

    private void OnDisable()
    {
        PlayerLink.Instance.OnSpawnPickups -= BeginSpawning;
    }
    
    
    private void BeginSpawning(List<Vector3> positions)
    {
        spawnPoses = positions;
        Dispatcher.RunOnMainThread(SpawnPickups);
    }

    private void SpawnPickups()
    {
        if (totalSpawned >= maxSpawn) return;
        
        foreach (var p in spawnPoses)
        {
            var pickType = Random.Range(0, 2);
            
            var temp =Instantiate(pickups[pickType], p,Quaternion.identity);
            temp.transform.Rotate(Vector3.right, 90);
            totalSpawned++;
        }
    }
}
