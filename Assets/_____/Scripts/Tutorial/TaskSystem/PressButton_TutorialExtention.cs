using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PressButton_TutorialExtention : TutorialExtention
{
    internal static PressButton_TutorialExtention Instance;

    internal UnityEvent ButtonPressedEvent { get; private set; } = new UnityEvent();
    [SerializeField] private Button _button;

    internal override void Setup()
    {
        base.Setup();
        _button.onClick.AddListener(ButtonPressedEvent.Invoke);
    }

    internal override void TurnOn()
    {
        base.TurnOn();
        Instance = this;

    }

    internal override void TurnOff()
    {
        base.TurnOff();
    }
}
