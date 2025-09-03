using System;
using UnityEngine;
using UnityEngine.UI;

public class UiMainMenu : UiCanvas
{
    [SerializeField] private Button startGameBtn;

    public void Initialize(Action onClickStartGame)
    {
        SetVisibilityState(true);
        startGameBtn.onClick.AddListener(() =>
        {
            AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.ButtonClick);
            onClickStartGame?.Invoke();
            SetVisibilityState(false);
        });
    }
}
