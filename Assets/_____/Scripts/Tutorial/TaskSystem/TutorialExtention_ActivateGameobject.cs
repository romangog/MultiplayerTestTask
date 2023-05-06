using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExtention_ActivateGameobject : TutorialExtention
{
    [SerializeField] private GameObject [] _objects;

    internal override void TurnOn()
    {
        base.TurnOn();
        foreach (var item in _objects)
        {
            item.SetActive(true);
        }
    }

    internal override void TurnOff()
    {
        base.TurnOff();
        foreach (var item in _objects)
        {
            item.SetActive(false);
        }
    }
}
