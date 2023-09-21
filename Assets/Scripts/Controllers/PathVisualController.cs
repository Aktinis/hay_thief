using UnityEngine;

public class PathVisualController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = null;


    public void DrawPath(Vector3[] path)
    {
        lineRenderer.positionCount = path.Length;
        lineRenderer.SetPositions(path);
    }

    public void Reset()
    {
        lineRenderer.positionCount = 0;
    }
}
