using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private RectTransform imgRect;
    [SerializeField] private float duration = 0.5f;

    private Vector3 scaleCache = Vector3.one;
    private CancellationTokenSource flipCTS;

    private void Awake()
    {
        if (imgRect != null)
            scaleCache = imgRect.localScale;
    }

    public void DoFlip(Sprite cardSprite, Action onComplete)
    {
        if (img == null || imgRect == null) return;

        // Cancel any ongoing flip task
        flipCTS?.Cancel();
        flipCTS = new CancellationTokenSource();

        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardFlip);

        _ = DoFlipTask(cardSprite, onComplete, flipCTS.Token);
    }

    private async Task DoFlipTask(Sprite cardSprite, Action onComplete, CancellationToken token)
    {
        try
        {
            float halfDuration = duration / 2f;

            // Let's srink the card x scale to 0
            float t = 0f;
            while (t < halfDuration)
            {
                token.ThrowIfCancellationRequested();

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
                token.ThrowIfCancellationRequested();

                t += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(t / halfDuration);

                scaleCache.x = Mathf.Lerp(0f, 1f, progress);
                imgRect.localScale = scaleCache;

                await Task.Yield();
            }

            scaleCache.x = 1f;
            imgRect.localScale = scaleCache;
        }
        catch (OperationCanceledException)
        {
            scaleCache.x = 1f;
            imgRect.localScale = scaleCache;
        }
        finally
        {
            onComplete?.Invoke();
        }
    }
}
