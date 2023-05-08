using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [HideLabel]
    [TabGroup("Game Mod")]
    public GameModSettings GameMod;

    [HideLabel]
    [TabGroup("Loader Settings")]
    public MyLoader.Settings LoaderSettings;

    [HideLabel]
    [TabGroup("Player Settings")]
    public Player.Settings PlayerSettings;

    [HideLabel]
    [TabGroup("Projectile Settings")]
    public Projectile.Settings ProjectileSettings;

    [HideLabel]
    [TabGroup("Game Settings")]
    public GameSettings GameSettings; // DEprecated


    [HideLabel]
    [TabGroup("Prefabs")]
    public Prefabs Prefabs;

    public override void InstallBindings()
    {
        Container.BindInstance(GameMod).AsSingle().NonLazy();
        Container.BindInstance(LoaderSettings).AsSingle().NonLazy();
        Container.BindInstance(PlayerSettings).AsSingle().NonLazy();
        Container.BindInstance(ProjectileSettings).AsSingle().NonLazy();
        Container.BindInstance(GameSettings).AsSingle().NonLazy(); // Deprecated
        Container.BindInstance(Prefabs).AsSingle().NonLazy();
    }
}