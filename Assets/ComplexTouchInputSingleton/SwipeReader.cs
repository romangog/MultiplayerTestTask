using System;
using UnityEngine;

namespace ComplexTouchInput
{
    public class SwipeReader : IComplexActionReader
    {
        public bool IsStopped => _IsStopped;
        public Action StoppedEvent { get; set; }
        public Wrapper SwipeObserver => _observer;

        private bool _IsStopped;
        private Wrapper _observer;

        private bool _IsHolding;
        private Vector3 _prevPos;
        private Vector3 _swipeDirection;
        private float _lengthSqr;
        private Vector3 _currentVector;
        private float _sharpness;

        public SwipeReader(Vector3 swipe, float sharpness)
        {
            _lengthSqr = swipe.magnitude;
            _swipeDirection = swipe.normalized;
            _sharpness = sharpness;
            _observer = new Wrapper(this);
        }

        public void TouchDown(Vector3 touchDownPhysicalPosition)
        {
            _IsHolding = true;
            _prevPos = touchDownPhysicalPosition;
        }

        public void TouchUp(Vector3 touchUpPhysicalPosition)
        {
            _IsHolding = false;
            _currentVector = Vector3.zero;
        }

        public void TouchUpdate(Vector3 touchPhysicalPosition)
        {
            if (!_IsHolding) return;
            Vector3 delta = touchPhysicalPosition - _prevPos;
            Vector3 proj = Vector3.Project(delta, _swipeDirection);
            _prevPos = touchPhysicalPosition;
            _currentVector += proj;
            _currentVector -= _currentVector.normalized * _sharpness * Time.deltaTime;
            if (_currentVector.normalized == -_swipeDirection.normalized)
            {
                _currentVector = Vector3.zero;
            }
            if (_currentVector.sqrMagnitude > _lengthSqr)
            {
                _observer.Invoke();
            }
        }

        public void StopReading()
        {
            if (_IsStopped) return;
            _IsStopped = true;
            StoppedEvent?.Invoke();
        }

        public void OnUpdate()
        {
            //...
        }

        public class Wrapper : IInputReaderWrapper
        {
            private Action SwipedEvent;
            private SwipeReader _sr;

            public Wrapper(SwipeReader sr)
            {
                _sr = sr;
            }

            public void Invoke()
            {
                SwipedEvent?.Invoke();
                StopReading();
            }

            public Wrapper OnSwiped(Action action)
            {
                SwipedEvent += action;
                return this;
            }

            public void StopReading()
            {
                _sr.StopReading();
            }
        }


    }
}
