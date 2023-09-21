using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonoUtilityService : MonoBehaviour, IMonoUtilityService
{
    private List<Action> cachedActions = new List<Action>();


    public static MonoUtilityService Initialize()
    {
        var obj = new GameObject("MonoUtility").AddComponent<MonoUtilityService>().GetComponent<MonoUtilityService>();
        DontDestroyOnLoad(obj);
        return obj;
    }

    public Coroutine StartWrappedCoroutine(IEnumerator action)
    {
        return StartCoroutine(action);
    }

    public Coroutine StartDelayedCoroutine(Action action, float time)
    {
        return StartCoroutine(InternalCoroutine(action, time));
    }

    public void SubscribeToUpdate(Action action)
    {
        cachedActions.Add(action);
    }

    public void UnsubscribeToUpdate(Action action)
    {
        cachedActions.Remove(action);
    }

    private static IEnumerator InternalCoroutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    private void Update()
    {
        cachedActions.ForEach(x => x.Invoke());
    }
}
