using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition_TutorialExtention : TutorialExtention
{
    internal static TargetPosition_TutorialExtention Instance;
    private GameObject _targetObject;

    internal override void TurnOn()
    {
        base.TurnOn();
        Instance = this;
        //_targetObject = Instantiate(GeneralSettings.TargetObject);
    }

    internal override void TurnOff()
    {
        base.TurnOff();
        Destroy(_targetObject);
    }

    internal void SetPosition(Vector3 position)
    {
        _targetObject.transform.position = position;
    }
}
