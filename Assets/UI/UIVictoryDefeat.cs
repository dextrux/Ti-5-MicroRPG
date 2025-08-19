using UnityEngine;
using TMPro;

public class UIVictoryDefeat : MonoBehaviour
{
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI defeatText;

    public void ShowVictory(string message = "Vitória!")
    {
        if (victoryText != null) victoryText.text = message;
        if (victoryPanel != null) victoryPanel.SetActive(true);
        if (defeatPanel != null) defeatPanel.SetActive(false);
    }

    public void ShowDefeat(string message = "Derrota!")
    {
        if (defeatText != null) defeatText.text = message;
        if (defeatPanel != null) defeatPanel.SetActive(true);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void HideAll()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
    }
}


