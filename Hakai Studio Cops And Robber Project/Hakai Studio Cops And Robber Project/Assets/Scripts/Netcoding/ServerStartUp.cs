using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplay;
using Unity.Services.Core;
using UnityEngine;
using System;

public class ServerStartUp : MonoBehaviour
{
    private const string _internalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;

    private IMultiplayService _multiplayService;

    void Start()
    {
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                server = true;
            }
            if (args[i] == "-port" && ((i + 1) < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i + 1]);
            }
        }
        if (server)
        {
            StartServer();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_internalServerIP, _serverPort);
        NetworkManager.Singleton.StartServer();
    }

    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();
        try
        {
            _multiplayService = MultiplayService.Instance;
            //await _multiplayService.StartServerQueryHandlerAsync();
        }
        catch (Exception ex)
        {

        }
    }
}
