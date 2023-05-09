using Fusion;
using UnityEngine;
using Zenject;

public class ProjectilesSpawner : NetworkBehaviour, IObjectPool<Projectile>, IOnPlayerJoinedInitializable
{
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
        _projectileSettings = projectileSettings;
    }


    public Projectile Get()
    {
        var projectile = Runner.Spawn(_prefabs.ProjectilePrefab, inputAuthority: Runner.LocalPlayer);
        projectile.Construct(_projectileSettings.HittableLayerMask, _projectileSettings.Speed);
        return projectile;
    }

    public void InitializeForLocalPlayer()
    {

    }

    public void Return(GameObject obj)
    {
        Runner.Despawn(obj.GetComponent<NetworkObject>());
    }
}
