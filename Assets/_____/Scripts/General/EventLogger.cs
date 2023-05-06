using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
//using GameAnalyticsSDK;

public class EventLogger : IInitializable, ITickable
{
    private float _timer;
    private bool _IsTimerRunning;

    public void Initialize()
    {
       // GameAnalytics.Initialize();
    }

    public void SendStart(string level)
    {
        Debug.Log("SendStart");
        _IsTimerRunning = true;
        _timer = 0f;
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + level);
    }

    public void SendEnd(string level, bool IsWin)
    {
        Debug.Log("SendEnd");
        //LevelFinishedResult result = (IsWin) ? LevelFinishedResult.win : LevelFinishedResult.lose;
        //HoopslyIntegration.RaiseLevelFinishedEvent(level, result);
        //GAProgressionStatus status = (IsWin) ? GAProgressionStatus.Complete : GAProgressionStatus.Fail;
        //GameAnalytics.NewProgressionEvent(status, "Level_" + level, Mathf.CeilToInt(_timer));
        _IsTimerRunning = false;
    }

    public void Tick()
    {
        if (_IsTimerRunning)
            _timer += Time.deltaTime;
    }
}
