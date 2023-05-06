using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFadeTweenAnim : TweenAnim
{
    [SerializeField] private SpriteRenderer _graphic;
    [SerializeField] private float _targetFade;
    [SerializeField] private float _time;

    public override void Play()
    {
        _tween = _graphic.DOFade(_targetFade, _time).SetEase(Ease.Linear);
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
