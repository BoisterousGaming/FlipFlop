using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private Button tglBtn;
    [SerializeField] private Image toggleImg;
    [SerializeField] private List<Sprite> onOffSprites = new();

    private void Awake()
    {
        tglBtn.onClick.AddListener(OnClickTglBtn);
    }

    private void Start()
    {
        OnUpdateTglUi(AudioSfxHandler.Instance.IsMute);
    }

    private void OnClickTglBtn()
    {
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.ButtonClick);
        AudioSfxHandler.Instance.IsMute = !AudioSfxHandler.Instance.IsMute;
        OnUpdateTglUi(AudioSfxHandler.Instance.IsMute);
    }

    private void OnUpdateTglUi(bool state)
    {
        toggleImg.sprite = state ? onOffSprites[1] : onOffSprites[0];
    }

    private void OnDestroy()
    {
        tglBtn.onClick.RemoveAllListeners();
    }
}
