using System.Collections;
using System.Collections.Generic;
using Zenject;

public class LobbySceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Lobby>().AsSingle().NonLazy();
    }
}
