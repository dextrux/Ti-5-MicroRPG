using UnityEngine;

public interface IAbilityController {
    AbilityView[] ActiveAbilities { get; }
    void ChangeActiveSet(int newIndexToActive);
    void CreateAbility(Transform referenceTransform, int abilitySlotIndex);
    int FindIndexAbility(AbilityView abilityViewToSearch);
}
