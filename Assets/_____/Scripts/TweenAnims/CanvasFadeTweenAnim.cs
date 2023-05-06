using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasFadeTweenAnim : TweenAnim
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _targetFade;
    [SerializeField] private float _time;
    [SerializeField] private float _delay;

    public override void Play()
    {
        _tween = _canvasGroup.DOFade(_targetFade, _time).SetDelay(_delay);
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
