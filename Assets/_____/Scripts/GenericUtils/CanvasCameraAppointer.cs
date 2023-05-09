using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CanvasCameraAppointer : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [Inject]
    private void Construct(UiCamera uiCamera)
    {
        _canvas.worldCamera = uiCamera.Camera;
    }
}
