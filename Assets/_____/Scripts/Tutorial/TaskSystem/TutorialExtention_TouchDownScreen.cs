using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialExtention_TouchDownScreen : TutorialExtention
{
    internal static TutorialExtention_TouchDownScreen Instance;

    internal UnityEvent ButtonTouchedDownEvent { get; private set; } = new UnityEvent();

    [SerializeField] private float _continueDelayTime;
    [SerializeField] private GameObject _continueScreen;

    private float _timer;

    internal override void Setup()
    {
        base.Setup();
    }

    private void Update()
    {
        if (!_IsOn) return;

        if (Input.GetMouseButtonDown(0))
        {
            _continueScreen.SetActive(false);
            ButtonTouchedDownEvent.Invoke();
        }

    }


    internal override void TurnOn()
    {
        base.TurnOn();
        Instance = this;
        _continueScreen.SetActive(true);

    }

    internal override void TurnOff()
    {
        base.TurnOff();
        _continueScreen.SetActive(false);
    }
}
