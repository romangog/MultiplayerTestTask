using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlinkScaleTweenAnim : TweenAnim
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _endScale;
    [SerializeField] private float _time;
    [SerializeField] private bool _repeating;
    [SerializeField] private float _firstProportion;

    private Sequence _seq;

    public override void Play()
    {
        _transform.localScale = _startScale;
        _tween = DOTween.Sequence();
        _seq = (Sequence)_tween;
        _seq.Append(_transform.DOScale(_endScale, _time * _firstProportion).SetEase(Ease.OutQuad));
        _seq.Append(_transform.DOScale(_startScale, _time * (1f- _firstProportion)).SetEase(Ease.InQuad));

        if (_repeating)
        {
            _seq.SetLoops(-1);
        }
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
