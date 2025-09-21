public interface IEffectable
{
    public void TakeDamage(int damageAmount);
    public void TakeDamagePerTurn(int damageAmount, int duration);
    public void Heal(int healAmount);
    public void HealPerTurn(int healAmount, int duration);
    public void AddShield(int value);
    public void AddShieldPerTurn(int value, int duration);
    public void Stun(int value);
    public void SubtractActionPoints(int value);
    public void SubtractAllActionPoints(int value);
    public void ReduceActionPointsGain(int value);
    public void ReduceActionPointsGainPerTurn(int valueToSubtract, int duration);
    public void IncreaseActionPointsGainPerTurn(int valueToIncrease, int duration);
    public void AddActionPoints(int valueToIncrease);
    public void ReduceMovementPerTurn(int valueToSubtract, int duration);
    public void LimitActionPointUse(int value, int duration);
}
