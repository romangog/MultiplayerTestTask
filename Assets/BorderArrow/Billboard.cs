using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.BorderArrow
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;

        internal void SetCameraTranform(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        private void Update()
        {
            Debug.Log(name);
            this.transform.forward = _cameraTransform.forward;
        }
    }
}
