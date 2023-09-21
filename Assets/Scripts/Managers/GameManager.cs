using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    private static readonly ILevelManagerService levelManager = new LevelManagerService();
    private static IMonoUtilityService monoUtilityService = null;
    private static GameState gameStateController = null;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        monoUtilityService = MonoUtilityService.Initialize();

        GlobalContainer.Add<IQuestService>(new QuestService());
        GlobalContainer.Add<IMonoUtilityService>(monoUtilityService);
        GlobalContainer.Add<IGlobalDatabaseService>(GlobalData.Initialize());
        GlobalContainer.Add<ILevelManagerService>(levelManager);

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private static void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if(int.TryParse(newScene.name, out int levelIndex))
        {
            StartGame(levelIndex);
        }
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public static void StartGame(int index)
    {
        levelManager.LoadGameLevel(index, () =>
        {
            gameStateController = new GameState();
            monoUtilityService.StartWrappedCoroutine(gameStateController.Setup(index));
        });
    }

    public static void ContinueGame()
    {
        levelManager.LoadGameLevel(1, () =>
        {
            monoUtilityService.StartWrappedCoroutine(gameStateController.Setup(1));
        });
    }

    public static void ExitGame()
    {
        gameStateController.Cleanup();
        gameStateController = null;
        levelManager.LoadScene(0, null);
    }
}