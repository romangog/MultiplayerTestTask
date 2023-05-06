using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class TutorialTaskSystem : MonoBehaviour
{
    public int LevelIndex => _currentTutorialLevelIndex;
    public int Index => _totalIndex;
    internal UnityEvent TutorialStartedEvent => _levelStartedEvent;
    internal UnityEvent LevelEndedEvent => _levelEndedEvent;

    // Tutorial Progress variables
    //internal static int TotalMoneyCollected;

    private UnityEvent _levelStartedEvent = new UnityEvent();
    private UnityEvent _levelEndedEvent = new UnityEvent();
    [SerializeField] private int _index;
    [SerializeField] private TutorialLevel _level;

    private int _currentTutorialLevelIndex;
    private int _totalIndex;

    // Level 0
    internal static int MOVE_TUTORIAL_INDEX = 0;

    private GameModSettings _gmod;

    [Inject]
    private void Construct(GameModSettings gmod)
    {
        _gmod = gmod;

        _level.Setup();
        _level.TaskSucceedEvent.AddListener(OnSucceeded);
    }



    internal void Load()
    {
        _totalIndex = PlayerPrefs.GetInt("TutorialIndex", 0);
        if(_gmod.Screencast)
        {
            _totalIndex = 200;
        }
        _level.SetTutorial(_totalIndex);
        Debug.LogWarning("TUTORIAL LOAD " + _totalIndex);
        //if (Setupper.ClearPrefs) Instance._currentTutorialIndex = 0;
    }

    internal void Setup()
    {

    }

    internal void SetFirstTutorial() => SetFirstTutorial_p();
    internal void SetCurrentTutorial() => SetCurrentTutorial_p();
    internal void RegressTutorial(int index, int v) => RegressTutorial_p(index, v);



    internal void IterateTutorial() => IterateTutorial_p();

    private void RegressTutorial_p(int index, int v)
    {
        if (Index == index)
        {
            _level.Regress(v);
            _totalIndex -= v;
            PlayerPrefs.SetInt("TutorialIndex", _totalIndex);
            SetTutorialSpecial();
        }
    }

    private void SetFirstTutorial_p()
    {
        if (_totalIndex >= _level.Tasks.Length) return;
        _level.SetFirstTutorial();
        SetTutorialSpecial();
    }
    private void SetCurrentTutorial_p()
    {
        if (_totalIndex >= _level.Tasks.Length) return;
        _level.Show(false);
        SetTutorialSpecial();
    }

    private void IterateTutorial_p()
    {
        _totalIndex++;
        PlayerPrefs.SetInt("TutorialIndex", _totalIndex);

        _level.IterateTask();
        if (_totalIndex == _level.Tasks.Length)
            return;

        _level.Show(true);
        SetTutorialSpecial();

    }

    internal bool TryProgress(int index, int count) => TryProgress_p(index, count);

    private void OnSucceeded()
    {
        IterateTutorial();
    }

    private void Progress(int progress = 1)
    {
        _level.Progress(progress);
    }

    private bool TryProgress_p(int index, int count)
    {
        if (Index == index)
        {
            Progress(count);
            return true;
        }
        return false;
    }

    private void SetTutorialSpecial()
    {
        //if (Index == MOVE_TUTORIAL_INDEX)
        //{
        //    TutorialExtention_TouchDownScreen.Instance.ButtonTouchedDownEvent.AddListener(OnTouchedMoveTutorial);
        //    void OnTouchedMoveTutorial()
        //    {
        //        Progress(1);
        //    }
        //    if (!SpawnedStartMoney)
        //    {
        //        SpawnedStartMoney = true;
        //        CoinSpawner
        //            .Instance
        //            .SetNum(100)
        //            .SetPosition(Level.Instance.StartMoneyPos.position)
        //            .SetOneStack()
        //            .SetRange(0f, 0f)
        //            .SetType(DropType.Money)
        //            .Spawn();
        //    }
        //}
    }
}
