using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using UnityEngine;

public interface IAbilityController {
    void InitEntryPoint();
    AbilityData[] ActiveAbilities { get; }
    void ChangeActiveSet(int newIndexToActive);
    void CreateAbility(Transform referenceTransform, int abilitySlotIndex);
    void CreateAbility(Transform referenceTransform, AbilityData abilityToSpawn);
    int FindIndexAbility(AbilityData abilityToSearch);
    public void NextSet();
    public void PreviousSet();
}
