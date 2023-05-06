using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{

    private TutorialExtention[] _extentions;
    private bool _IsInitialized;

    internal void Initialize()
    {
        if (_IsInitialized) return;

        _IsInitialized = true;
        _extentions = this.GetComponents<TutorialExtention>();

        foreach (var ext in _extentions)
        {
            ext.Setup();
        }
    }


    private void OnEnable()
    {
        foreach (var ext in _extentions)
        {
            ext.TurnOn();
        }
    }

    private void OnDisable()
    {
        foreach (var ext in _extentions)
        {
            ext.TurnOff();
        }
    }
}
