using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{
    public int CardID => cardID;
    public bool IsFlipped => isFlipped;

    private int cardID;
    private bool isFlipped;
    private bool isFlipping;

    private Sprite frontSprite;
    private Sprite backSprite;

    private Action<Card> onCardTapped;

    [SerializeField] private Image img;
    [SerializeField] private CardFlipper cardFlipper;

    public void SetCardDetails(int cardID, Sprite frontSprite, Sprite backSprite)
    {
        this.cardID = cardID;
        this.frontSprite = frontSprite;
        this.backSprite = backSprite;
        img.sprite = backSprite;
        isFlipped = false;
    }

    public void RegisterOnTapped(Action<Card> callback)
    {
        onCardTapped = callback;
    }

    public void ShowCardFront()
    {
        if (isFlipped) return;
        isFlipped = true;
        isFlipping = true;
        cardFlipper.DoFlip(frontSprite, () =>
        {
            isFlipping = false;
        });
    }

    public void ShowCardBack()
    {
        if (!isFlipped) return;
        isFlipped = false;
        isFlipping = true;
        cardFlipper.DoFlip(backSprite, () =>
        {
            isFlipping = false;
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFlipping || isFlipped) return;
        onCardTapped?.Invoke(this);
    }
}
