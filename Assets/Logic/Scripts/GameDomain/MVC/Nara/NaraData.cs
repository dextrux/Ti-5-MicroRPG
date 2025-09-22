namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraData {
        private readonly NaraConfigurationSO _naraSO;

        private const int ZERO_INT = 0;

        public int ActualHealth { get; private set; }
        public int PreviewHealth { get; private set; }
        public int ActualShield { get; private set; }
        public int RemainningMovementDistance { get; private set; }

        public NaraData(NaraConfigurationSO naraSO) {
            _naraSO = naraSO;
            ResetData();
        }

        public void ResetData() {
            ActualHealth = _naraSO.MaxHealth;
            ActualShield = ZERO_INT;
            RemainningMovementDistance = _naraSO.InitialMovementDistance;
        }

        public void TakeDamage(int damageAmound) {
            if (damageAmound > ActualShield) {
                damageAmound -= ActualShield;
                ActualShield = ZERO_INT;
                ActualHealth -= damageAmound;
            }
            else {
                ActualShield -= damageAmound;
            }
        }

        public void Heal(int healAmount) {
            ActualHealth += healAmount;
            if (ActualHealth > _naraSO.MaxHealth) {
                ActualHealth = _naraSO.MaxHealth;
            }
        }

        public void AddShield(int value) {
            ActualShield += value;
        }

        public bool IsAlive() {
            return ActualHealth <= 0f;
        }
    }
}