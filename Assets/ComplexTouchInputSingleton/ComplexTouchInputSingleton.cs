using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ComplexTouchInput
{
    // Derived from MonoBehaviour instead of ITickable
    public class ComplexTouchInput : ITickable
    {
        // Added Instance Field

        private List<IComplexActionReader> _inputReaders = new List<IComplexActionReader>();
        private List<IComplexActionReader> _removedReaders = new List<IComplexActionReader>();
        private List<IComplexActionReader> _preRegisteredReaders = new List<IComplexActionReader>();


        // Named Update instead of Tick
        public void Tick()
        {
            AddPreRegisteredReaders();
            ClearRemovedReaders();
            if (Input.GetMouseButtonDown(0))
                OnTouchDown();
            if (Input.GetMouseButtonUp(0))
                OnTouchUp();
            if (Input.GetMouseButton(0))
                OnTouchUpdate();
            OnUpdate();
        }

        private void AddPreRegisteredReaders()
        {
            _inputReaders.AddRange(_preRegisteredReaders);
            _preRegisteredReaders.Clear();
        }

        private void ClearRemovedReaders()
        {
            foreach (var reader in _removedReaders)
            {
                _inputReaders.Remove(reader);
            }
            _removedReaders.Clear();
        }

        private void OnTouchDown()
        {
            Vector3 touchPos = GetTouchPhysicalPosition(Input.mousePosition);
            foreach (var reader in _inputReaders)
                if (!reader.IsStopped) reader.TouchDown(touchPos);
        }

        private void OnTouchUp()
        {
            Vector3 touchPos = GetTouchPhysicalPosition(Input.mousePosition);
            foreach (var reader in _inputReaders)
                if (!reader.IsStopped) reader.TouchUp(touchPos);
        }

        private void OnTouchUpdate()
        {
            Vector3 touchPos = GetTouchPhysicalPosition(Input.mousePosition);
            foreach (var reader in _inputReaders)
                if (!reader.IsStopped) reader.TouchUpdate(touchPos);
        }

        private void OnUpdate()
        {
            foreach (var reader in _inputReaders)
                if (!reader.IsStopped) reader.OnUpdate();
        }

        private Vector2 GetTouchPhysicalPosition(Vector3 mousePos)
        {
            float ratio = Screen.width / (float)Screen.height;
            float physicalDistX = mousePos.x / Screen.width;
            float physicalDistY = mousePos.y / Screen.height;
            physicalDistY /= ratio;
            return new Vector2(physicalDistX, physicalDistY) * Screen.dpi;
        }

        public SwipeReader.Wrapper ReadSwipe(Vector3 swipe, float sharpness)
        {
            SwipeReader reader = new SwipeReader(swipe, sharpness);
            RegisterReader(reader);
            return reader.SwipeObserver;
        }

        public FastTapReader.Wrapper ReadFastTap(int taps, float decrease)
        {
            FastTapReader reader = new FastTapReader(taps, decrease);
            RegisterReader(reader);
            return reader.FastTapObserver;
        }

        private void RegisterReader(IComplexActionReader reader)
        {
            _preRegisteredReaders.Add(reader);
            reader.StoppedEvent += () => StopReader(reader);
        }

        private void StopReader(IComplexActionReader sr)
        {
            _removedReaders.Add(sr);
        }
    }
}
