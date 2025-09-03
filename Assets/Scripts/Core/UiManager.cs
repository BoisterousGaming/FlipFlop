using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private UiMainMenu uiMainMenu;
    [SerializeField] private UiGameplay uiGameplay;
    [SerializeField] private UiGameOver uiGameOver;

    public UiMainMenu MainMenu => uiMainMenu;
    public UiGameplay Gameplay => uiGameplay;
    public UiGameOver GameOver => uiGameOver;
}
