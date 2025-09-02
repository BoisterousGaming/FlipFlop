using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerDownHandler
{
    private int cardID;
    private bool isFlipped;

    private Sprite frontSprite;
    private Sprite backSprite;
    private Image image;

    private Action<Card> onCardTapped;

    [SerializeField] private CardFlipper cardFlipper;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetCardSpriteReferences(Sprite frontSide, Sprite backSide)
    {
        frontSprite = frontSide;
        backSprite = backSide;
        image.sprite = backSprite;
    }

    public void ShowFront()
    {
        isFlipped = true;
        _ = cardFlipper.DoFlipAsync(frontSprite);
    }

    public void ShowBack()
    {
        isFlipped = false;
        _ = cardFlipper.DoFlipAsync(backSprite);
    }

    #region IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        onCardTapped?.Invoke(this);
    }
    #endregion
}
