using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SpawnPointGenerator
{
    [MenuItem("Level/Generate level spawn points")]
    public static void GenerateData()
    {
        LevelData dataAsset = AssetDatabase.LoadAssetAtPath<LevelData>($"Assets/Resources/{Paths.LEVEL_DATA_PATH}{SceneManager.GetActiveScene().name}");
        if (dataAsset == null)
        {
            dataAsset = ScriptableObject.CreateInstance<LevelData>();

            AssetDatabase.CreateAsset(dataAsset, $"Assets/Resources/{Paths.LEVEL_DATA_PATH}{SceneManager.GetActiveScene().name}.asset");
            AssetDatabase.SaveAssets();
        }
        List<Vector2> spawnPoints = new List<Vector2>();
        int outOfBoundsCount = 0; 
        for (int i = 0; i < 100; i++)
        {
            outOfBoundsCount = 0;
            for (int j = 0; j < 100; j++)
            {
                if (i <= 8 && j <= 8)
                {
                    continue;
                }

                if(Physics.Raycast(new Vector3(i,100f,j), Vector3.down, out var hit, 200f))
                {
                    if(hit.transform.gameObject.layer == 9)
                    {
                        spawnPoints.Add(new Vector2(i, j));
                    }
                }
                else
                {
                    outOfBoundsCount++;
                }

                if (outOfBoundsCount >= 3)
                {
                    continue;
                }
            }
        }

        dataAsset.SetData(spawnPoints);
        EditorUtility.SetDirty(dataAsset);
        AssetDatabase.SaveAssets();
        EditorUtility.ClearDirty(dataAsset);
    }
}
