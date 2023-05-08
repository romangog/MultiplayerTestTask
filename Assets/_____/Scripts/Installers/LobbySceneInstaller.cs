using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbySceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SimpleTouchInput>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FusionManager>().AsSingle().NonLazy();

    }
}

public class EntryPoint: IInitializable
{
    private readonly SceneLoaderWrapper loader;

    public EntryPoint(SceneLoaderWrapper loader)
    {
        this.loader = loader;
    }

    public void Initialize()
    {
        Debug.Log("Load");
        loader.LoadLevel(1);
    }
}
