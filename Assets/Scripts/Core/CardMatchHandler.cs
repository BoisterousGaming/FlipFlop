using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchHandler : MonoBehaviour
{
    [SerializeField] private float mismatchDelay = 0.5f;
    [SerializeField] private float maxSingleFlipTime = 0.5f;
    [SerializeField] private float destroyDelay = 0.25f;

    private List<Card> flippedCards = new();
    private HashSet<int> matchedCardIDs = new();
    private Coroutine singleFlipTimeoutRoutine;

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
        singleFlipTimeoutRoutine = null;
    }

    private void HandleCardTapped(Card tappedCard)
    {
        if (gridHandler == null || tappedCard.IsFlipped) return;

        if (flippedCards.Count >= 2)
        {
            ForceCloseUnmatchedPair();
        }

        tappedCard.ShowCardFront();
        flippedCards.Add(tappedCard);

        if (flippedCards.Count == 1)
        {
            if (singleFlipTimeoutRoutine != null)
                StopCoroutine(singleFlipTimeoutRoutine);
            singleFlipTimeoutRoutine = StartCoroutine(SingleFlipTimeoutRoutine(tappedCard));
        }
        else if (flippedCards.Count == 2)
        {
            if (singleFlipTimeoutRoutine != null)
            {
                StopCoroutine(singleFlipTimeoutRoutine);
                singleFlipTimeoutRoutine = null;
            }

            StartCoroutine(CheckMatchRoutine());
        }
    }

    private void ForceCloseUnmatchedPair()
    {
        if (flippedCards.Count < 2) return;

        Card first = flippedCards[0];
        Card second = flippedCards[1];

        if (first.CardID != second.CardID)
        {
            first.ShowCardBack();
            second.ShowCardBack();
            onMismatch?.Invoke(first.CardID, second.CardID);
        }
        flippedCards.RemoveRange(0, 2);
        // flippedCards.Clear();
    }

    private IEnumerator SingleFlipTimeoutRoutine(Card firstCard)
    {
        yield return new WaitForSecondsRealtime(maxSingleFlipTime);

        if (flippedCards.Count == 1 && flippedCards[0] == firstCard)
        {
            firstCard.ShowCardBack();
            flippedCards.Clear();
            Debug.Log("Single card auto-closed due to timeout");
        }

        singleFlipTimeoutRoutine = null;
    }

    private IEnumerator CheckMatchRoutine()
    {
        yield return null; // Allow flip animation frame

        Card first = flippedCards[0];
        Card second = flippedCards[1];

        if (first.CardID == second.CardID)
        {
            matchedCardIDs.Add(first.CardID);
            onMatch?.Invoke(first.CardID);

            yield return new WaitForSecondsRealtime(destroyDelay);

            gridHandler.RemoveCard(first);
            gridHandler.RemoveCard(second);
            Destroy(first.gameObject);
            Destroy(second.gameObject);

            if (gridHandler.Cards.Count == 0)
            {
                onAllMatched?.Invoke();
            }
        }
        else
        {
            onMismatch?.Invoke(first.CardID, second.CardID);
            yield return new WaitForSecondsRealtime(mismatchDelay);

            first.ShowCardBack();
            second.ShowCardBack();
        }

        if (flippedCards.Count > 2)
            flippedCards.RemoveRange(0, 2);
        else
            flippedCards.Clear();
    }

}
