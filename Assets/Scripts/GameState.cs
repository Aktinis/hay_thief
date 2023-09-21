using System.Collections;
using UnityEngine;

public sealed class GameState
{
    private PathVisualController visualController = null;

    private PlayerCharacter playerCharacter = null;
    private CameraController cameraController = null;
    private InputManager inputManager = null;
    private AICharacterPool enemyPool = null;
    private DeathParticlePool deathParticlePool = null;
    private AICharacterManager enemyManager = null;
    private bool runUpdate = false;
    private bool initialSetup = false;
    private IQuestService questService = null;
    private IGlobalDatabaseService globalDatabaseService = null;
    private IMonoUtilityService monoUtilityService = null;
    private LevelData leveldData = null;


    public GameState()
    {
        questService = GlobalContainer.Get<IQuestService>();
        globalDatabaseService = GlobalContainer.Get<IGlobalDatabaseService>();
        monoUtilityService = GlobalContainer.Get<IMonoUtilityService>();
        monoUtilityService.SubscribeToUpdate(Update);
    }

    public void Cleanup()
    {
        InputManager.Move -= playerCharacter.OnMove;
        runUpdate = false;
        initialSetup = false;
        monoUtilityService.UnsubscribeToUpdate(Update);
        UIManager.Cleanup();
        Resources.UnloadUnusedAssets();
    }

    public PlayerCharacter GetPlayerCharacter()
    {
        return playerCharacter;
    }

    public CameraController GetPlayerCamera()
    {
        return cameraController;
    }

    public IEnumerator Setup(int currentLevelIndex)
    {
        if (!initialSetup)
        {
            yield return InitialSetup();
            yield return UIManager.Setup();
        }

        yield return LevelSetup(currentLevelIndex);
    }

    private IEnumerator InitialSetup()
    {
        playerCharacter = GameObject.Instantiate<PlayerCharacter>(Resources.Load<PlayerCharacter>(Paths.PLAYER_PREFAB_PATH));
        yield return new WaitForEndOfFrame();

        playerCharacter.Setup(globalDatabaseService.GetPlayerStartPosition, Quaternion.identity, OnPlayerDeath);

        cameraController = GameObject.Instantiate<CameraController>(Resources.Load<CameraController>(Paths.CAMERA_CONTROLLER_PREFAB_PATH));

        yield return new WaitForEndOfFrame();

        enemyPool = new AICharacterPool(Resources.Load<AICharacter>(Paths.ENEMY_PREFAB_PATH));
        yield return new WaitForEndOfFrame();
        enemyPool.Setup(globalDatabaseService.GetPoolPrewarmCount);
        yield return new WaitForEndOfFrame();

        deathParticlePool = new DeathParticlePool(Resources.Load<ParticleSystem>(Paths.DEATH_PARTICLE_PREFAB_PATH));
        yield return new WaitForEndOfFrame();
        deathParticlePool.Setup(globalDatabaseService.GetPoolPrewarmCount);
        yield return new WaitForEndOfFrame();

        enemyManager = new AICharacterManager(enemyPool, deathParticlePool, () =>
        {
            questService.Increment();
            UIManager.SendMessage<QuestScreen>(new QuestScreenMessage(questService.GetProgress(), questService.GetGoal()));
        });

        inputManager = new InputManager(cameraController, playerCharacter.SetTarget);

        InputManager.Move += playerCharacter.OnMove;

        initialSetup = true;
    }

    private IEnumerator LevelSetup(int currentLevelIndex)
    {
        visualController = GameObject.Instantiate<PathVisualController>(Resources.Load<PathVisualController>(Paths.PATHING_VISUAL_CONTROLLER_PREFAB_PATH));
        LoadGameData(currentLevelIndex);
        questService.Initialize(globalDatabaseService.GetEnemyCount);

        enemyManager.Reset();
        enemyManager.Setup(globalDatabaseService.GetEnemyCount, leveldData.SpawnPointGridPositions);

        yield return new WaitForEndOfFrame();

        UIManager.Open<QuestScreen>(new QuestScreenMessage(questService.GetProgress(), questService.GetGoal()));

        playerCharacter.Setup(globalDatabaseService.GetPlayerStartPosition, Quaternion.identity, null);

        cameraController.Setup(leveldData.CameraStartPosition, leveldData.CameraRotation, leveldData.BoundsSize, playerCharacter.transform);


        yield return new WaitForEndOfFrame();
        runUpdate = true;
    }

    private void LoadGameData(int currentLevelIndex)
    {
        if (leveldData != null)
        {
            Resources.UnloadAsset(leveldData);
            leveldData = null;
        }
        leveldData = Resources.Load<LevelData>($"{Paths.LEVEL_DATA_PATH}{currentLevelIndex}");
    }

    private void Update()
    {
        if (runUpdate)
        {
            inputManager.HandleInput();
            playerCharacter.Move();
            playerCharacter.UpdateAnimation();
            if (playerCharacter.IsMoving())
            {
                visualController.DrawPath(playerCharacter.GetPath());
            }
            enemyManager.Update();
        }

        if (runUpdate && questService.GetProgress() == questService.GetGoal())
        {
            runUpdate = false;
            visualController.Reset();
            UIManager.Close<QuestScreen>();
            monoUtilityService.StartDelayedCoroutine(() => 
            { 
                UIManager.Open<EndScreen>(new EndScreenMessage(true)); 
            }, 1f);

        }
    }

    private void OnPlayerDeath()
    {
        runUpdate = false;
        visualController.Reset();
        UIManager.Close<QuestScreen>();
        UIManager.Open<EndScreen>(new EndScreenMessage(false));
    }
}
