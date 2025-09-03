using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardGridHandler : MonoBehaviour
{
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);
    [SerializeField] private bool forceSquare = true; // This bool allow to toggle between square vs rectangle card shape
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private List<Sprite> cardFrontSprites = new();

    private RectTransform container;
    private readonly List<Card> cards = new();

    public IReadOnlyList<Card> Cards => cards;

    public int Rows { get; set; }
    public int Columns { get; set; }

    private void Awake()
    {
        container = GetComponent<RectTransform>();
    }

    // Let's Generate a new card grid with matching pairs
    public void GenerateGrid(Action<Card> onCardTapped)
    {
        int totalCards = Rows * Columns;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total cards must be even to form pairs!");
            return;
        }

        ClearGrid();
        cards.Clear();

        // Build shuffled ID list (two of each ID)
        var ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        Shuffle(ids);

        // Instantiate cards / grid elemments
        for (int i = 0; i < totalCards; i++)
        {
            var cardGO = Instantiate(cardPrefab, container);
            var card = cardGO.GetComponent<Card>();
            if (card == null)
            {
                Debug.LogError("Card prefab missing Card script!");
                continue;
            }

            var frontSprite = cardFrontSprites[ids[i] % cardFrontSprites.Count];

            card.SetCardDetails(ids[i], frontSprite, backSprite);
            card.RegisterOnTapped(onCardTapped);

            cards.Add(card);
        }

        UpdateGridLayout();
    }

    // Update positions and sizes of all cards to fit inside the container
    public void UpdateGridLayout()
    {
        if (Rows <= 0 || Columns <= 0) return;

        float containerWidth = container.rect.width;
        float containerHeight = container.rect.height;

        float cellWidth = (containerWidth - (spacing.x * (Columns - 1))) / Columns;
        float cellHeight = (containerHeight - (spacing.y * (Rows - 1))) / Rows;

        // If force square, pick the smaller dimension
        if (forceSquare)
        {
            float cellSize = Mathf.Min(cellWidth, cellHeight);
            cellWidth = cellHeight = cellSize;
        }

        float totalGridWidth = (cellWidth * Columns) + (spacing.x * (Columns - 1));
        float totalGridHeight = (cellHeight * Rows) + (spacing.y * (Rows - 1));

        float startX = -totalGridWidth / 2 + cellWidth / 2;
        float startY = totalGridHeight / 2 - cellHeight / 2;

        // Caches to avoid allocations
        var sizeCache = Vector2.zero;
        var posCache = Vector2.zero;

        sizeCache.x = cellWidth;
        sizeCache.y = cellHeight;

        for (int i = 0; i < cards.Count; i++)
        {
            int row = i / Columns;
            int column = i % Columns;

            var cardRect = cards[i].GetComponent<RectTransform>();
            if (cardRect == null) continue;

            // Set card size
            cardRect.sizeDelta = sizeCache;

            // Set card position
            posCache.x = startX + (cellWidth + spacing.x) * column;
            posCache.y = startY - (cellHeight + spacing.y) * row;
            cardRect.anchoredPosition = posCache;
        }
    }

    // Clear the grid once it's not required
    private void ClearGrid()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public void RemoveCard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
        }
    }
}
