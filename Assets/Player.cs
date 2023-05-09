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
    [Networked(OnChanged = nameof(OnHealthChanged))] public float Health { get; set; }
    [Networked(OnChanged = nameof(OnColorChanged))] public int ColorIndex { get; set; }
    [Networked(OnChanged = nameof(OnAliveChanged))] public NetworkBool IsAlive { get; set; }

    [Networked] private bool _IsActive { get; set; }
    [Networked] private TickTimer _fireTimer { get; set; }

    [SerializeField] private HitboxRoot _hitboxRoot;
    [SerializeField] private NetworkTransform _lookRotatingRoot;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Settings _settings;
    private IObjectPool<Projectile> _projectilesPool;


    public void Construct(
        Settings settings,
        IObjectPool<Projectile> projectilesPool)
    {
        _settings = settings;
        _projectilesPool = projectilesPool;
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

    private static void OnHealthChanged(Changed<Player> changed)
    {
        EventBus.PlayerHealthChangedEvent?.Invoke(changed.Behaviour);
    }

    private static void OnColorChanged(Changed<Player> changed)
    {
        var player = changed.Behaviour;
        player._spriteRenderer.color = player._settings.SpriteColors[player.ColorIndex];
    }

    private static void OnAliveChanged(Changed<Player> changed)
    {
        var player = changed.Behaviour;
        player.gameObject.SetActive(player.IsAlive);
    }

    public override void Spawned()
    {
        EventBus.PlayerSpawnedEvent?.Invoke(this);
    }

    public void Activate()
    {
        ResetFireTimer();
        _IsActive = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsAlive && _IsActive) return;
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
                    Fire(inputVector);
                }
            }
        }
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
            coinObject.gameObject.SetActive(false);
            if (coinObject.IsCollected) continue;
            if (Object.HasStateAuthority)
            {
                coinObject.IsCollected = true;
                CollectedCoins++;
            }
        }
    }

    internal void OnHit()
    {
        Health = Mathf.MoveTowards(Health, 0f, _settings.Damage);
        if (Health == 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        EventBus.PlayerDiedEvent?.Invoke(this);
    }

    private void Fire(Vector3 direction)
    {
        Projectile projectile = _projectilesPool.Get();
        projectile.transform.rotation = _bulletSpawnPoint.rotation;
        projectile.transform.position = _bulletSpawnPoint.position;
        projectile.Shoot(Object.InputAuthority);
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
        public float Damage;
        public float MaxHealth;
        public LayerMask CollectableCoinsMask;
        public Color[] SpriteColors;
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


