using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayUiBindSO", menuName = "Scriptable Objects/GamePlayUiBindSO")]
public class GamePlayUiBindSO : ScriptableObject {
    public float ActualBosshealthPercent;
    public float PreviewBossHealthPercent;
    public int ActualBossLife;

    public float ActualPlayerLifePercent;
    public float PreviewPlayerLifePercent;
    public int ActualPlayerHealth;
    public int PlayerActionPoints;

    public int Skill1Cost;
    public int Skill2Cost;
    public int Skill3Cost;

    public string Skill1Name;
    public string Skill2Name;
    public string Skill3Name;
}