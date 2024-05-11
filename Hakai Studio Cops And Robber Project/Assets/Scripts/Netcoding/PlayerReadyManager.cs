using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Multiplay;
using UnityEngine;

public class PlayerReadyManager : NetworkBehaviour
{
    //public static event EventHandler OnInstanceCreated;

    //public static void ResetStaticData()
    //{
    //    OnInstanceCreated = null;
    //}

    public static PlayerReadyManager Instance { get; private set; }

    //public event EventHandler OnReadyChanged;
    //public event EventHandler OnGameStarting;


    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();

        //OnInstanceCreated?.Invoke(this, EventArgs.Empty);
    }

    private async void Start()
    {
#if DEDICATED_SERVER
        Debug.Log("DEDICATED_SERVER Player Ready");

        Debug.Log("ReadyServerForPlayersAsync");
        await MultiplayService.Instance.ReadyServerForPlayersAsync();

        Camera.main.enabled = false;
#endif
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        TargetSetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        Debug.Log("SetPlayerReadyServerRpc " + serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            //OnGameStarting?.Invoke(this, EventArgs.Empty);
            Loader.LoadNetwork(Loader.Scene.MatchScene);
        }
    }

    [ClientRpc]
    private void TargetSetPlayerReadyClientRpc(ulong clientId)
    {
        // Handle setting player ready on client side here
        Debug.Log($"TargetSetPlayerReadyClientRpc called for client: {clientId}");
    }

    public void SetTest()
    {
        Debug.Log("Ping");
        TestServerRpc();
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        TestClientRpc(serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ulong client)
    {
        Debug.Log("Pong");
    }

    //[ClientRpc]
    //private void SetPlayerReadyClientRpc(ulong clientId)
    //{
    //    playerReadyDictionary[clientId] = true;

    //    //OnReadyChanged?.Invoke(this, EventArgs.Empty);
    //}


    //public bool IsPlayerReady(ulong clientId)
    //{
    //    return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    //}
}
