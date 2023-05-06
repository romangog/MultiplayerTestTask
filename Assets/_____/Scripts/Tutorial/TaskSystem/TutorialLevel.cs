using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialLevel
{
    internal UnityEvent TaskSucceedEvent { get; private set; } = new UnityEvent();
    internal TutorialTask[] Tasks => _tasks;

    [SerializeField] private TutorialTask[] _tasks;

    private int _currentIndex;



    internal void Setup()
    {
        foreach (var task in _tasks)
        {
            task.Setup();
            task.SucceedEvent.AddListener(OnSucceedTask);
        }
    }

    private void OnSucceedTask()
    {
        TaskSucceedEvent.Invoke();
    }

    internal void SetFirstTutorial()
    {
        if (_currentIndex >= _tasks.Length) return;
        _currentIndex = 0;
        Show(false);
    }

    internal void IterateTask()
    {
        _currentIndex++;
    }

    internal void Show(bool delay)
    {
        _tasks[_currentIndex].Show(delay);

    }

    internal void SetTutorial(int index)
    {
        _currentIndex = index;
    }

    internal void Progress(int progress = 1)
    {
        _tasks[_currentIndex].Progress(progress);
    }

    internal void Regress(int v)
    {
        _tasks[_currentIndex].Regress();
        _currentIndex -= v;
        Show(true);
    }
}
