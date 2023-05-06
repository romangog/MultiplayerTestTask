using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum LevelStateType
{
    Start,
    Main,
    Finish
}

public class LevelStateMachine : ITickable, IInitializable, IFixedTickable
{
    private LevelState _currentState;

    private LevelState[] _states;

    public LevelStateMachine(
        EventBus eventBus,
        CamerasController camerasController,
        GameSettings gameSettings,
        SimpleTouchInput simpleTouchInput,
        BlackScreen blackScreen,
        SLS.Snapshot snapshot,
        Prefabs prefabs,
        SceneLoaderWrapper sceneLoader,
        Money money
        )
    {

        StartLevelState startLevelState = new StartLevelState();

        _states = new LevelState[]
        {
            startLevelState
        };

        foreach (var state in _states)
        {
            state.CalledForStateChangeEvent += ChangeState;
        }
    }

    public void ChangeState(LevelStateType stateType)
    {
        if (_currentState != null && _currentState.Type != stateType)
        {
            _currentState.Stop();
        }
        _currentState = _states[(int)stateType];

        _currentState.Start();
    }

    public void FixedTick()
    {
        _currentState?.FixedUpdate();
    }

    public void Initialize()
    {
        ChangeState(LevelStateType.Start);
    }

    public void Tick()
    {
        _currentState?.Update();
    }
}

public abstract class LevelState
{
    public Action<LevelStateType> CalledForStateChangeEvent;
    public abstract LevelStateType Type { get; }
    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Stop();
}

public class StartLevelState : LevelState
{
    public override LevelStateType Type => LevelStateType.Start;

    public StartLevelState()
    {
    }

    public override void Start()
    {
        
    }

    public override void Stop()
    {
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }
}