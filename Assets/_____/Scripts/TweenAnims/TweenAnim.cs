using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TweenAnim : MonoBehaviour
{
    protected Tween _tween;

    public abstract void Play();

    public abstract void Stop();
}
