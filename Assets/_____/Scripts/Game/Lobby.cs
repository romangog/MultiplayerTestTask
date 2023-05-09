using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Zenject;


public class Lobby : INetworkRunnerCallbacks, IInitializable
{
    private readonly ICreateRoomInvokable _createRoomCall;
    private readonly IJoinRoomInvokable _joinRoomCall;
    private readonly INetworkSceneManager _networkSceneManager;
    private readonly IConnectionInteractionBlockable _connectionInteractionBlockable;
    private readonly IConnectionInfoDisplayable _connectionInfoDisplayable;
    private NetworkRunner _runner;

    public Lobby(
        ICreateRoomInvokable createRoomCall,
        IJoinRoomInvokable joinRoomCall,
        INetworkSceneManager networkSceneManager,
        ISceneLoadedEventInvokable sceneLoadedInvokable,
        IConnectionInteractionBlockable connectionInteractionBlockable,
        IConnectionInfoDisplayable connectionInfoDisplayable)
    {
        this._createRoomCall = createRoomCall;
        this._joinRoomCall = joinRoomCall;
        this._networkSceneManager = networkSceneManager;
        this._connectionInteractionBlockable = connectionInteractionBlockable;
        this._connectionInfoDisplayable = connectionInfoDisplayable;
    }

    public void Initialize()
    {
        InitRunner();

        _createRoomCall.CreateRoomCallEvent += OnCreateRoomCalled;
        _joinRoomCall.JoinRoomCallEvent += OnJoinRoomCalled;
        _connectionInteractionBlockable.CancelConnectionCallEvent += OnConnectionCancelled;
    }

    public void InitRunner()
    {
        _runner = new GameObject("Runner").AddComponent<NetworkRunner>();
        _runner.AddCallbacks(this);
        GameObject.DontDestroyOnLoad(_runner.gameObject);
    }

    private async void OnCreateRoomCalled(CreateRoomCallArgs args)
    {
        _connectionInteractionBlockable.BlockInteraction();
        _connectionInfoDisplayable.DisplayConnectionInfo($"Creating room \"{args.RoomName}\"...");
        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = args.RoomName,
            SceneManager = _networkSceneManager,
            GameMode = GameMode.Host
        });
    }

    private async void OnJoinRoomCalled(JoinRoomCallArgs args)
    {
        _connectionInteractionBlockable.BlockInteraction();
        _connectionInfoDisplayable.DisplayConnectionInfo($"Joining room \"{args.RoomName}\"...");
        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = args.RoomName,
            SceneManager = _networkSceneManager,
            GameMode = GameMode.Client,
        });
    }

    private void OnConnectionCancelled()
    {
        // Simplest (and the only possible?) way to interrupt connection attempt
        // https://forum.photonengine.com/discussion/19991/is-there-a-way-to-handle-re-connection-in-fusion-or-do-we-have-to-handle-it-manually
        _runner.Shutdown();
        InitRunner();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        _runner.SetActiveScene((int)MapIndex.Game);
    }

    #region UNUSED_CALLBACKS

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
    #endregion
}
