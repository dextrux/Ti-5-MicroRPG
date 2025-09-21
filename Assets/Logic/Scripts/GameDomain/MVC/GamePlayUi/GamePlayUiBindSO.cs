using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GamePlayUiBindSO", menuName = "Scriptable Objects/GamePlayUiBindSO")]
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
}