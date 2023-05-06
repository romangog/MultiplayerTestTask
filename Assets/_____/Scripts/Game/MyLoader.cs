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

public class MyLoader : NetworkSceneManagerBase
{
	[SerializeField] private GameObject _loadScreen;

	private Settings _settings;

	[Inject]
	private void Construct(Settings settings)
    {
		_settings = settings;
	}

	private void Awake()
	{
		_loadScreen.SetActive(false);
	}

	protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
	{
		Debug.Log($"Switching Scene from {prevScene} to {newScene}");

		_loadScreen.SetActive(true);

		List<NetworkObject> sceneObjects = new List<NetworkObject>();

		string path;
		switch ((MapIndex)(int)newScene)
		{
			case MapIndex.Loading: path = _settings.LoadingScene; break;
			case MapIndex.Lobby: path = _settings.LobbyScene; break;
			case MapIndex.Game: path = _settings.GameScene; break;
			default: path = _settings.LobbyScene; break;
		}
		yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
		var loadedScene = SceneManager.GetSceneByPath(path);
		Debug.Log($"Loaded scene {path}: {loadedScene}");
		sceneObjects = FindNetworkObjects(loadedScene, disable: false);

		// Delay one frame
		yield return null;
		finished(sceneObjects);

		Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded {sceneObjects.Count} scene objects");

		_loadScreen.SetActive(false);
	}

	[Serializable]
	public class Settings
    {
		public SceneReference LoadingScene;
		public SceneReference LobbyScene;
		public SceneReference GameScene;
	}
}
