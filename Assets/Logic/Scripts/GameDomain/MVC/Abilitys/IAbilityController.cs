using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;

public interface IAbilityController {
    void InitEntryPoint(INaraController naraController);
    AbilityData[] ActiveAbilities { get; }
    void ChangeActiveSet(int newIndexToActive);
    void CreateAbility(IEffectable caster, int abilitySlotIndex);
    void CreateAbility(IEffectable caster, AbilityData abilityToSpawn);
    int FindIndexAbility(AbilityData abilityToSearch);
    public void NextSet();
    public void PreviousSet();
}
