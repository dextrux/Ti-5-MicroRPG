using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuração")]
    public int maxHealth = 30;
    public int startHealth = 30;

    [Header("UI")]
    public UIHealthBar uiHealthBar;

    public System.Action<int, int> OnHealthChanged; // (current, max)
    public System.Action OnDefeated;

    private int currentHealth;

    void Start()
    {
        currentHealth = Mathf.Clamp(startHealth, 0, Mathf.Max(1, maxHealth));
        if (uiHealthBar != null)
        {
            uiHealthBar.Initialize(currentHealth, maxHealth);
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ModifyHealth(int delta)
    {
        SetHealth(currentHealth + delta);
    }

    public void SetHealth(int value)
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        if (uiHealthBar != null)
        {
            uiHealthBar.SetHealth(currentHealth, maxHealth);
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            OnDefeated?.Invoke();
        }
    }
}


