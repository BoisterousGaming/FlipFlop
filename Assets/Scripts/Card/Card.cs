using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image img;
    [SerializeField] private CardFlipper cardFlipper;

    private int cardID;
    private bool isFlipped;

    private Sprite frontSprite;
    private Sprite backSprite;

    private Action<Card> onCardTapped;
    private Action<Card> onCardClosed;

    private float cardAutoCloseDelay;
    private CancellationTokenSource autoCloseCTS;

    public int CardID => cardID;
    public bool IsFlipped => isFlipped;

    public void SetCardDetails(int cardID, Sprite frontSprite, Sprite backSprite, float cardAutoCloseDelay)
    {
        this.cardID = cardID;
        this.frontSprite = frontSprite;
        this.backSprite = backSprite;
        this.cardAutoCloseDelay = cardAutoCloseDelay;

        img.sprite = backSprite;
        isFlipped = false;

        autoCloseCTS?.Cancel();
        autoCloseCTS = null;
    }

    public void RegisterCallbacks(Action<Card> onCardTapped, Action<Card> onCardClosed)
    {
        this.onCardTapped = onCardTapped;
        this.onCardClosed = onCardClosed;
    }

    public void ShowCardFront()
    {
        if (isFlipped) return;
        isFlipped = true;

        cardFlipper.DoFlip(frontSprite, () =>
        {
            StartAutoCloseTask();
        });
    }

    public void ShowCardBack()
    {
        if (!isFlipped) return;
        isFlipped = false;

        autoCloseCTS?.Cancel();
        autoCloseCTS = null;

        cardFlipper.DoFlip(backSprite, () => onCardClosed?.Invoke(this));
    }


    private void StartAutoCloseTask()
    {
        // Cancel any existing auto-close task
        autoCloseCTS?.Cancel();
        autoCloseCTS = new CancellationTokenSource();

        _ = AutoCloseTask(autoCloseCTS.Token);
    }

    private async Task AutoCloseTask(CancellationToken token)
    {
        try
        {
            await Task.Delay((int)(cardAutoCloseDelay * 1000), token);

            if (isFlipped && !token.IsCancellationRequested)
            {
                ShowCardBack();
            }
        }
        catch (TaskCanceledException) { }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFlipped) return;
        onCardTapped?.Invoke(this);
    }
}
