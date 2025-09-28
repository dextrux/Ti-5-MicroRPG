using System;
using UnityEngine;

public interface IAbilityController {
    void InitEntryPoint();
    AbilityView[] ActiveAbilities { get; }
    void ChangeActiveSet(int newIndexToActive);
    void CreateAbility(Transform referenceTransform, int abilitySlotIndex);
    void CreateAbility(Transform referenceTransform, AbilityView abilityToSpawn);
    int FindIndexAbility(AbilityView abilityViewToSearch);

    public void NextSet();

    public void PreviousSet();
}
