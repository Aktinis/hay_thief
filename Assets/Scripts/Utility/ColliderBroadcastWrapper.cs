using System;
using UnityEngine;

public class ColliderBroadcastWrapper : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent;


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
}