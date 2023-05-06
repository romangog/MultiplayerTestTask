using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Money : ITickable, IInitializable
{
    internal int Count;
    internal int MoneyOnLevel;

    internal UnityEvent<int> MoneyUpdatedEvent { get; private set; } = new UnityEvent<int>();
    internal UnityEvent<int> MoneyOnLevelUpdatedEvent { get; private set; } = new UnityEvent<int>();
    internal UnityEvent MoneyNotEnoughEvent { get; private set; } = new UnityEvent();

    private GameModSettings _gmod;
    private readonly SLS.Snapshot _saveData;

    public Money(GameModSettings gmod,
        SLS.Snapshot saveData)
    {
        _gmod = gmod;
        this._saveData = saveData;
    }

    public void Initialize()
    {
        Load();
    }

    internal void Load()
    {
        if (_gmod.StartMoney.Allowed)
            Count = _gmod.StartMoney.Num;
        else
            Count = _saveData.Money;
        MoneyUpdatedEvent.Invoke(Count);
    }

    internal void DropLevelMoney()
    {
        MoneyOnLevel = 0;
        MoneyOnLevelUpdatedEvent.Invoke(MoneyOnLevel);
    }

    internal void Add(int addAmount)
    {
        Count += addAmount;
        _saveData.Money = Count;
        MoneyUpdatedEvent?.Invoke(Count);
    }

    internal void AddOnLevel(int addAmount)
    {
        MoneyOnLevel += addAmount;
        MoneyOnLevelUpdatedEvent.Invoke(MoneyOnLevel);
    }

    internal bool TrySpend(int spendAmount)
    {
        if (Count - spendAmount < 0)
        {
            MoneyNotEnoughEvent.Invoke();
            return false;
        }
        Count -= spendAmount;
        _saveData.Money = Count;
        MoneyUpdatedEvent.Invoke(Count);

        return true;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Add(500);
        }
    }

    internal void SpendOnLevel(int v)
    {
        if (v > MoneyOnLevel)
            v = MoneyOnLevel;

        MoneyOnLevel -= v;
        MoneyOnLevelUpdatedEvent.Invoke(MoneyOnLevel);
    }

    internal void SetMoney(int v)
    {
        Count = v;
        _saveData.Money = Count;
        MoneyUpdatedEvent?.Invoke(Count);
    }

    internal void SetMoneyOnLevel(int v)
    {
        MoneyOnLevel = v;
        MoneyOnLevelUpdatedEvent?.Invoke(MoneyOnLevel);
    }
}
