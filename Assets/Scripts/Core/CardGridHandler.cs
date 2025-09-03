using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardGridHandler : MonoBehaviour
{
    [Min(2)] [SerializeField] private int rows = 2;
    [Min(2)] [SerializeField] private int columns = 2;
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);
    [SerializeField] private bool forceSquare = true; // This bool allow to toggle between square vs rectangle card shape
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private List<Sprite> cardFrontSprites = new();

    private RectTransform container;
    private readonly List<Card> cards = new();

    public IReadOnlyList<Card> Cards => cards;

    private void Awake()
    {
        container = GetComponent<RectTransform>();
    }

    // Let's Generate a new card grid with matching pairs
    public void GenerateGrid(System.Action<int> onCardTapped)
    {
        int totalCards = rows * columns;
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
        if (rows <= 0 || columns <= 0) return;

        float containerWidth = container.rect.width;
        float containerHeight = container.rect.height;

        float cellWidth = (containerWidth - (spacing.x * (columns - 1))) / columns;
        float cellHeight = (containerHeight - (spacing.y * (rows - 1))) / rows;

        // If force square, pick the smaller dimension
        if (forceSquare)
        {
            float cellSize = Mathf.Min(cellWidth, cellHeight);
            cellWidth = cellHeight = cellSize;
        }

        float totalGridWidth = (cellWidth * columns) + (spacing.x * (columns - 1));
        float totalGridHeight = (cellHeight * rows) + (spacing.y * (rows - 1));

        float startX = -totalGridWidth / 2 + cellWidth / 2;
        float startY = totalGridHeight / 2 - cellHeight / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            int row = i / columns;
            int column = i % columns;

            var cardRect = cards[i].GetComponent<RectTransform>();
            if (cardRect == null) continue;

            cardRect.sizeDelta = new Vector2(cellWidth, cellHeight);

            float xPos = startX + (cellWidth + spacing.x) * column;
            float yPos = startY - (cellHeight + spacing.y) * row;

            cardRect.anchoredPosition = new Vector2(xPos, yPos);
        }
    }

    // Clear the grid once it's not required
    private void ClearGrid()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
