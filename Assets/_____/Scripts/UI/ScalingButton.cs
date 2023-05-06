using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SimpleTouchGraphic))]
public class ScalingButton : MonoBehaviour
{
    private SimpleTouchGraphic _button;
    private RectTransform _buttonTransform;

    void Start()
    {
        _buttonTransform = GetComponent<RectTransform>();
        _button = GetComponent<SimpleTouchGraphic>();

        _button.PointerDownEvent += OnPointerDown;
        _button.PointerUpEvent += OnPointerUp;
    }

    private void OnPointerDown()
    {
        _buttonTransform.DOScale(0.75f, 0.2f).SetEase(Ease.OutBack);
    }

    private void OnPointerUp()
    {
        _buttonTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
}
