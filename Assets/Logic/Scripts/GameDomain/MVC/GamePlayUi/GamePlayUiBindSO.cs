using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GamePlayUiBindSO", menuName = "Scriptable Objects/UI Bindings/GamePlayUiBindSO")]
public class GamePlayUiBindSO : ScriptableObject {

    [SerializeField] private Length _actualBosshealthPercent;
    [SerializeField] private Length _previewBossHealthPercent;
    [SerializeField] private int _actualBossLife;

    [SerializeField] private Length _actualPlayerLifePercent;
    [SerializeField] private Length _previewPlayerLifePercent;
    [SerializeField] private int _actualPlayerHealth;
    [SerializeField] private int _playerActionPoints;

    [SerializeField] private int _skill1Cost;
    [SerializeField] private int _skill2Cost;
    [SerializeField] private int _skill3Cost;

    [SerializeField] private string _skill1Name;
    [SerializeField] private string _skill2Name;
    [SerializeField] private string _skill3Name;

    public Length ActualBosshealthPercent { get => _actualBosshealthPercent; set => _actualBosshealthPercent = value; }
    public Length PreviewBossHealthPercent { get => _previewBossHealthPercent; set => _previewBossHealthPercent = value; }
    public int ActualBossLife { get => _actualBossLife; set => _actualBossLife = value; }

    public Length ActualPlayerLifePercent { get => _actualPlayerLifePercent; set => _actualPlayerLifePercent = value; }
    public Length PreviewPlayerLifePercent { get => _previewPlayerLifePercent; set => _previewPlayerLifePercent = value; }
    public int ActualPlayerHealth { get => _actualPlayerHealth; set => _actualPlayerHealth = value; }
    public int PlayerActionPoints { get => _playerActionPoints; set => _playerActionPoints = value; }

    public int Skill1Cost { get => _skill1Cost; set => _skill1Cost = value; }
    public int Skill2Cost { get => _skill2Cost; set => _skill2Cost = value; }
    public int Skill3Cost { get => _skill3Cost; set => _skill3Cost = value; }

    public string Skill1Name { get => _skill1Name; set => _skill1Name = value; }
    public string Skill2Name { get => _skill2Name; set => _skill2Name = value; }
    public string Skill3Name { get => _skill3Name; set => _skill3Name = value; }
}