using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerReadyManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    public static PlayerReadyManager Instance { get; private set; }

    private Dictionary<PlayerRef, bool> playerReadyDictionary = new Dictionary<PlayerRef, bool>();
    public Dictionary<PlayerRef, bool> PlayerReadyDictionary { get { return playerReadyDictionary; } }

    [SerializeField] private TextMeshProUGUI playerReadyText;

    int playerReady;

    private void Awake()
    {
        Instance = this;

        playerReadyText = GameObject.FindAnyObjectByType<PlayerReady>().GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetPlayerReady()
    {
        RpcSendingPlayerReady();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RpcSendingPlayerReady(RpcInfo info = default)
    {
        RpcUpdatePlayerReady(info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RpcUpdatePlayerReady(PlayerRef player)
    {
        playerReady = 0;

        if (playerReadyText == null)
        {
            playerReadyText = GameObject.FindAnyObjectByType<PlayerReady>().GetComponent<TextMeshProUGUI>();
        }

        if (player == Runner.LocalPlayer)
        {
            playerReadyDictionary[player] = true;
        }

        foreach(bool ready in playerReadyDictionary.Values)
        {
            if (ready)
            {
                playerReady++;
            }
        }

        playerReadyText.text = $"Player Ready {playerReady} / {playerReadyDictionary.Keys.Count}";

        if (Runner.IsServer)
        {
            bool allPlayersReady = true;

            foreach(bool ready in playerReadyDictionary.Values)
            {
                if (!ready)
                {
                    allPlayersReady = false;
                    return;
                }
            }

            SceneManager.LoadScene(2);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerReadyDictionary.Add(player, false);
            UpdatePLayerReady();
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        playerReadyDictionary.Remove(player);
        UpdatePLayerReady();
    }

    public void UpdatePLayerReady()
    {
        playerReady = 0;

        foreach (bool ready in playerReadyDictionary.Values)
        {
            if (ready)
            {
                playerReady++;
            }
        }

        playerReadyText.text = $"Player Ready {playerReady} / {playerReadyDictionary.Keys.Count}";
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    //{
    //    //SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
    //    TargetSetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
    //    Debug.Log("SetPlayerReadyServerRpc " + serverRpcParams.Receive.SenderClientId);
    //    playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

    //    bool allClientsReady = true;
    //    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    //    {
    //        if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
    //        {
    //            // This player is NOT ready
    //            allClientsReady = false;
    //            break;
    //        }
    //    }

    //    if (allClientsReady)
    //    {
    //        //OnGameStarting?.Invoke(this, EventArgs.Empty);
    //        Loader.LoadNetwork(Loader.Scene.MatchScene);
    //    }
    //}

    //[ClientRpc]
    //private void TargetSetPlayerReadyClientRpc(ulong clientId)
    //{
    //    // Handle setting player ready on client side here
    //    Debug.Log($"TargetSetPlayerReadyClientRpc called for client: {clientId}");
    //}
}
