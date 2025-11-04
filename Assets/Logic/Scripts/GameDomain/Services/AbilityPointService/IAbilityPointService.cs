using Logic.Scripts.GameDomain.MVC.Abilitys;
using System.Collections.Generic;

public interface IAbilityPointService {
    List<AbilityData> AllAbilities { get; }
    int CurrentBalance { get; }
    int UsedDisadvantage { get; }
    int MaxDisadvantage { get; }
    int Advantage { get; }
    void RecomputeStats();
    bool TryIncreaseStat(AbilityData ability, AbilityStat stat);
    bool TryDecreaseStat(AbilityData ability, AbilityStat stat);
    void ResetAllAbilities();
    void SaveStats();
    void LoadStats();
    void DeleteSavedStats();
}
