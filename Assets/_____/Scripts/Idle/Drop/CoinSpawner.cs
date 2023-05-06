using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoinSpawner
{
    private int _num = 1;
    private DropType _type;
    private float _rangeMin = 1f;
    private float _rangeMax = 3f;
    private int _packsCount = 0;
    private bool _IsHoming = false;
    private Vector3 _position = Vector3.zero;
    private Dictionary<DropType, TextPopup> _typePopupDictionary = new Dictionary<DropType, TextPopup>();

    private Coin.Factory _coinFactory;

    public CoinSpawner(Coin.Factory coinFactory)
    {
        _coinFactory = coinFactory;
    }

    internal CoinSpawner SetNum(int num)
    {
        _num = num;
        return this;
    }

    internal CoinSpawner SetType(DropType type)
    {
        _type = type;
        return this;
    }

    internal CoinSpawner SetRange(float rangeMin, float rangeMax)
    {
        _rangeMin = rangeMin;
        _rangeMax = rangeMax;
        return this;
    }

    internal CoinSpawner SetPacksCount(int count)
    {
        _packsCount = count;
        return this;
    }

    internal CoinSpawner SetHoming()
    {
        _IsHoming = true;
        return this;
    }

    internal CoinSpawner SetPosition(Vector3 pos)
    {
        _position = pos;
        return this;
    }

    internal void Spawn()
    {
        if (_packsCount == 0)
            SpawnFountain();
        else
        {
            int numPerPack = (int)(_num / (float)_packsCount);
            if (numPerPack <= 0)
            {
                Debug.LogWarning("NumPEr Stack = " + numPerPack);
            }
            for (int i = 0; i < _packsCount; i++)
            {
                SpawnOneStack(numPerPack);
            }
        }

        Clear();

        void Clear()
        {
            _type = DropType.Money;
            _position = Vector3.zero;
            _rangeMin = 1f;
            _rangeMin = 3f;
            _num = 1;
            _IsHoming = false;
            _packsCount = 0;
        }

        void SpawnOneStack(int num)
        {
            Drop drop = SpawnOneCoin();
            if (num <= 0)
            {
                Debug.LogWarning("SpawnOneStack: num = " + num);
            }
            drop.Launch();
        }

        void SpawnFountain()
        {
            for (int i = 0; i < _num; i++)
            {
                Drop drop = SpawnOneCoin();
                drop.Launch();
            }
        }

        Drop SpawnOneCoin()
        {
            Drop.Settings settings = new Drop.Settings
            {
                RangeMin = _rangeMin,
                RangeMax = _rangeMax,
                IsHoming = _IsHoming,
                StartPosition = _position
            };

            Drop drop = Create(settings);
            drop.CollectedEvent.AddListener(OnCoinCollected);
            drop.SpawnedPopupEvent.AddListener(OnSpawnedPopup);
            drop.OnStackedEvent.AddListener(RemoveListenersFromCoin);
            return drop;
        }

        Drop Create(Drop.Settings settings)
        {
            switch (_type)
            {
                case DropType.Money:
                    return _coinFactory.Create(settings);
                default:
                    return null;
            }
        }
    }

    private void OnCoinCollected(Drop drop)
    {
        if (_typePopupDictionary.ContainsKey(drop.Type) && _typePopupDictionary[drop.Type].TimeSinceSpawn < 0.75f)
        {
            drop.ForbidSpawningPopup();
            _typePopupDictionary[drop.Type].AddNum(drop.Stack);
            RemoveListenersFromCoin(drop);
        }
    }

    private void OnSpawnedPopup(Drop drop, TextPopup popup)
    {
        _typePopupDictionary[drop.Type] = popup;
        RemoveListenersFromCoin(drop);
    }

    private void RemoveListenersFromCoin(Drop drop)
    {
        drop.SpawnedPopupEvent.RemoveAllListeners();
        drop.CollectedEvent.RemoveAllListeners();
        drop.OnStackedEvent.RemoveAllListeners();
    }
}
