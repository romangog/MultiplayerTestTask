using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Feel: MonoBehaviour
{
    [SerializeField] private MMFeedback _shakeCameraLight;

    internal void ShakeCameraLight()
    {
        _shakeCameraLight.Play(Vector3.zero);
    }
}
