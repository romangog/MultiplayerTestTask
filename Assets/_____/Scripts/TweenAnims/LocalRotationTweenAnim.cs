using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotationTweenAnim : TweenAnim
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 _eulers;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _time;

    public override void Play()
    {
        _tween = _transform.DOLocalRotate(_eulers, _time).SetEase(_ease);
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
