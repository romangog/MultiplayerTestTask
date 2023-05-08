using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using DG.Tweening;
using Zenject;

public class GameSession : NetworkBehaviour, INetworkRunnerCallbacks
{
    private IObjectPool<Projectile> _projectilesPool;
    private Prefabs _prefabs;
    private Player.Settings _playerSettings;
    private IPlayerCoinsDisplayable _playerCoinsDisplay;
    private IPlayerNameDisplayable _playerNameDisplay;
    private IOnPlayerJoinedInitializable[] _playerJoinedInitializers;
    private Joystick _joystick;

    [Networked(OnChanged = nameof(UpdateNumberText))] public int Number { get; set; }

    private Dictionary<PlayerRef, Player> _players = new Dictionary<PlayerRef, Player>();

    [Inject]
    private void Construct(IObjectPool<Projectile> projectilesPool,
        Prefabs prefabs,
        Player.Settings playerSettings,
        IPlayerCoinsDisplayable playerCoinsDisplayable,
        IPlayerNameDisplayable playerNameDisplayable,
        IOnPlayerJoinedInitializable [] playerJoinedInitializable,
        Joystick joystick)
    {
        _projectilesPool = projectilesPool;
        _prefabs = prefabs;
        _playerSettings = playerSettings;
        _playerCoinsDisplay = playerCoinsDisplayable;
        _playerNameDisplay = playerNameDisplayable;
        _playerJoinedInitializers = playerJoinedInitializable;
        _joystick = joystick;
        EventBus.PlayerSpawnedEvent += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(Player player)
    {
        player.Construct(_playerSettings, _projectilesPool);
        player.Activate();
    }

    public static void UpdateNumberText(Changed<GameSession> changed)
    {
        ScreenLog.Log("Number", changed.Behaviour.Number.ToString());
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
        Object.AssignInputAuthority(Runner.LocalPlayer);

        foreach (var init in _playerJoinedInitializers)
        {
            init.InitializeForLocalPlayer();
        }

        if (Object.HasStateAuthority)
        {
            foreach (var player in Runner.ActivePlayers)
            {
                SpawnPlayer(player);
            }

        }
        else
        {

        }

        if (Object.HasStateAuthority)
            DOTween.Sequence()
                .PrependInterval(3f)
                .AppendCallback(ChangeNumber)
                .SetLoops(-1);

        // Deprecated
    }

    private void SpawnPlayer(PlayerRef player)
    {
        var playerView = Runner.Spawn(_prefabs.PlayerView, inputAuthority: player);
        Runner.SetPlayerObject(player, playerView.Object);
        _players.Add(player, playerView);
        
        playerView.PlayerCollectedCoinEvent += () => OnPlayerCollectedCoin(player);
        playerView.Name = "Player" +  _players.Count.ToString();
        RPC_UpdatePlayerNameInfo();
    }

    // StateAuthority
    private void OnPlayerCollectedCoin(PlayerRef player)
    {
        _players[player].CollectedCoins++;
        RPC_UpdatePlayerCoinsInfo();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.InputAuthority)]
    public void RPC_UpdatePlayerCoinsInfo()
    {
        _playerCoinsDisplay.SetCoins(_players[Object.InputAuthority].CollectedCoins);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.InputAuthority)]
    public void RPC_UpdatePlayerNameInfo()
    {
        _playerNameDisplay.SetName(_players[Object.InputAuthority].Name.Value);
    }

    private void ChangeNumber()
    {
        Number = UnityEngine.Random.Range(0, 1000);
    }

    public void SceneLoadDone()
    {
        switch ((MapIndex)(int)Runner.CurrentScene)
        {
            case MapIndex.Loading:

                break;
            case MapIndex.Lobby:
                break;
            case MapIndex.Game:
                break;
            default:
                break;
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (Object.HasStateAuthority)
            SpawnPlayer(player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {


    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerInput plInput = new PlayerInput();
        plInput.Horizontal = _joystick.Horizontal;
        plInput.Vertical = _joystick.Vertical;
        plInput.Buttons.Set(PlayerButtons.Fire, Input.GetKeyDown(KeyCode.Space));
        input.Set(plInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {


    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
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
}
