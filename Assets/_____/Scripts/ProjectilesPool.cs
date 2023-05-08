using Fusion;
using System.Collections;
using UnityEngine;
using Zenject;

public class ProjectilesPool : SimulationBehaviour, IObjectPool<Projectile>, IOnPlayerJoinedInitializable
{
    [SerializeField] private int _initialSize;
    private NetworkComponentPool<Projectile> _gameObjectsPool;
    private NetworkRunner _runner;
    private Prefabs _prefabs;
    private Projectile.Settings _projectileSettings;

    [Inject]
    private void Construct(
        Prefabs prefabs,
        Projectile.Settings projectileSettings,
        NetworkRunner runner)
    {
        _runner = runner;
        _prefabs = prefabs;
        Debug.Log(_prefabs);
        Debug.Log(_prefabs.ProjectilePrefab);
        _projectileSettings = projectileSettings;
    }

    private void OnAddedNewProjectile(Projectile obj)
    {
        obj.Construct(_projectileSettings.HittableLayerMask, _projectileSettings.Speed);
    }

    public Projectile Get()
    {
        Projectile projectile = _gameObjectsPool.Get();
        return projectile;
    }

    public void Return(GameObject obj)
    {
        _gameObjectsPool.Return(obj);
    }

    public void InitializeForLocalPlayer()
    {
        Debug.Log("OnPlayerJoined projectile pool");
        _gameObjectsPool = new NetworkComponentPool<Projectile>(_prefabs.ProjectilePrefab, _initialSize, null, _runner, OnAddedNewProjectile);
    }
}

public interface IOnPlayerJoinedInitializable
{
    public void InitializeForLocalPlayer();
}


public interface IObjectPool<T> where T : Object
{
    T Get();
    void Return(GameObject obj);
}
