using UnityEngine;

[CreateAssetMenu]
public sealed class GlobalData : ScriptableObject, IGlobalDatabaseService
{
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private int poolPrewarmCount = 10;
    [SerializeField] private Vector3 playerStartPosition = new Vector3(1.0f, 0f, 1.0f);
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask floorLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;


    public static GlobalData Initialize()
    {
        return Resources.Load<GlobalData>(Paths.GLOBAL_DATA_ASSET_PATH);
    }

    public int GetPlayerLayerMaskValue 
    { 
        get 
        { 
            return playerLayerMask.value;
        }
    } 

    public int GetEnemyLayerMaskValue
    {
        get
        {
            return enemyLayerMask.value;
        }
    }

    public int GetFloorLayerMaskValue
    {
        get
        {
            return floorLayerMask.value;
        }
    }

    public int GetObstacleLaterMaskValue
    {
        get
        {
            return obstacleLayerMask.value;
        }
    }

    public int GetEnemyCount
    {
        get
        {
            return enemyCount;
        }
    }

    public int GetPoolPrewarmCount
    {
        get
        {
            return poolPrewarmCount;
        }
    }

    public Vector3 GetPlayerStartPosition
    {
        get
        {
            return playerStartPosition;
        }
    }
} 
