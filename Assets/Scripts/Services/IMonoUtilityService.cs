using System;
using System.Collections;
using UnityEngine;

public interface IMonoUtilityService
{
    public Coroutine StartWrappedCoroutine(IEnumerator action);

    public Coroutine StartDelayedCoroutine(Action action, float time);

    public void SubscribeToUpdate(Action action);

    public void UnsubscribeToUpdate(Action action);
}
