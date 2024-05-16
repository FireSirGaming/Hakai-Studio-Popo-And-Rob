using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Multiplay;
using UnityEngine;

public class PlayerReady : MonoBehaviour
{
    public static event EventHandler OnInstanceCreated;

    public static void ResetStaticData()
    {
        OnInstanceCreated = null;
    }

    public static PlayerReady Instance { get; private set; }

    private async void Start()
    {
#if DEDICATED_SERVER
        Debug.Log("DEDICATED_SERVER Player Ready");

        Debug.Log("ReadyServerForPlayersAsync");
        await MultiplayService.Instance.ReadyServerForPlayersAsync();

        Camera.main.enabled = false;
#endif
    }
}
