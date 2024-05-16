using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Fusion;

public class RespawnScript : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject dieCamera;
    [SerializeField] private float maxRespawnTime = 5f;
    [SerializeField] private float currentRespawnTime;
    [SerializeField] private PlayerHealth playerHealth;

    private void Awake()
    {
        currentRespawnTime = maxRespawnTime;
    }

    private void Update()
    {
        if (dieCamera.GetComponentInChildren<Camera>().isActiveAndEnabled)
        {
            StartRespawn();
        }
    }

    private void StartRespawn()
    {
        currentRespawnTime -= Time.deltaTime * Runner.DeltaTime;

        if (currentRespawnTime <= 0) 
        {
            currentRespawnTime = maxRespawnTime;
            SpawnPlayer();
            dieCamera.GetComponentInChildren<Camera>().enabled = false;
        }
    }

    public void SpawnPlayer()
    {
        playerHealth.Spawned();
    }
}
