using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExtention : MonoBehaviour
{
    protected bool _IsOn;
    internal virtual void Setup()
    {

    }

    internal virtual void TurnOn()
    {
        _IsOn = true;
    }

    internal virtual void TurnOff()
    {
        _IsOn = false;
    }
}
