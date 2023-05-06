using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MySceneLoader
{
    [Inject] public ZenjectSceneLoader _loader;

    private bool _LoadingScene;
    private bool _UnloadingScene;
    private LoadingScreenView _loadingScreenView;

    public MySceneLoader(LoadingScreenView loadingScreenView)
    {
        _loadingScreenView = loadingScreenView;
    }

    internal void LoadScene(int num)
    {
        _LoadingScene = true;
        AsyncOperation op = _loader.LoadSceneAsync(num, LoadSceneMode.Additive, null, LoadSceneRelationship.Child);
        op.completed += OnCompletedLoading;
        WaitProcessing();
    }

    internal void LoadUnloadScene(int num, int oldSceneNum)
    {
        _loadingScreenView.Show(LoadUnload);
        _UnloadingScene = true;
        void LoadUnload()
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(oldSceneNum);
            unloadOp.completed += OnCompletedUnloading;
            LoadScene(num);
        }

    }

    private void OnCompletedUnloading(AsyncOperation op)
    {
        _UnloadingScene = false;
        op.completed -= OnCompletedUnloading;
    }

    private void OnCompletedLoading(AsyncOperation op)
    {
        _LoadingScene = false;
        op.completed -= OnCompletedLoading;
    }

    private bool IsProcessing() => _LoadingScene || _UnloadingScene;

    private async void WaitProcessing()
    {
        await Task.Run(()=>Thread.Sleep(1000));
        while (IsProcessing())
        {
            await Task.Yield();
        }
        _loadingScreenView.Hide();
    }

}
