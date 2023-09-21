using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private CinemachineVirtualCamera vCamera = null;
    [SerializeField] private Transform bounds;

    public float GetFarClippingPlane()
    {
        return mainCamera.farClipPlane;
    }

    public void SetTarget(Transform target)
    {
        vCamera.Follow = target;
    }

    public void Setup(Vector3 position, Vector3 rotation, Vector3 size, Transform target)
    {
        transform.position = position;
        vCamera.transform.rotation = Quaternion.Euler(rotation);
        bounds.localScale = size;
        SetTarget(target);
    }

    public Ray ScreenPointToRay(Vector3 pos)
    {
        return mainCamera.ScreenPointToRay(pos);
    }

    public Vector3 ScreenToWorldPoint(Vector3 pos)
    {
        return mainCamera.ScreenToWorldPoint(pos);
    }
}
