using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScreenDistanceMeter : IInitializable
{
    internal static ScreenDistanceMeter Instance;
    [SerializeField] private float _coefficient;

    private float _ratio;
    private float _width;
    private float _height;

    void IInitializable.Initialize()
    {
        _width = Screen.width;
        _height = Screen.height;

        _ratio = _width / _height;
    }

    internal Vector2 GetPhysicalPosition(Vector3 mousePos)
    {
        float ratio = Screen.width / (float)Screen.height;
        float physicalDistX = mousePos.x / Screen.width;
        float physicalDistY = mousePos.y / Screen.height;
        physicalDistY /= ratio;
        return new Vector2(physicalDistX, physicalDistY) * Screen.dpi;
    }


}
