using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private RectTransform imgRect;
    [SerializeField] private float duration = 0.5f;

    private Vector3 scaleCache = Vector3.one;
    private bool isFlipping;

    private void Awake()
    {
        if (imgRect != null)
            scaleCache = imgRect.localScale;
    }

    public async Task DoFlipAsync(Sprite cardSprite, Action onComplete = null)
    {
        if (isFlipping || img == null || imgRect == null) return;

        isFlipping = true;
        await DoFlipTask(cardSprite, onComplete);
        isFlipping = false;
    }

    private async Task DoFlipTask(Sprite cardSprite, Action onComplete)
    {
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardFlip);

        float halfDuration = duration / 2f;

        // Let's srink the card x scale to 0
        float t = 0f;
        while (t < halfDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / halfDuration);

            scaleCache.x = Mathf.Lerp(1f, 0f, progress);
            imgRect.localScale = scaleCache;
            await Task.Yield();
        }

        // Card Scale is at 0, let's change the sprite to next visible side of the card.
        img.sprite = cardSprite;

        // Let's expand the card x scale to 1
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / halfDuration);

            scaleCache.x = Mathf.Lerp(0f, 1f, progress);
            imgRect.localScale = scaleCache;
            await Task.Yield();
        }

        scaleCache.x = 1f;
        imgRect.localScale = scaleCache;

        onComplete?.Invoke();
    }
}
