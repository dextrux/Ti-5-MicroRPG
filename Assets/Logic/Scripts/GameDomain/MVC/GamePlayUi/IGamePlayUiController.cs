namespace Logic.Scripts.GameDomain.MVC.Ui {
    public interface IGamePlayUiController {
        public void OnActualBossHealthChange(int newValue);

        public void OnPreviewBossHealthChange(int newValue);

        public void OnActualBossLifeChange(int newValue);

        public void OnActualPlayerLifePercentChange(int newValue);

        public void OnPreviewPlayerLifePercentChange(int newValue);

        public void OnActualPlayerHealthChange(int newValue);

        public void OnPlayerActionPointsChange(int newValue);

        public void OnSkill1CostChange(int newValue);

        public void OnSkill2CostChange(int newValue);

        public void OnSkill3CostChange(int newValue);

        public void OnSkill1NameChange(string newValue);

        public void OnSkill2NameChange(string newValue);

        public void OnSkill3NameChange(string newValue);
    }
}