using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelManagerService : ILevelManagerService
{
    private int lastLoadedSceneIndex = -1;
    private Action cachedCallback = null;


    public void LoadScene(int sceneIndex, Action callback)
    {
        cachedCallback = callback;
        var op = SceneManager.LoadSceneAsync(sceneIndex);
        op.completed += OnSceneLoaded;
    }

    public void LoadGameLevel(int index, Action callback = null)
    {
        cachedCallback = callback;

        if (lastLoadedSceneIndex > 0)
        {
            var op = SceneManager.UnloadSceneAsync(lastLoadedSceneIndex.ToString());
            op.completed += OnGameLevelUnload;
        }
        else
        {
            lastLoadedSceneIndex = index;
            SceneManager.LoadScene(1);
            var op = SceneManager.LoadSceneAsync(lastLoadedSceneIndex.ToString(), LoadSceneMode.Additive);
            op.completed += OnLoadGameLevel;
        }
    }

    private void OnGameLevelUnload(AsyncOperation obj)
    {
        obj.completed -= OnGameLevelUnload;
        var op = SceneManager.LoadSceneAsync(lastLoadedSceneIndex.ToString(), LoadSceneMode.Additive);
        op.completed += OnLoadGameLevel;

    }

    private void OnLoadGameLevel(AsyncOperation obj)
    {
        obj.completed -= OnLoadGameLevel;
        cachedCallback?.Invoke();
        cachedCallback = null;
    }

    private void OnSceneLoaded(AsyncOperation obj)
    {
        cachedCallback?.Invoke();
        lastLoadedSceneIndex = -1;
        cachedCallback = null;
    }
}
