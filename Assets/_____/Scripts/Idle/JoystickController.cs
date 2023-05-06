using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class JoystickController:MonoBehaviour, IInitializable
{
    internal Vector2 RawValue;
    internal Vector3 TransformedValue;

    private Joystick _joystick;

    Vector3 _relativeForward;
    Vector3 _relativeRight;

    private bool _IsEnabled;
    private MainCamera _mainCamera;

    [Inject]
    private void Construct(MainCamera mainCamera)
    {
        Debug.Log("Construct joystick controller");
        _mainCamera = mainCamera;
    }

    void IInitializable.Initialize()
    {
        _joystick = this.GetComponent<Joystick>();
    }

    private void Update()
    {
        if (_IsEnabled)
        {
            CalculateRelativeAxis();
            RawValue = _joystick.Direction;
            TransformedValue = _relativeRight * RawValue.x + _relativeForward * RawValue.y;
        }
    }

    internal void Setup()
    {
        CalculateRelativeAxis();
    }

    internal void CalculateRelativeAxis()
    {
        _relativeRight = _mainCamera.transform.TransformDirection(Vector3.right);
        _relativeForward = Vector3.Cross(_relativeRight, Vector3.up);
    }

    internal void DisableControl()
    {
        _IsEnabled = false;
        _joystick.gameObject.SetActive(false);
        _joystick.Disable();
        RawValue = Vector2.zero;
        TransformedValue = Vector3.zero;
    }

    internal void EnableControl()
    {
        _IsEnabled = true;
        _joystick.gameObject.SetActive(true);
        RawValue = Vector2.zero;
        TransformedValue = Vector3.zero;
    }

}