using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CamerasController : MonoBehaviour
{
    [SerializeField] private List<VirtualCameraController> _camerasSet;

    public VirtualCameraController CurrentCamera => _currentCamera;

    private VirtualCameraController _currentCamera;

    public void ChooseCamera(int i)
    {
        ChooseCamera(_camerasSet[i]);
    }

    public void ChooseCamera(VirtualCameraController camera)
    {
        if (camera == _currentCamera) return;

        if (_currentCamera != null)
            _currentCamera.DecPriority();
        camera.gameObject.SetActive(true);
        camera.IncrPriority();
        _currentCamera = camera;
    }
}
