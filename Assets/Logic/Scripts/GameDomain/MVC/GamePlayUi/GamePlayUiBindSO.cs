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
    [SerializeField] private int _skill4Cost;
    [SerializeField] private int _skill5Cost;
    [SerializeField] private int _skill6Cost;

    [SerializeField] private int _skill1Cooldown;
    [SerializeField] private int _skill2Cooldown;
    [SerializeField] private int _skill3Cooldown;
    [SerializeField] private int _skill4Cooldown;
    [SerializeField] private int _skill5Cooldown;
    [SerializeField] private int _skill6Cooldown;

    [SerializeField] private string _skill1Name;
    [SerializeField] private string _skill2Name;
    [SerializeField] private string _skill3Name;
    [SerializeField] private string _skill4Name;
    [SerializeField] private string _skill5Name;
    [SerializeField] private string _skill6Name;

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
    public int Skill4Cost { get => _skill1Cost; set => _skill4Cost = value; }
    public int Skill5Cost { get => _skill2Cost; set => _skill5Cost = value; }
    public int Skill6Cost { get => _skill3Cost; set => _skill6Cost = value; }

    public int Skill1Cooldown { get => _skill1Cost; set => _skill1Cooldown = value; }
    public int Skill2Cooldown { get => _skill2Cost; set => _skill2Cooldown = value; }
    public int Skill3Cooldown { get => _skill3Cost; set => _skill3Cooldown = value; }
    public int Skill4Cooldown { get => _skill1Cost; set => _skill4Cooldown = value; }
    public int Skill5Cooldown { get => _skill2Cost; set => _skill5Cooldown = value; }
    public int Skill6Cooldown { get => _skill3Cost; set => _skill6Cooldown = value; }

    public string Skill1Name { get => _skill1Name; set => _skill1Name = value; }
    public string Skill2Name { get => _skill2Name; set => _skill2Name = value; }
    public string Skill3Name { get => _skill3Name; set => _skill3Name = value; }
    public string Skill4Name { get => _skill1Name; set => _skill4Name = value; }
    public string Skill5Name { get => _skill2Name; set => _skill5Name = value; }
    public string Skill6Name { get => _skill3Name; set => _skill6Name = value; }
}