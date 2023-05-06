using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenView : MonoBehaviour
{
    [SerializeField] private CanvasFadeTweenAnim _canvasFadeZero;
    [SerializeField] private CanvasFadeTweenAnim _canvasFadeOne;
    [SerializeField] private GameObject _raycastBlocker;

    internal void Show(Action callback)
    {
        _raycastBlocker.SetActive(true);
        _canvasFadeOne.Play();

        AsyncUtility.DelayTask(1f, callback);
    }

    internal void Hide()
    {
        _raycastBlocker.SetActive(false);
        _canvasFadeZero.Play();
    }
}
