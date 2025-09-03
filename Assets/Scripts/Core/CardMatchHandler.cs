using System;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchHandler : MonoBehaviour
{
    private List<Card> flippedCards = new();
    private HashSet<int> matchedCardIDs = new();

    private Action<int> onMatch;
    private Action<int, int> onMismatch;
    private Action onAllMatched;

    private CardGridHandler gridHandler;

    public void Initialize(CardGridHandler handler, Action<int> onMatch, Action<int, int> onMismatch, Action onAllMatched, out Action<Card> handleCardTapped)
    {
        gridHandler = handler;
        flippedCards.Clear();
        matchedCardIDs.Clear();

        this.onMatch = onMatch;
        this.onMismatch = onMismatch;
        this.onAllMatched = onAllMatched;

        handleCardTapped = HandleCardTapped;
    }

    private void HandleCardTapped(Card tappedCard)
    {
        // TODO - Write Card tapped logic
    }
}
