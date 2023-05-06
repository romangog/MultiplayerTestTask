using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeTweenAnim : TweenAnim
{
    [SerializeField] private Graphic _graphic;
    [SerializeField] private float _targetFade;
    [SerializeField] private float _time;

    public override void Play()
    {
        _tween?.Kill();
        _tween = _graphic.DOFade(_targetFade, _time).SetEase(Ease.Linear);
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
