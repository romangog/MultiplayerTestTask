using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Zenject;
using UnityEngine.SceneManagement;

public class FusionManager : INetworkRunnerCallbacks, IInitializable
{
    private readonly ICreateRoomInvokable _createRoomCall;
    private readonly IJoinRoomInvokable _joinRoomCall;
    private readonly INetworkSceneManager _networkSceneManager;
    private readonly GameSettings _gameSettings;

    private NetworkRunner _runner;

    public FusionManager(
        ICreateRoomInvokable createRoomCall,
        IJoinRoomInvokable joinRoomCall,
        INetworkSceneManager networkSceneManager,
        GameSettings gameSettings)
    {
        this._createRoomCall = createRoomCall;
        this._joinRoomCall = joinRoomCall;
        this._networkSceneManager = networkSceneManager;
        this._gameSettings = gameSettings;
    }

    public void Initialize()
    {
        _runner = new GameObject("Runner").AddComponent<NetworkRunner>();
        _runner.AddCallbacks(this);
        GameObject.DontDestroyOnLoad(_runner.gameObject);

        _createRoomCall.CreateRoomCallEvent += OnCreateRoomCalled;
        _joinRoomCall.JoinRoomCallEvent += OnJoinRoomCalled;
    }

    private async void OnCreateRoomCalled(CreateRoomCallArgs args)
    {
        Debug.Log("Create room " + args.RoomName);

        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = args.RoomName,
            SceneManager = _networkSceneManager,
            GameMode = GameMode.Host,
        });

        if (result.Ok)
        {
            Debug.Log("Ok");
        }
        else
        {
            Debug.Log("Failed");
        }

    }

    private async void OnJoinRoomCalled(JoinRoomCallArgs args)
    {
        Debug.Log("Join room " + args.RoomName);

        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = args.RoomName,
            SceneManager = _networkSceneManager,
            GameMode = GameMode.Client,
        });

        if (result.Ok)
        {
            Debug.Log("Ok");
        }
        else
        {
            Debug.Log("Failed");
        }
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

    public void OnDisconnectedFromServer(NetworkRunner runner)
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

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerJoined: " + runner.IsClient);

        _runner.SetActiveScene((int)MapIndex.Game);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
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

public interface ISceneLoader
{
    public void SwitchScene();
}
