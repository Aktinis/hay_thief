using UnityEngine;
using UnityEngine.UI;


public sealed class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button button = null;


    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameManager.StartGame(1);
        });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
