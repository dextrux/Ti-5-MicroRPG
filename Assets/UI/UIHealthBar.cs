using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    public Image healthFillImage; // Configure como Filled no Editor
    public TextMeshProUGUI healthText;

    private int currentHealth;
    private int maxHealth;

    public void Initialize(int startCurrent, int startMax)
    {
        maxHealth = Mathf.Max(1, startMax);
        currentHealth = Mathf.Clamp(startCurrent, 0, maxHealth);
        Refresh();
    }

    public void SetHealth(int newCurrent, int newMax)
    {
        maxHealth = Mathf.Max(1, newMax);
        currentHealth = Mathf.Clamp(newCurrent, 0, maxHealth);
        Refresh();
    }

    public void ModifyHealth(int delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        Refresh();
    }

    private void Refresh()
    {
        if (healthFillImage != null)
        {
            float fill = maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
            healthFillImage.fillAmount = Mathf.Clamp01(fill);
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}


