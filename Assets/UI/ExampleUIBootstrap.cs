using UnityEngine;

// Componente opcional para facilitar testes no protótipo
public class ExampleUIBootstrap : MonoBehaviour
{
    public UIBinder binder;
    public UIAbilityPanel abilityPanel;
    public UIHealthBar playerHealth;
    public UIHealthBar bossHealth;
    public HealthSystem playerHealthSystem;
    public HealthSystem bossHealthSystem;
    public UIVictoryDefeat victoryDefeat;

    [Header("Valores fictícios")]
    public int playerMaxHP = 30;
    public int playerStartHP = 30;
    public int bossMaxHP = 50;
    public int bossStartHP = 50;

    void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.Initialize(playerStartHP, playerMaxHP);
        }
        if (bossHealth != null)
        {
            bossHealth.Initialize(bossStartHP, bossMaxHP);
        }

        if (binder != null && abilityPanel != null)
        {
            abilityPanel.Initialize(binder.turnoTatico);
        }

        // Opcional: configurar sistemas de vida fictícios, se existirem
        if (playerHealthSystem != null)
        {
            playerHealthSystem.maxHealth = playerMaxHP;
            playerHealthSystem.startHealth = playerStartHP;
            playerHealthSystem.uiHealthBar = playerHealth;
        }
        if (bossHealthSystem != null)
        {
            bossHealthSystem.maxHealth = bossMaxHP;
            bossHealthSystem.startHealth = bossStartHP;
            bossHealthSystem.uiHealthBar = bossHealth;
        }

        if (victoryDefeat != null && bossHealthSystem != null && playerHealthSystem != null)
        {
            bossHealthSystem.OnDefeated += () => victoryDefeat.ShowVictory("Você derrotou o Chefe!");
            playerHealthSystem.OnDefeated += () => victoryDefeat.ShowDefeat("Você foi derrotado.");
        }
    }
}


