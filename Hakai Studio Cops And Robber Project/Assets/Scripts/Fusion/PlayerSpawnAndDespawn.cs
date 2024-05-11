using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnAndDespawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, new Vector3(0f, 20f, 0f), Quaternion.identity);
        }
    }
}

