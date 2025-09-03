using System;
using UnityEngine;
using UnityEngine.UI;

public class UiMainMenu : UiCanvas
{
    [SerializeField] private Button startGameBtn;

    public void Initialize(Action onClickStartGame)
    {
        startGameBtn.onClick.AddListener(() =>
        {
            onClickStartGame?.Invoke();
            SetVisibilityState(false);
        });
    }
}
