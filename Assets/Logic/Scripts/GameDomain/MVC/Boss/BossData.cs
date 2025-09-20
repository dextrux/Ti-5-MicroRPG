namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossData
    {
        private readonly BossConfigurationSO _bossSO;
        private const int ZERO_INT = 0;

        public int ActualHealth { get; private set; }
        public int ActualShield { get; private set; }
        public int RemainningMovementDistance { get; private set; }

        public BossData(BossConfigurationSO bossSO)
        {
            _bossSO = bossSO;
            ResetData();
        }

        public void ResetData()
        {
            ActualHealth = _bossSO.MaxHealth;
            ActualShield = ZERO_INT;
            RemainningMovementDistance = _bossSO.InitialMovementDistance;
        }

        public void TakeDamage(int damageAmound)
        {
            if (damageAmound > ActualShield)
            {
                damageAmound -= ActualShield;
                ActualShield = ZERO_INT;
                ActualHealth -= damageAmound;
            }
            else
            {
                ActualShield -= damageAmound;
            }
        }

        public void Heal(int healAmount)
        {
            ActualHealth += healAmount;
            if (ActualHealth > _bossSO.MaxHealth)
            {
                ActualHealth = _bossSO.MaxHealth;
            }
        }

        public void AddShield(int value)
        {
            ActualShield += value;
        }

        public bool IsAlive()
        {
            return ActualHealth <= 0f;
        }
    }
}
