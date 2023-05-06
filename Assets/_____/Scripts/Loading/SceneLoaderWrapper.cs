using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zenject;

public class SceneLoaderWrapper
{
    public static bool SkipStartScreen;

    private int _currentAdditiveSceneIndex = -1;
    private MySceneLoader _sceneLoader;

    public SceneLoaderWrapper(MySceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    internal void LoadLevel(int level)
    {
        if (_currentAdditiveSceneIndex != -1)
        {
            _sceneLoader.LoadUnloadScene(level, _currentAdditiveSceneIndex);
        }
        else
        {
            _sceneLoader.LoadScene(level);
        }
        _currentAdditiveSceneIndex = level;
    }
}
