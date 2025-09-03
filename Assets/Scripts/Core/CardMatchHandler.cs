using System;
using System.Threading.Tasks;
using UnityEngine;

public class CardMatchHandler : MonoBehaviour
{
    private Action<int> onMatch;
    private Action<int, int> onMismatch;
    private Action onAllMatched;
    private Action<int> onScoreChanged;

    private CardGridHandler gridHandler;
    private Card lastFlippedCard;

    private float cardDestroyDelay;
    private int score = 0;

    public void Initialize(CardGridHandler handler, float cardDestroyDelay, Action<int> onMatch, Action<int, int> onMismatch, Action onAllMatched, Action<int> onScoreChanged, out Action<Card> onCardTapped, out Action<Card> onCardClosed)
    {
        gridHandler = handler;
        lastFlippedCard = null;

        this.onMatch = onMatch;
        this.onMismatch = onMismatch;
        this.onAllMatched = onAllMatched;
        this.onScoreChanged = onScoreChanged;

        this.cardDestroyDelay = cardDestroyDelay;

        onCardTapped = HandleCardTapped;
        onCardClosed = HandleCardClosed;
    }

    private void HandleCardTapped(Card tappedCard)
    {
        tappedCard.ShowCardFront();

        if (lastFlippedCard != null && lastFlippedCard != tappedCard)
        {
            if (lastFlippedCard.IsFlipped && tappedCard.CardID == lastFlippedCard.CardID)
            {
                onMatch?.Invoke(tappedCard.CardID);
                DestroyMatched(lastFlippedCard, tappedCard);

                score += 100;
                onScoreChanged?.Invoke(score);

                lastFlippedCard = null;
                return;
            }
            else
            {
                onMismatch?.Invoke(lastFlippedCard.CardID, tappedCard.CardID);

                score -= 10;
                if (score < 0)
                    score = 0;
                onScoreChanged?.Invoke(score);
            }
        }

        lastFlippedCard = tappedCard;
    }


    private void HandleCardClosed(Card closedCard)
    {
        if (lastFlippedCard == closedCard)
        {
            lastFlippedCard = null;
        }
    }

    private async void DestroyMatched(Card first, Card second)
    {
        await Task.Delay((int)(cardDestroyDelay * 1000));

        gridHandler.RemoveCard(first);
        gridHandler.RemoveCard(second);

        Destroy(first.gameObject);
        Destroy(second.gameObject);

        if (gridHandler.Cards.Count == 0)
            onAllMatched?.Invoke();
    }
}
