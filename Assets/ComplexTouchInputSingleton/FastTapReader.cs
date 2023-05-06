using System;
using UnityEngine;

namespace ComplexTouchInput
{
    public class FastTapReader : IComplexActionReader
    {
        public bool IsStopped => _IsStopped;
        public Action StoppedEvent { get; set; }
        public Wrapper FastTapObserver => _observer;

        private bool _IsStopped;
        private Wrapper _observer;

        private float _decrease;
        private float _maxScore;
        private float _currentScore;

        public FastTapReader(int taps, float decreasePerSec)
        {
            _decrease = decreasePerSec;
            _maxScore = taps;
            _currentScore = 0f;
            _observer = new Wrapper(this);
        }

        public void TouchDown(Vector3 touchDownPhysicalPosition)
        {
            _currentScore += 1f;
            _observer.Tap();
            _currentScore = Mathf.Min(_maxScore, _currentScore);
            _observer.ChangeProgress(_currentScore / _maxScore);
            if (_currentScore >= _maxScore)
                _observer.Complete();
        }

        public void TouchUp(Vector3 touchUpPhysicalPosition)
        {
            //---
        }

        public void TouchUpdate(Vector3 touchPhysicalPosition)
        {
            //...
        }

        public void OnUpdate()
        {
            _currentScore -= _decrease * Time.deltaTime;
            _currentScore = Mathf.Max(0f, _currentScore);
            _observer.ChangeProgress(_currentScore / _maxScore);
        }

        public void StopReading()
        {
            if (_IsStopped) return;
            _IsStopped = true;
            StoppedEvent?.Invoke();
        }


        public class Wrapper :IInputReaderWrapper
        {
            private Action CompleteEvent;
            private Action TappedEvent;
            private Action<float> ProgressChangedEvent;

            private FastTapReader _reader;

            public Wrapper(FastTapReader reader)
            {
                _reader = reader;
            }

            public void Complete()
            {
                CompleteEvent?.Invoke();
                StopReading();
            }

            public void Tap()
            {
                TappedEvent?.Invoke();
            }

            public void ChangeProgress(float progress)
            {
                ProgressChangedEvent?.Invoke(progress);
            }

            public Wrapper OnCompleted(Action action)
            {
                CompleteEvent += action;
                return this;
            }

            public Wrapper OnTapped(Action action)
            {
                TappedEvent += action;
                return this;
            }

            public Wrapper OnProgressChanged(Action<float> action)
            {
                ProgressChangedEvent += action;
                return this;
            }

            public void StopReading()
            {
                _reader.StopReading();
            }
        }
    }
}
