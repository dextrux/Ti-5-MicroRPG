using System.Threading;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public interface IGamePlayUiController {

        void SetBossValues(int newValue);
        void SetBossValues(int newPreviewValue, int newActualValue);
        void SetPlayerValues(int newValue);
        void SetPlayerValues(int newPreviewValue, int newActualValue);
        void SetAbilityValues(int ability1Cost, string ability1Name, int ability2Cost, string ability2Name, int ability3Cost, string ability3Name);

        void OnActualBossHealthChange(int newValue);

        void OnPreviewBossHealthChange(int newValue);

        void OnActualBossLifeChange(int newValue);

        void OnActualPlayerLifePercentChange(int newValue);

        void OnPreviewPlayerLifePercentChange(int newValue);

        void OnActualPlayerHealthChange(int newValue);

        void OnPlayerActionPointsChange(int newValue);

        void OnSkill1CostChange(int newValue);

        void OnSkill2CostChange(int newValue);

        void OnSkill3CostChange(int newValue);

        void OnSkill1NameChange(string newValue);

        void OnSkill2NameChange(string newValue);

        void OnSkill3NameChange(string newValue);
    }
}