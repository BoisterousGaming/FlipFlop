using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private CardGridHandler gridHandler;
    [SerializeField] private CardMatchHandler matchHandler;

    [SerializeField][Min(2)] private int rows = 4;
    [SerializeField][Min(2)] private int columns = 4;

    [SerializeField] private float cardAutoCloseDelay = 0.5f;
    [SerializeField] private float cardDestroyDelay = 0.25f;

    private void Start()
    {
        uiManager.MainMenu.Initialize(StartNewGame);
    }

    private void StartNewGame()
    {
        uiManager.Gameplay.SetVisibilityState(true);
        gridHandler.Rows = rows;
        gridHandler.Columns = columns;

        matchHandler.Initialize(gridHandler, cardDestroyDelay, HandleMatch, HandleMismatch, HandleAllMatched, uiManager.Gameplay.OnScoreChanged, out Action<Card> handleCardTapped, out Action<Card> onCardClosed);
        gridHandler.GenerateGrid(handleCardTapped, onCardClosed, cardAutoCloseDelay);
    }

    private void HandleMatch(int cardID)
    {
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardMatch);
    }

    private void HandleMismatch(int firstID, int secondID)
    {
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardMissMatch);
    }

    private void HandleAllMatched()
    {
        uiManager.Gameplay.SetVisibilityState(false);
        uiManager.GameOver.SetVisibilityState(true);
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.GameOver);
    }
}
