using UnityEngine;

public class UIBinder : MonoBehaviour
{
    [Header("Referências")]
    public TurnoTatico turnoTatico;
    public UIActionPoints uiActionPoints;
    public UITurnIndicator uiTurnIndicator;
    public UIAbilityPanel uiAbilityPanel;
    public UIHealthBar uiPlayerHealth;
    public UIHealthBar uiBossHealth;
    public UIVictoryDefeat uiVictoryDefeat;

    void Awake()
    {
        if (turnoTatico == null)
        {
            turnoTatico = FindFirstObjectByType<TurnoTatico>();
        }
    }

    void OnEnable()
    {
        if (turnoTatico != null)
        {
            turnoTatico.OnPontosDeAcaoAtualizados += HandlePAUpdated;
            turnoTatico.OnTurnoIniciado += HandleTurnStarted;
            turnoTatico.OnTurnoTerminado += HandleTurnEnded;
        }

        if (uiTurnIndicator != null && turnoTatico != null)
        {
            uiTurnIndicator.BindSkipAction(() => turnoTatico.TerminarTurno());
        }
    }

    void OnDisable()
    {
        if (turnoTatico != null)
        {
            turnoTatico.OnPontosDeAcaoAtualizados -= HandlePAUpdated;
            turnoTatico.OnTurnoIniciado -= HandleTurnStarted;
            turnoTatico.OnTurnoTerminado -= HandleTurnEnded;
        }
    }

    private void HandlePAUpdated(int atuais, int max)
    {
        if (uiActionPoints != null)
        {
            uiActionPoints.SetPoints(atuais, max);
        }
    }

    private void HandleTurnStarted(int current, int total)
    {
        if (uiTurnIndicator != null)
        {
            uiTurnIndicator.SetTurn(current, total, true);
        }

        if (uiAbilityPanel != null)
        {
            uiAbilityPanel.ResetAll();
        }
    }

    private void HandleTurnEnded(int current, int total)
    {
        if (uiTurnIndicator != null)
        {
            uiTurnIndicator.SetTurn(current, total, false);
        }
    }
}


