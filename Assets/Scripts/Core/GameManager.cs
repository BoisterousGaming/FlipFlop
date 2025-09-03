using System;
using System.Linq;
using System.Collections.Generic;
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

    private int currenctScore;

    private void Start()
    {
        uiManager.MainMenu.Initialize(StartGame);
    }

    private void StartGame()
    {
        SaveData saveData = SaveLoadManager.LoadGame();

        if (saveData != null)
        {
            currenctScore = saveData.score;
            uiManager.Gameplay.SetVisibilityState(true);
            uiManager.Gameplay.OnScoreChanged(currenctScore);
            matchHandler.Initialize(gridHandler, cardDestroyDelay, HandleMatch, HandleMismatch, HandleAllMatched, (score) =>
            {
                currenctScore = score;
                uiManager.Gameplay.OnScoreChanged(score);
            },
            out Action<Card> handleCardTapped, out Action<Card> onCardClosed);
            gridHandler.GenerateGridFromData(
            saveData.gridData,
            handleCardTapped,
            onCardClosed,
            cardAutoCloseDelay
        );
        }
        else
        {
            uiManager.Gameplay.SetVisibilityState(true);
            gridHandler.Rows = rows;
            gridHandler.Columns = columns;

            matchHandler.Initialize(gridHandler, cardDestroyDelay, HandleMatch, HandleMismatch, HandleAllMatched, (score) =>
            {
                currenctScore = score;
                uiManager.Gameplay.OnScoreChanged(score);
            },
            out Action<Card> handleCardTapped, out Action<Card> onCardClosed);
            GridData gridData = gridHandler.GenerateGrid(
            handleCardTapped,
            onCardClosed,
            cardAutoCloseDelay
        );
        }
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
        Debug.Log("HandleAllMatched");
        uiManager.Gameplay.SetVisibilityState(false);
        uiManager.GameOver.SetVisibilityState(true);
        AudioSfxHandler.Instance.PlayAudioOneShot(Enums.AudioSfxType.GameOver);
        SaveLoadManager.ClearSave();
    }

    private void OnApplicationQuit()
    {
        SaveCurrentGame();
    }

    private void SaveCurrentGame()
    {
        GridData gridData = gridHandler.GetCurrentGridData(
            new List<int>(matchHandler.MatchedCardIDs)
        );

        SaveLoadManager.SaveGame(currenctScore, gridData);
    }
}
