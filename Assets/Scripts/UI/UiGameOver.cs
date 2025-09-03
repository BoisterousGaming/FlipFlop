using System;
using UnityEngine;
using UnityEngine.UI;

public class UiGameOver : UiCanvas
{
    [SerializeField] private Button reloadGameBtn;

    public void Initialize(Action onClickReloadGame)
    {
        SetVisibilityState(true);
        reloadGameBtn.onClick.AddListener(() =>
        {
            AudioSfxHandler.Instance.PlayAudio(Enums.AudioSfxType.ButtonClick);
            onClickReloadGame?.Invoke();
            SetVisibilityState(false);
        });
    }
}
