using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BootSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MySceneLoader>().AsSingle().NonLazy();
        Container.Bind<SceneLoaderWrapper>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle().NonLazy();

    }
}
