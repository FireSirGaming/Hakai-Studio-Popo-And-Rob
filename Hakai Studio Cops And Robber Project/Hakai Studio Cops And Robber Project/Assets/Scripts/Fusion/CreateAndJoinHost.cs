using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateAndJoinHost : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    private NetworkRunner networkRunner;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(() =>
        {
            if (networkRunner == null)
            {
                RoomCreate(GameMode.Host);
            }

        });

        joinRoomButton.onClick.AddListener(() =>
        {
            if (networkRunner == null)
            {
                RoomCreate(GameMode.Client);
            }
        });
    }

    async void RoomCreate(GameMode mode)
    {
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;

        var waitingScene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex + 1);
        var waitSceneInfo = new NetworkSceneInfo();

        if (waitingScene.IsValid)
        {
            waitSceneInfo.AddSceneRef(waitingScene, LoadSceneMode.Additive);
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestWaiting",
            Scene = waitingScene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
