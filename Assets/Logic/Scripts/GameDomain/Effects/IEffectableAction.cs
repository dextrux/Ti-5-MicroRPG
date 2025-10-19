using UnityEngine;

public interface IEffectableAction
{
    public void Stun(int value);
    public void ReduceMovementPerTurn(int valueToSubtract, int duration);
    public void AddActionPoints(int valueToIncrease);
    public void SubtractActionPoints(int value);
    public void SubtractAllActionPoints(int value);
    public void ReduceActionPointsGainPerTurn(int valueToSubtract, int duration);
    public void IncreaseActionPointsGainPerTurn(int valueToIncrease, int duration);
    public void LimitActionPointUse(int value, int duration);
}
