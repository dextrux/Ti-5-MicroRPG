using System;
using System.Collections.Generic;

public class CharacterModel
{
    public int Health { get; private set; }
    public int ActionPoints { get; private set; }
    public bool IsInvulnerable { get; private set; }
    public List<IStatusEffect> ActiveStates { get; private set; }

    public event Action<int> OnHealthChanged;
    public event Action<int> OnActionPointsChanged;
    public event Action<List<IStatusEffect>> OnStatesChanged;
    public event Action<bool> OnInvulnerabilityChanged;

    public CharacterModel(int initialHealth, int initialActionPoints)
    {
        Health = initialHealth;
        ActionPoints = initialActionPoints;
        IsInvulnerable = false;
        ActiveStates = new List<IStatusEffect>();
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable) return;

        Health -= damage;
        if (Health < 0) Health = 0;
        OnHealthChanged?.Invoke(Health);
    }

    public void Heal(int amount)
    {
        Health += amount;
        OnHealthChanged?.Invoke(Health);
    }

    public void SetInvulnerable(bool value)
    {
        IsInvulnerable = value;
        OnInvulnerabilityChanged?.Invoke(IsInvulnerable);
    }

    public void ChangeActionPoints(int value)
    {
        ActionPoints += value;
        if (ActionPoints < 0) ActionPoints = 0;
        OnActionPointsChanged?.Invoke(ActionPoints);
    }

    public void AddState(IStatusEffect state)
    {
        ActiveStates.Add(state);
        OnStatesChanged?.Invoke(ActiveStates);
    }

    public void RemoveState(IStatusEffect state)
    {
        ActiveStates.Remove(state);
        OnStatesChanged?.Invoke(ActiveStates);
    }
}
