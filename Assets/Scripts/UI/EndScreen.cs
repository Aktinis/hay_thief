using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class EndScreen : BaseScreen<EndScreen, EndScreenMessage>
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartbutton;
    [SerializeField] private CanvasGroup canvasGroup;


    protected override void OnClose(Action onComplete)
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    protected override void OnOpen(EndScreenMessage message, Action onComplete)
    {
        restartbutton.gameObject.SetActive(!message.HasWon);
        continueButton.gameObject.SetActive(message.HasWon);

        canvasGroup.alpha = .0f;
        gameObject.SetActive(true);

        StartCoroutine(FadeIn(onComplete));
    }

    private void Continue()
    {
        Close();
        GameManager.ContinueGame();
    }

    private void Exit()
    {
        GameManager.ExitGame();
    }

    private void Restart()
    {
        Close();
        GameManager.ContinueGame();
    }

    private IEnumerator FadeIn(Action onComplete)
    {

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1.5f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        onComplete?.Invoke();
    }

    private IEnumerator FadeOut(Action onComplete)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 2 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void Start()
    {
        continueButton.onClick.AddListener(Continue);
        restartbutton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);
    }

    private void OnDestroy()
    {
        restartbutton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }
}
