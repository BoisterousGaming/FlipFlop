using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardGridHandler gridHandler;
    [SerializeField] private CardMatchHandler matchHandler;

    [SerializeField][Min(2)] private int rows = 4;
    [SerializeField][Min(2)] private int columns = 4;

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        gridHandler.Rows = rows;
        gridHandler.Columns = columns;

        matchHandler.Initialize(gridHandler, HandleMatch, HandleMismatch, HandleAllMatched, out Action<Card> handleCardTapped);
        gridHandler.GenerateGrid(handleCardTapped);
    }

    private void HandleMatch(int cardID)
    {
        // TODO - Write card match logic
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardMatch);
    }

    private void HandleMismatch(int firstID, int secondID)
    {
        // TODO - Write card miss match logic
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.CardMissMatch);
    }

    private void HandleAllMatched()
    {
        // TODO - Write game over logic
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.GameOver);
    }
}
