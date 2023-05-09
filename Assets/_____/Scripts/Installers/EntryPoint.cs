using UnityEngine;
using Zenject;

public class EntryPoint: IInitializable
{
    private readonly SceneLoaderWrapper loader;

    public EntryPoint(SceneLoaderWrapper loader)
    {
        this.loader = loader;
    }

    public void Initialize()
    {
        loader.LoadLevel(1);
    }
}
