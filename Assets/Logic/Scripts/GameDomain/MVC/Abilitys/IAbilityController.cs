using UnityEngine;

public interface IAbilityController {
    void ChangeActiveSet(int newIndexToActive);
    void CreateAbility(Transform referenceTransform, int abilitySlotIndex);
}
