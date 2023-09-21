using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField] private Vector3 boundsSize = new Vector3(8f,1f,12f);
    [SerializeField] private Vector3 cameraPosition = new Vector3(7.5f, 9.5f,1.5f);
    [SerializeField] private Vector3 cameraRotation = new Vector3(45f,0f,0f);
    [SerializeField] private List<Vector2> spawnPointGridPositions = new List<Vector2>();


    public int SpawnPointCount => spawnPointGridPositions.Count;

    public Vector2 GetSpawnPoint(int index) => spawnPointGridPositions[index];

    public List<Vector2> SpawnPointGridPositions => spawnPointGridPositions;
    public Vector3 BoundsSize => boundsSize;
    public Vector3 CameraStartPosition => cameraPosition;
    public Vector3 CameraRotation => cameraRotation;

    public void SetData(List<Vector2> spawnPointGridPositions)
    {
        this.spawnPointGridPositions = spawnPointGridPositions;
    }
}
