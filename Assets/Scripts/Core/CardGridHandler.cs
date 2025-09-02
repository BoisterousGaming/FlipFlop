using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardGridHandler : MonoBehaviour
{
    [Min(2)] [SerializeField] private int rows = 2;
    [Min(2)] [SerializeField] private int columns = 2;
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);
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

        var ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        Shuffle(ids);

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

    public void UpdateGridLayout()
    {
        // TODO - Update positions and sizes of all cards to fit container
    }

    private void ClearGrid()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        // TODO - Assign ids to individual cards
    }
}
