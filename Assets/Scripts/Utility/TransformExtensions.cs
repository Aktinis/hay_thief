using UnityEngine;

public static class TransformExtensions
{
    public static Vector3 GetTopDownPosition(this Transform transform)
    {
        return new Vector3(transform.position.x, 0, transform.position.z);
    }

    public static int GetLayerTranslatedToLayerMaskValue(this Transform transform)
    {
        return 1 << transform.gameObject.layer;
    }
} 
