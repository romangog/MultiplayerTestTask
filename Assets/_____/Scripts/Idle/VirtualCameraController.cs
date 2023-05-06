using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using Zenject;
using System;

public class VirtualCameraController : MonoBehaviour
{
    public CinemachineVirtualCameraBase VirtualCamera => _virtualCamera;
    [SerializeField] private CinemachineVirtualCameraBase _virtualCamera;

    internal void SetTarget(Transform transform)
    {
        _virtualCamera.Follow = transform;
        _virtualCamera.LookAt = transform;
    }

    internal void IncrPriority()
    {
        _virtualCamera.Priority = 11;
    }

    internal void DecPriority()
    {
        _virtualCamera.Priority = 10;
    }
}
