using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonoComponentPool<T> : IObjectPool<T> where T : MonoBehaviour
{
    private List<T> _pool;
    private T _prefab;
    private int _nextObjectIndex;
    private Transform _parent;

    public MonoComponentPool(T prefab, int initialCapacity, Transform parent)
    {
        _prefab = prefab;
        _pool = new List<T>(initialCapacity);
        _parent = parent;

        for (int i = 0; i < initialCapacity; i++)
        {
            ExpandPool();
        }

        foreach (var obj in _pool)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public T Get()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].gameObject.activeSelf)
            {
                _pool[i].gameObject.SetActive(true);
                return _pool[i];
            }
        }
        ExpandPool();
        return _pool[_pool.Count - 1];
    }

    public void ExpandPool()
    {
        _pool.Add(UnityEngine.Object.Instantiate(_prefab, _parent));
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
    }
}


public class NetworkComponentPool<T> : IObjectPool<T> where T : NetworkBehaviour
{

    //public Action<T> AddedNewObjectEvent;
    private readonly List<T> _pool;
    private readonly T _prefab;
    private readonly Transform _parent;

    private readonly NetworkRunner _runner;
    private readonly Action<T> _onAddedObjectEvent;

    public NetworkComponentPool(T prefab, int initialCapacity, Transform parent, NetworkRunner runner, Action<T> OnAddedObject)
    {
        _runner = runner;
        _onAddedObjectEvent = OnAddedObject;
        _prefab = prefab;
        _pool = new List<T>(initialCapacity);
        _parent = parent;

        for (int i = 0; i < initialCapacity; i++)
        {
            ExpandPool();
        }

        foreach (var obj in _pool)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public T Get()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].gameObject.activeSelf)
            {
                _pool[i].gameObject.SetActive(true);
                return _pool[i];
            }
        }
        ExpandPool();
        return _pool[_pool.Count - 1];
    }

    public void ExpandPool()
    {
        var obj = _runner.Spawn(_prefab, inputAuthority: _runner.LocalPlayer);
        _pool.Add(obj);
        Debug.Log(obj);
        Debug.Log(_prefab);
        Debug.Log(obj.transform);
        Debug.Log(obj.transform.parent);
        Debug.Log(_parent);
        obj.transform.parent = _parent;
        _onAddedObjectEvent?.Invoke(obj);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
    }
}
