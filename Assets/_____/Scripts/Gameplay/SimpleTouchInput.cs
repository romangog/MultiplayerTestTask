using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SimpleTouchInput : ITickable
{
    public Action StartedHoldingEvent;
    public Action EndedHoldingEvent;

    public bool IsHolding => _IsHolding;
    public Vector3 Distance => _distance;
    public Vector3 TickDelta => _delta;

    private bool _IsHolding;
    private Vector3 _touchDownPosition;
    private Vector3 _delta;
    private Vector3 _distance;
    private Vector3 _lastTickPos;
    private bool _IsReading;

    public void StartReading()
    {
        _IsReading = true;
    }

    public void StopReading()
    {
        _IsReading = false;
        _IsHolding = false;
        _delta = Vector3.zero;
        _distance = Vector3.zero;
    }

    public void Tick()
    {
        if (!_IsReading) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnTouchDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnTouchUp();
        }

        if (_IsHolding)
            OnTouchHold();
    }

    private void OnTouchDown()
    {
        _IsHolding = true;
        _touchDownPosition = GetTouchPhysicalPosition(Input.mousePosition);
        _lastTickPos = _touchDownPosition;
        StartedHoldingEvent?.Invoke();
    }

    private void OnTouchUp()
    {
        _IsHolding = false;
        EndedHoldingEvent?.Invoke();
        _delta = Vector3.zero;
        _distance = Vector3.zero;
    }

    private void OnTouchHold()
    {
        Vector3 currentPos = GetTouchPhysicalPosition(Input.mousePosition);
        ScreenLog.Log("PhysicalTouchPosition: " , currentPos.ToString());
        _distance = currentPos - _touchDownPosition;
        _delta = currentPos - _lastTickPos;
        _lastTickPos = currentPos;
    }

    private Vector2 GetTouchPhysicalPosition(Vector3 mousePos)
    {
        float ratio = Screen.width / (float)Screen.height;
        float physicalDistX = mousePos.x / Screen.width;
        float physicalDistY = mousePos.y / Screen.height;
        physicalDistY /= ratio;
        ScreenLog.Log("mousePos: ", mousePos.ToString());
        ScreenLog.Log("Screen.width: ", Screen.width.ToString());
        ScreenLog.Log("Screen.height: ", Screen.height.ToString());
        ScreenLog.Log("Screen.dpi: ", Screen.dpi.ToString());

        return new Vector2(physicalDistX, physicalDistY) / Screen.dpi;
    }
}
