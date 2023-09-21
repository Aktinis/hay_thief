using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    private static Dictionary<Type, BaseScreen> screens = new Dictionary<Type, BaseScreen>();


    public static IEnumerator Setup()
    {
        var rootCanvasTransform = GameObject.Instantiate<Transform>(Resources.Load<Transform>("UICanvas"));
        yield return new WaitForEndOfFrame();
        var allScreens = Resources.LoadAll<BaseScreen>("UI");
        yield return new WaitForEndOfFrame();
        foreach (var screen in allScreens)
        {
            screen.gameObject.SetActive(false);
            var newScreen = GameObject.Instantiate(screen, rootCanvasTransform);
            screens.Add(screen.GetScreenType(), newScreen);
        }
        yield return new WaitForEndOfFrame();
    }

    public static void Cleanup()
    {
        screens.Clear();
    }

    public static void Open<T>(BaseMessage message = null, Action onComplete = null) where T : BaseScreen
    {
        if (screens.ContainsKey(typeof(T)))
        {
            screens[typeof(T)].Open(message, onComplete);
        }
        else
        {
            Debug.Log($"Screen doesn't exist! {typeof(T)} can't Open");
        }
    }

    public static void Close<T>(Action onComplete = null) where T : BaseScreen
    {
        if (screens.ContainsKey(typeof(T)))
        {
            screens[typeof(T)].Close(onComplete);
        }
        else
        {
            Debug.Log($"Screen doesn't exist! {typeof(T)} can't Close");
        }
    }

    public static void SendMessage<T>(BaseMessage message)
    {
        if (screens.ContainsKey(typeof(T)))
        {
            screens[typeof(T)].ReceiveMessage(message);
        }
        else
        {
            Debug.Log($"Screen doesn't exist! {typeof(T)} can't Close");
        }
    }
}
