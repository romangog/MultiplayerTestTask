using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendLocalMoveBlinkingTweenAnim : TweenAnim
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 _move;
    [SerializeField] private float _inTime;
    [SerializeField] private float _outTime;
    [SerializeField] private Ease _startEase;
    [SerializeField] private Ease _endEase;

    private Sequence _seq;

    public override void Play()
    {
        _tween = DOTween.Sequence();
        _seq = (Sequence)_tween;
        _seq.Append(_transform.DOBlendableLocalMoveBy(_move, _inTime).SetEase(_startEase));
        _seq.Append(_transform.DOBlendableLocalMoveBy(-_move, _outTime).SetEase(_endEase));
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
