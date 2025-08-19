using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITurnIndicator : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI hintText;
    public Button skipButton;
    private System.Action onSkip;

    public void SetTurn(int current, int total, bool isPlayerTurn)
    {
        if (turnText != null)
        {
            turnText.text = $"Turno: {current}/{total}";
        }

        if (hintText != null)
        {
            hintText.text = isPlayerTurn ? "Your turn" : "Enemy turn";
        }

        if (skipButton != null)
        {
            skipButton.interactable = isPlayerTurn;
        }
    }

    public void BindSkipAction(System.Action onSkipAction)
    {
        onSkip = onSkipAction;
        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            if (onSkip != null)
            {
                skipButton.onClick.AddListener(() => onSkip());
            }
        }
    }
}


