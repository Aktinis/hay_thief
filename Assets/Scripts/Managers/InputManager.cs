using System;
using UnityEngine;

public sealed class InputManager
{
    public static event Action<Vector3> Move;
    private readonly IInput activeInput = null;


    public InputManager(CameraController cameraController, Action<Transform> setTarget)
    {
        activeInput = new MouseInput(cameraController, OnMove, setTarget);
    }

    private void OnMove(Vector3 pos)
    {
        Move(pos);
    }

    public void HandleInput()
    {
        activeInput.HandleInput();
    }
}
