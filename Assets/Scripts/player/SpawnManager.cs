using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    /*public List<Transform> spawnPoints;*/

    public Transform spawnpoint;

    
    void Start()
    {

    }

    public void SpawnPlayer()
    {
        //Code voor meerdere spawnpoints

        /*int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Debug.Log("Spawn Point Index: " + spawnPointIndex);
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        Debug.Log("Spawned at " + spawnPoint.name);*/
        if (spawnpoint == null)
        {
            spawnpoint = player.transform;
        }
        player.transform.position = spawnpoint.position;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        /*spawnPoints.Add(spawnPoint);*/

        spawnpoint = spawnPoint;
    }
}
