using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexTouchInput
{
    public interface IComplexActionReader
    {
        bool IsStopped { get; }
        Action StoppedEvent { get; set; }
        void TouchDown(Vector3 touchDownPhysicalPosition);
        void TouchUp(Vector3 touchUpPhysicalPosition);
        void TouchUpdate(Vector3 touchPhysicalPosition);
        void OnUpdate();
        void StopReading();
    }
}
