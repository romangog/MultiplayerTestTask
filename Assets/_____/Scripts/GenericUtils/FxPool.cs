using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxPool : MonoBehaviour
{
    public GameObject GameObjectToPool;
    public int PoolSize = 20;
    public bool PoolCanExpand = true;
    protected List<GameObject> _pooledGameObjects;
    private int _lastObject;

    /// <summary>
    /// On awake we fill our object pool
    /// </summary>
    protected virtual void Awake()
    {
        FillObjectPool();
    }

    /// <summary>
    /// Determines the name of the object pool.
    /// </summary>
    /// <returns>The object pool name.</returns>
    protected virtual string DetermineObjectPoolName()
    {
        return ("[MiniObjectPool] " + this.name);
    }

    /// <summary>
    /// Implement this method to fill the pool with objects
    /// </summary>
    public virtual void FillObjectPool()
    {
        if (GameObjectToPool == null)
        {
            return;
        }

        // we initialize the list we'll use to 
        _pooledGameObjects = new List<GameObject>();

        int objectsToSpawn = PoolSize;

        // we add to the pool the specified number of objects
        for (int i = 0; i < objectsToSpawn; i++)
        {
            AddOneObjectToThePool();
        }
    }

    /// <summary>
    /// Implement this method to return a gameobject
    /// </summary>
    /// <returns>The pooled game object.</returns>
    public virtual GameObject ActivateEffect()
    {
        GameObject effect = _pooledGameObjects[_lastObject];
        _pooledGameObjects[_lastObject].SetActive(false);
        _pooledGameObjects[_lastObject].SetActive(true);
        _lastObject = (_lastObject + 1) % _pooledGameObjects.Count;
        return effect;
    }

    /// <summary>
    /// Adds one object of the specified type (in the inspector) to the pool.
    /// </summary>
    /// <returns>The one object to the pool.</returns>
    protected virtual GameObject AddOneObjectToThePool()
    {
        if (GameObjectToPool == null)
        {
            Debug.LogWarning("The " + gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
            return null;
        }
        GameObject newGameObject = (GameObject)Instantiate(GameObjectToPool);
        newGameObject.gameObject.SetActive(false);
        newGameObject.transform.SetParent(this.transform);
        newGameObject.name = GameObjectToPool.name + "-" + _pooledGameObjects.Count;

        _pooledGameObjects.Add(newGameObject);

        return newGameObject;
    }
}

