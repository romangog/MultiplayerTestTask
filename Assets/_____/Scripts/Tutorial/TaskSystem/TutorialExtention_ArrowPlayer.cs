using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExtention_ArrowPlayer : TutorialExtention
{
    internal static TutorialExtention_ArrowPlayer Instance;
    public Transform From;
    public Transform To;
    public Transform ArrowPivot;
    public Vector3 Up = new Vector3(0, 1, 0);

    private Vector3 _direction = new Vector3(0, 0, 1);

    internal void LateUpdate()
    {
        ArrowPivot.transform.position = From.transform.position;
        if (To)
        {
            Vector3 pos = To.position;
            pos.y = ArrowPivot.transform.position.y;
            ArrowPivot.transform.LookAt(pos, Up);
        }
        else
        {
            ArrowPivot.transform.forward = _direction;
        }
    }

    internal override void TurnOn()
    {
        base.TurnOn();
        Instance = this;
        ArrowPivot.transform.gameObject.SetActive(true);
    }

    internal override void TurnOff()
    {
        base.TurnOff();
        ArrowPivot.transform.gameObject.SetActive(false);
    }

    internal void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
}

