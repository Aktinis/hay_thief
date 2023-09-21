using System;
using UnityEngine;

public sealed class MouseInput : IInput
{
    private readonly Action<Vector3> onClick = null;
    private CameraController cameraController = null;
    private IGlobalDatabaseService globalDatabaseService = null;
    private readonly Action<Transform> setTarget;


    public MouseInput(CameraController cameraController, Action<Vector3> onClick, Action<Transform> setTarget)
    {
        globalDatabaseService = GlobalContainer.Get<IGlobalDatabaseService>();
        this.cameraController = cameraController;
        this.onClick = onClick;
        this.setTarget = setTarget;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = cameraController.ScreenPointToRay(Input.mousePosition);
            var allRaycasts = Physics.RaycastAll(ray, cameraController.GetFarClippingPlane());
            int index = -1;
            for (int i = 0; i < allRaycasts.Length; i++)
            {
                if (allRaycasts[i].transform.GetLayerTranslatedToLayerMaskValue() == globalDatabaseService.GetEnemyLayerMaskValue)
                {
                    index = i;
                    break;
                }
            }

            var followTarget = index >= 0 ? allRaycasts[index].collider.gameObject.transform : null;

            if(allRaycasts.Length > 0)
            {
                if (followTarget)
                {
                    setTarget?.Invoke(followTarget);
                }
                else
                {
                    onClick?.Invoke(new Vector3(allRaycasts[0].point.x, 0, allRaycasts[0].point.z));
                }
            }

        }
    }
}
