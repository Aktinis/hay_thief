using System;

public interface ILevelManagerService
{
    public void LoadGameLevel(int index, Action callback = null);

    public void LoadScene(int sceneIndex, Action callback);

}