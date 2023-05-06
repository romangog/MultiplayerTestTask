using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingFadeTweenAnim : TweenAnim
{
    [SerializeField] private Graphic _graphic;
    [SerializeField] private float _fadeMin;
    [SerializeField] private float _fadeMax;
    [SerializeField] private float _time;
    [SerializeField] private bool _repeating;

    private Sequence _seq;

    public override void Play()
    {
        _graphic.color = new Color(0, 0, 0, _fadeMin);
        _tween = DOTween.Sequence();
        _seq = (Sequence) _tween;
        _seq.Append(_graphic.DOFade(_fadeMax, _time/2f).SetEase(Ease.Linear));
        _seq.Append(_graphic.DOFade(_fadeMin, _time/2f).SetEase(Ease.Linear));

        if(_repeating)
        {
            _seq.SetLoops(-1);
        }
    }

    public override void Stop()
    {
        _tween?.Kill();
    }
}
