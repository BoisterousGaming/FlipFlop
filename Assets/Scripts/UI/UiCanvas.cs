using UnityEngine;

public abstract class UiCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public virtual void SetVisibilityState(bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
}
