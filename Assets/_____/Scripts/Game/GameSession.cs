using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using DG.Tweening;
using Zenject;
using System.Linq;

public class GameSession : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Transform[] _spawnPoints;
    private IObjectPool<Projectile> _projectilesPool;
    private Prefabs _prefabs;
    private Player.Settings _playerSettings;
    private IPlayerCoinsDisplayable _playerCoinsDisplay;
    private IPlayerNameDisplayable _playerNameDisplay;
    private IOnPlayerJoinedInitializable[] _playerJoinedInitializers;
    private IPlayerHealthDisplayable _playerHealthDisplayable;
    private IWinPopupDisplayable _winPopupDisplayable;
    private IGameStatusDisplayable _gameStatusDisplayable;
    private Joystick _joystick;

    private Dictionary<PlayerRef, Player> _players = new Dictionary<PlayerRef, Player>();

    [Networked] private TickTimer _gameStartTimer { get; set; }

    private bool _IsGameStarted;

    [Inject]
    private void Construct(IObjectPool<Projectile> projectilesPool,
        Prefabs prefabs,
        Player.Settings playerSettings,
        IPlayerCoinsDisplayable playerCoinsDisplayable,
        IPlayerNameDisplayable playerNameDisplayable,
        IPlayerHealthDisplayable playerHealthDisplayable,
        IOnPlayerJoinedInitializable[] playerJoinedInitializable,
        IWinPopupDisplayable winPopupDisplayable,
        IGameStatusDisplayable gameStatusDisplayable,
        Joystick joystick)
    {
        _projectilesPool = projectilesPool;
        _prefabs = prefabs;
        _playerSettings = playerSettings;
        _playerCoinsDisplay = playerCoinsDisplayable;
        _playerNameDisplay = playerNameDisplayable;
        _playerJoinedInitializers = playerJoinedInitializable;
        _playerHealthDisplayable = playerHealthDisplayable;
        _winPopupDisplayable = winPopupDisplayable;
        _gameStatusDisplayable = gameStatusDisplayable;
        _joystick = joystick;
        EventBus.PlayerSpawnedEvent += OnPlayerSpawned;
        EventBus.PlayerNameChangedEvent += OnPlayerNameChanged;
        EventBus.PlayerCollectedCoinsChangedEvent += OnPlayerCollectedCoinsChanged;
        EventBus.CoinCollectedEvent += OnCoinCollected;
        EventBus.PlayerHealthChangedEvent += OnHealthChanged;
        EventBus.PlayerDiedEvent += OnPlayerDied;
    }

    public override void FixedUpdateNetwork()
    {

        if (_gameStartTimer.IsRunning && !_IsGameStarted)
        {
            _gameStatusDisplayable.DisplayGameStatus($"Game starts in {Mathf.RoundToInt(_gameStartTimer.RemainingTime(Runner).Value)}...");
        }

        if (_gameStartTimer.Expired(Runner) && !_IsGameStarted)
        {
            Debug.LogWarning("Display zero status");
            _gameStatusDisplayable.DisplayGameStatus("");
            if (Object.HasStateAuthority)
                StartGame();
            _IsGameStarted = true;
        }
    }

    private void OnPlayerSpawned(Player player)
    {
        player.Construct(_playerSettings, _projectilesPool);

        if (_IsGameStarted)
            player.Activate();
    }

    public void OnPlayerNameChanged(Player player)
    {
        if (player.Object.InputAuthority == Runner.LocalPlayer)
            _playerNameDisplay.SetName(player.Name.Value);
    }

    private void OnPlayerCollectedCoinsChanged(Player player)
    {
        if (player.Object.InputAuthority == Runner.LocalPlayer)
            _playerCoinsDisplay.SetCoins(player.CollectedCoins);
    }

    private void OnHealthChanged(Player player)
    {
        if (player.Object.InputAuthority == Runner.LocalPlayer)
            _playerHealthDisplayable.SetHealth(player.Health);
    }

    private void OnCoinCollected(CollectableCoin coin)
    {
        coin.gameObject.SetActive(false);
        if (Object.HasStateAuthority)
            Runner.Despawn(coin.Object);
    }

    private void OnPlayerDied(Player player)
    {
        player.IsAlive = false;
        if (!Object.HasStateAuthority) return;
        _players.Remove(player.Object.InputAuthority);
        var alivePlayers = _players.Where(x => x.Value.IsAlive);
        if (alivePlayers.Count() == 1)
        {
            var winner = alivePlayers.First().Value;
            RPC_EndGame(winner);
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_EndGame(Player winner)
    {
        _winPopupDisplayable.DisplayWinnerInfo(winner.name, winner.CollectedCoins, _playerSettings.SpriteColors[winner.ColorIndex]);
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
    }

    private void StartGame()
    {
        foreach (var player in _players)
        {
            player.Value.Activate();
        }
        // Spawn Coins
        for (int i = 0; i < 11; i++)
        {
            Vector3 position = new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-8f, 8f), 0f);
            var coin = Runner.Spawn(_prefabs.Coin, position: position);
        }
    }

    private void SpawnPlayer(PlayerRef player)
    {
        Vector3 spawnPoint = _spawnPoints[_players.Count % _spawnPoints.Length].position;
        var playerView = Runner.Spawn(_prefabs.PlayerView, inputAuthority: player, position: spawnPoint);
        Runner.SetPlayerObject(player, playerView.Object);
        playerView.ColorIndex = _players.Count % _spawnPoints.Length;
        playerView.Name = "Player" + _players.Count.ToString();
        playerView.Health = _playerSettings.MaxHealth;
        playerView.CollectedCoins = 0;
        playerView.IsAlive = true;
        _players.Add(player, playerView);
        if (_players.Count >= 2)
        {
            StartGameTimer();
        }
        else
        {
            _gameStatusDisplayable.DisplayGameStatus($"Waiting for a second player");
        }
    }

    private void StartGameTimer()
    {
        _gameStartTimer = TickTimer.CreateFromSeconds(Runner, 3f);
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
