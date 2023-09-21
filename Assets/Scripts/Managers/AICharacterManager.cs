using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AICharacterManager
{
    private List<AICharacter> characters = new List<AICharacter>();
    private List<float> characterNavigatonCooldowns = new List<float>();
    private List<Vector2> spawnPositions = new List<Vector2>();

    private AICharacterPool characterPool = null;
    private DeathParticlePool deathParticlePool = null;
    private Action onCharacterDeath = null;
    private IMonoUtilityService monoUtilityService = null;

    public AICharacterManager(AICharacterPool characterPool, DeathParticlePool deathParticlePool, Action questUpdate)
    {
        monoUtilityService = GlobalContainer.Get<IMonoUtilityService>();
        this.deathParticlePool = deathParticlePool;
        this.onCharacterDeath = questUpdate;
        this.characterPool = characterPool;
    }

    public void Setup(int enemyCount, List<Vector2> spawnPositions)
    {
        this.spawnPositions = spawnPositions;
        SetupCharacters(enemyCount);
    }

    public void Update()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].UpdateLogic();
            characters[i].UpdateAnimation();
        }
    }

    public Vector3 GetRandomisedPosition()
    {
        var index = UnityEngine.Random.Range(0, spawnPositions.Count);
        return new Vector3(spawnPositions[index].x, 0, spawnPositions[index].y);
    }

    public void Reset()
    {
        foreach (var item in characters)
        {
            characterPool.ReleaseToPool(item);
        }
        characters.Clear();
    }

    private void SetupCharacters(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var spawnedEnemy = characterPool.GetFromPool();
            spawnedEnemy.Setup(GetRandomisedPosition(), Quaternion.identity, () =>
            {
                onCharacterDeath?.Invoke();
                var particle = deathParticlePool.GetFromPool();
                particle.transform.position = spawnedEnemy.transform.GetTopDownPosition();
                monoUtilityService.StartDelayedCoroutine(() =>
                {
                    if (particle != null && deathParticlePool != null)
                    {
                        deathParticlePool.ReleaseToPool(particle);
                    }
                }, 1.0f);
            }, GetRandomisedPosition);

            characters.Add(spawnedEnemy);
            characterNavigatonCooldowns.Add(0);
        }
    }
}
