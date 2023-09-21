using System;
using TMPro;
using UnityEngine;

public sealed class QuestScreen : BaseScreen<QuestScreen, QuestScreenMessage>
{
    [SerializeField] private TextMeshProUGUI text = null;


    protected override void OnClose(Action onComplete)
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen(QuestScreenMessage message, Action onComplete)
    {
        UpdateGoal(message.Progress, message.Goal);
        gameObject.SetActive(true);
    }

    protected override void OnMessageReceive(QuestScreenMessage message)
    {
        UpdateGoal(message.Progress, message.Goal);
    }

    private void UpdateGoal(int progress, int goal)
    {
        text.text = progress + " / " + goal;
    }
}
