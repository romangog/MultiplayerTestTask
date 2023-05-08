using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Fusion;
using System;

public class Player : NetworkBehaviour
{
    public Action PlayerCollectedCoinEvent;

    [Networked(OnChanged = nameof(OnNameChanged))] public NetworkString<_32> Name { get; set; }


    [Networked] public int CollectedCoins { get; set; }
    [Networked] public float Health { get; set; }

    [SerializeField] private HitboxRoot _hitboxRoot;
    [SerializeField] private NetworkTransform _lookRotatingRoot;
    [SerializeField] private Transform _bulletSpawnPoint;

    private Settings _settings;
    private IObjectPool<Projectile> _projectilesPool;
    private TickTimer _fireTimer;

    public void Construct(
        Settings settings,
        IObjectPool<Projectile> projectilesPool)
    {
        _settings = settings;
        _projectilesPool = projectilesPool;
        CollectedCoins = 0;
        Health = 1f;
    }

    private static void OnNameChanged(Changed<Player> changed)
    {
        changed.Behaviour.gameObject.name = changed.Behaviour.Name.Value;
    }

    public override void Spawned()
    {
        EventBus.PlayerSpawnedEvent?.Invoke(this);
    }

    public void Activate()
    {
        ResetFireTimer();
    }

    public void Move(Vector3 movement)
    {
        this.transform.position += movement;
        _lookRotatingRoot.transform.rotation = Quaternion.LookRotation(movement, Vector3.back);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_CollectCoin()
    {
        PlayerCollectedCoinEvent?.Invoke();
    }

    public override void FixedUpdateNetwork()
    {
        //if (!Runner.IsServer) return;
        if (Runner.TryGetInputForPlayer<PlayerInput>(Object.InputAuthority, out PlayerInput input))
        {
            Vector3 movement = new Vector3(input.Horizontal, input.Vertical, 0) * _settings.MoveSpeed * Runner.DeltaTime;
            if (movement.sqrMagnitude > 0)
                Move(movement);
            if (_fireTimer.ExpiredOrNotRunning(Runner))
            {
                ResetFireTimer();
                RPC_Fire(movement);
            }
        }

        CollectCoins();
    }

    private void CollectCoins()
    {
        bool collectedCoin = false;
        //Runner.LagCompensation.OverlapSphere(
        //    _hitboxRoot.Hitboxes[0].transform.position,
        //    _hitboxRoot.Hitboxes[0].BoxExtents,
        //    Quaternion.identity)

        if (collectedCoin)
            PlayerCollectedCoinEvent?.Invoke();
    }

    internal void OnHit()
    {
        Health -= 0.1f;
        Debug.Log(name + " Hit");
    }

    private void Fire(Vector3 direction)
    {
        Projectile projectile = _projectilesPool.Get();
        projectile.transform.rotation = _bulletSpawnPoint.rotation;
        projectile.transform.position = _bulletSpawnPoint.position;
        projectile.Shoot(Object.InputAuthority);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_Fire(Vector3 direction)
    {
        Fire(direction);
    }

    private void ResetFireTimer()
    {
        _fireTimer = TickTimer.CreateFromSeconds(Runner, 60f / _settings.ShotsPerMinute);
    }

    [Serializable]
    public class Settings
    {
        public float ShotsPerMinute;
        public float MoveSpeed;
    }
}

public enum PlayerButtons
{
    Fire = 0
}

public struct PlayerInput : INetworkInput
{
    public float Vertical;
    public float Horizontal;
    public NetworkButtons Buttons;
}


