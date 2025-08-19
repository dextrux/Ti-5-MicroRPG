using UnityEngine;
using TMPro;

public class UIActionPoints : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    public void SetPoints(int current, int max)
    {
        if (pointsText != null)
        {
            pointsText.text = $"PA: {current}/{max}";
        }
    }
}


