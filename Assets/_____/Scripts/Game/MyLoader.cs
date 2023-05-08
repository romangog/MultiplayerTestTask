using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public enum MapIndex
{
	Loading,
	Lobby,
	Game,
};

public class MyLoader : NetworkSceneManagerBase, ISceneLoadedEventInvokable
{
	[SerializeField] private GameObject _loadScreen;

	private Settings _settings;
	private ZenjectSceneLoader _zenjectLoader;

    public event Action<MapIndex> MapLoadedEvent;

    [Inject]
	private void Construct(Settings settings,
		ZenjectSceneLoader zenjectLoader)
    {
		_settings = settings;
		_zenjectLoader = zenjectLoader;
	}

	private void Awake()
	{
		_loadScreen.SetActive(false);

		DontDestroyOnLoad(gameObject);
	}

	protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
	{
		Debug.Log($"Switching Scene from {prevScene} to {newScene}");

		_loadScreen.SetActive(true);

		yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

		List<NetworkObject> sceneObjects = new List<NetworkObject>();

		string path;
		switch ((MapIndex)(int)newScene)
		{
			case MapIndex.Loading: path = _settings.LoadingScene; break;
			case MapIndex.Lobby: path = _settings.LobbyScene; break;
			case MapIndex.Game: path = _settings.GameScene; break;
			default: path = _settings.LobbyScene; break;
		}

		yield return _zenjectLoader.LoadSceneAsync(path, LoadSceneMode.Additive, null, LoadSceneRelationship.Child);
		var loadedScene = SceneManager.GetSceneByPath(path);
		Debug.Log($"Loaded scene {path}: {loadedScene}");
		sceneObjects = FindNetworkObjects(loadedScene, disable: false);

		// Delay one frame
		yield return null;
		finished(sceneObjects);

		Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");

		_loadScreen.SetActive(false);

		MapLoadedEvent?.Invoke((MapIndex)(int)newScene);
	}

    [Serializable]
	public class Settings
    {
		public SceneReference LoadingScene;
		public SceneReference LobbyScene;
		public SceneReference GameScene;
	}
}

public interface ISceneLoadedEventInvokable
{
	public event Action<MapIndex> MapLoadedEvent;
	
}
