using System;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour
{
    public abstract Type GetScreenType();

    public abstract void Open(BaseMessage message, Action onComplete);

    public abstract void ReceiveMessage(BaseMessage message);

    public abstract void Close(Action onComplete);
}

public abstract class BaseScreen<TScreen, TMessage> : BaseScreen where TMessage : BaseMessage
{
    public override void Close(Action onComplete = null)
    {
        OnClose(onComplete);
    }

    public override Type GetScreenType()
    {
        return typeof(TScreen);
    }

    public override void Open(BaseMessage message, Action onComplete)
    {
        OnOpen((TMessage)message, onComplete);
    }

    public override void ReceiveMessage(BaseMessage message)
    {
        OnMessageReceive((TMessage)message);
    }

    protected virtual void OnMessageReceive(TMessage message) { }

    protected abstract void OnOpen(TMessage message, Action onComplete);

    protected abstract void OnClose(Action onComplete);
}