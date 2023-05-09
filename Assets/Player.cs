using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Fusion;
using System;

public class Player : NetworkBehaviour
{
    public Action<CollectableCoin> PlayerCollectedCoinEvent;

    [Networked(OnChanged = nameof(OnNameChanged))] public NetworkString<_32> Name { get; set; }
    [Networked(OnChanged = nameof(OnCollectedCoinsChanged))] public int CollectedCoins { get; set; }

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
        EventBus.PlayerNameChangedEvent?.Invoke(changed.Behaviour);
    }

    private static void OnCollectedCoinsChanged(Changed<Player> changed)
    {
        EventBus.PlayerCollectedCoinsChangedEvent?.Invoke(changed.Behaviour);
    }

    public override void Spawned()
    {
        EventBus.PlayerSpawnedEvent?.Invoke(this);
    }

    public void Activate()
    {
        ResetFireTimer();
    }



    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_CALL(CollectableCoin coinObj)
    {
        Debug.Log("RPC RECIEVED AT STATE AUTHORYTY");
        //PlayerCollectedCoinEvent?.Invoke(coinObj);
    }

    public override void FixedUpdateNetwork()
    {
        //if (!Runner.IsServer) return;
        //if (!Object.HasStateAuthority) return;
        if (Runner.TryGetInputForPlayer<PlayerInput>(Object.InputAuthority, out PlayerInput input))
        {
            Vector3 inputVector = new Vector3(input.Horizontal, input.Vertical, 0);
            Vector3 movement = inputVector * _settings.MoveSpeed * Runner.DeltaTime;
            if (inputVector.sqrMagnitude > 0.1f)
            {
                Move(movement);
                Rotate(inputVector);
            }


            if (Object.HasStateAuthority)
            {
                if (_fireTimer.ExpiredOrNotRunning(Runner))
                {
                    ResetFireTimer();
                    //RPC_Fire(movement);
                    Fire(inputVector);
                }
            }
        }
        if (Object.HasStateAuthority)
            CollectCoins();
    }

    public void Move(Vector3 movement)
    {
        this.transform.position += movement;
    }

    private void Rotate(Vector3 inputVector)
    {
        _lookRotatingRoot.transform.rotation = Quaternion.LookRotation(inputVector, Vector3.back);
    }

    private void CollectCoins()
    {
        List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
        Runner.LagCompensation.OverlapSphere(
            _hitboxRoot.Hitboxes[0].transform.position,
            _hitboxRoot.Hitboxes[0].SphereRadius,
            Object.InputAuthority,
            hits,
            _settings.CollectableCoinsMask);

        foreach (var hit in hits)
        {
            var coinObject = hit.GameObject.GetComponent<CollectableCoin>();
            //Debug.Log("Coin collected before checking: " + coinObject.IsCollected);
            //Debug.Log("Coin id: " + coinObject.Object.Id);
            if (coinObject.IsCollected) continue;
            //Debug.Log("Hit coin, collect");
            //Debug.Log("coin pos: " + hit.Point);
            coinObject.IsCollected = true;
            CollectedCoins++;
            //Runner.Despawn(coinObject.Object);
            //RPC_CollectCoin(coinObject);
        }
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

    //[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    //public void RPC_Fire(Vector3 direction)
    //{
    //    Fire(direction);
    //}

    private void ResetFireTimer()
    {
        _fireTimer = TickTimer.CreateFromSeconds(Runner, 60f / _settings.ShotsPerMinute);
    }

    [Serializable]
    public class Settings
    {
        public float ShotsPerMinute;
        public float MoveSpeed;
        public LayerMask CollectableCoinsMask;
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


