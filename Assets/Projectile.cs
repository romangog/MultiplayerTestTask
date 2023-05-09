using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion.LagCompensation;
using System;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private NetworkTransform _transform;
    private float _speed;
    private PlayerRef _senderPlayer;
    private LayerMask _hittableLayerMask;
    private TickTimer _timer;

    public void Construct(LayerMask hittbleLayerMask, float speed)
    {
        _hittableLayerMask = hittbleLayerMask;
        _speed = speed;
    }

    public void Shoot(PlayerRef sender)
    {
        _senderPlayer = sender;
        _timer = TickTimer.CreateFromSeconds(Runner, 10f);
    }

    public override void FixedUpdateNetwork()
    {
        if(_timer.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(this.Object);
            return;
        }
        _transform.Transform.position += transform.forward * _speed * Runner.DeltaTime;
        List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
        Runner.LagCompensation.RaycastAll(this.transform.position, this.transform.forward, _speed * Runner.DeltaTime, _senderPlayer, hits, layerMask: _hittableLayerMask);

        foreach (var hit in hits)
        {
            if(hit.GameObject.transform.parent.TryGetComponent(out Player player))
            {
                if(player.Object.InputAuthority != _senderPlayer)
                {
                    player.OnHit();
                    this.gameObject.SetActive(false);
                    Runner.Despawn(this.Object);
                }
                break;
            }
        }

    }
    [Serializable]
    public class Settings
    {
        public LayerMask HittableLayerMask;
        public float Speed;
    }
}
