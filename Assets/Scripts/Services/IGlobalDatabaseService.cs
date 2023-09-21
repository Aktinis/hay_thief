using UnityEngine;

public interface IGlobalDatabaseService
{
    public int GetPlayerLayerMaskValue { get; }

    public int GetEnemyLayerMaskValue { get; }

    public int GetFloorLayerMaskValue { get; }

    public int GetObstacleLaterMaskValue { get; }

    public int GetEnemyCount { get; }

    public int GetPoolPrewarmCount { get; }

    public Vector3 GetPlayerStartPosition { get; }

}
