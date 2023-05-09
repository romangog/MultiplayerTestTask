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
    private readonly ISceneLoadedEventInvokable _sceneLoadedInvokable;
    private readonly GameSettings _gameSettings;
    private readonly Prefabs _prefabs;
    private readonly NetworkRunner _runner;
    private readonly Player.Settings _playerSettings;

    public FusionManager(
        ICreateRoomInvokable createRoomCall,
        IJoinRoomInvokable joinRoomCall,
        INetworkSceneManager networkSceneManager,
        ISceneLoadedEventInvokable sceneLoadedInvokable,
        GameSettings gameSettings,
        Prefabs prefabs,
        NetworkRunner networkRunner,
        Player.Settings playerSettings)
    {
        this._createRoomCall = createRoomCall;
        this._joinRoomCall = joinRoomCall;
        this._networkSceneManager = networkSceneManager;
        this._sceneLoadedInvokable = sceneLoadedInvokable;
        this._gameSettings = gameSettings;
        this._prefabs = prefabs;
        this._runner = networkRunner;
        this._playerSettings = playerSettings;
    }

    public void Initialize()
    {
        _runner.AddCallbacks(this);
        GameObject.DontDestroyOnLoad(_runner.gameObject);

        _createRoomCall.CreateRoomCallEvent += OnCreateRoomCalled;
        _joinRoomCall.JoinRoomCallEvent += OnJoinRoomCalled;
    }

    private async void OnCreateRoomCalled(CreateRoomCallArgs args)
    {
        Debug.Log("Create room " + args.RoomName);

        _sceneLoadedInvokable.MapLoadedEvent += OnGameSceneLoaded;
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

    private void OnGameSceneLoaded(MapIndex arg)
    {

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
        switch ((MapIndex)(int)_runner.CurrentScene)
        {
            case MapIndex.Loading:

                break;
            case MapIndex.Lobby:
                break;
            case MapIndex.Game:
                Debug.Log("Game loaded");
                break;
            default:
                break;
        }
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
