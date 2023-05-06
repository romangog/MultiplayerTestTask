using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleTweenAnim : TweenAnim
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;

    public override void Play()
    {
        _tween?.Kill();
        _tween = _targetTransform.DOScale(_targetScale, _time).SetEase(_ease);
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
