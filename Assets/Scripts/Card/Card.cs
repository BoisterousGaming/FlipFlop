using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IPointerDownHandler
{
    private int cardID;
    private bool isFlipped = false;

    private Sprite frontSprite;
    private Sprite backSprite;
    private Image image;

    private Action<Card> onCardTapped;

    void Awake()
    {
        image = GetComponent<Image>();
        ShowBack();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onCardTapped?.Invoke(this);
    }

    private void SetImage(Sprite sprite, bool frontSide)
    {
        image.sprite = sprite;
        isFlipped = frontSide;
        // TODO - Do card flip animation & play flip sound
    }

    public void ShowFront()
    {
        SetImage(frontSprite, true);
    }

    public void ShowBack()
    {
        SetImage(backSprite, false);
    }
}
