using System;
using TMPro;
using UnityEngine;

public class UiGameplay : UiCanvas
{
    [SerializeField] private TMP_Text scoreTxt;

    public void OnScoreChanged(int score)
    {
        scoreTxt.text = $"Score: {score}";
    }
}
