using UnityEngine;
using System.Collections.Generic;
using System;
using Logic.Scripts.GameDomain.MVC.Abilitys;

public class AbilityPointService : IAbilityPointService {

    public int advantagePoints;
    public int maxDisadvantagePoints;

    public List<AbilityData> allTrackedAbilities;

    private int currentBalance;
    private int usedDisadvantagePoints;
    private AbilityPointData AbilityPointData;

    public List<AbilityData> AllAbilities => allTrackedAbilities;
    public int CurrentBalance => currentBalance;
    public int UsedDisadvantage => usedDisadvantagePoints;
    public int MaxDisadvantage => maxDisadvantagePoints;
    public int Advantage => advantagePoints;

    public AbilityPointService(List<AbilityData> abilities, AbilityPointData pointData) {
        allTrackedAbilities = abilities;
        AbilityPointData = pointData;
        LoadStats();
    }

    public void RecomputeStats() {
        int totalPointsSpent = 0;
        int totalPointsGained = 0;

        foreach (AbilityData ability in allTrackedAbilities) {
            if (ability == null) continue;
            totalPointsSpent += ability.GetPointsSpent();
            totalPointsGained += ability.GetPointsGained();
        }

        usedDisadvantagePoints = totalPointsGained;

        currentBalance = advantagePoints - totalPointsSpent + totalPointsGained;

        if (currentBalance < 0) {
            currentBalance = 0;
        }
    }

    public bool TryIncreaseStat(AbilityData ability, AbilityStat stat) {
        int cost = 1;
        if (currentBalance < cost) {
            return false;
        }
        if (stat == AbilityStat.Cooldown || stat == AbilityStat.Cost) {
            int currentCooldownModifier = ability.GetModifierStatValue(stat);
            int newModifier = currentCooldownModifier + 1;

            int oldPointsGained = ability.GetPointsGained();

            ability.SetModifierStatValue(stat, newModifier);

            float newFinalValue = ability.GetCurrentStatValue(stat);
            int newPointsGained = ability.GetPointsGained();
            int disadvantageChange = newPointsGained - oldPointsGained;

            if (newFinalValue < 1) {
                ability.SetModifierStatValue(stat, currentCooldownModifier);
                return false;
            }

            if (disadvantageChange > 0 && usedDisadvantagePoints + disadvantageChange > maxDisadvantagePoints) {
                ability.SetModifierStatValue(stat, currentCooldownModifier);
                return false;
            }

            RecomputeStats();
            return true;
        }

        int currentModifier = ability.GetModifierStatValue(stat);
        ability.SetModifierStatValue(stat, currentModifier + 1);

        RecomputeStats();
        return true;
    }

    public bool TryDecreaseStat(AbilityData ability, AbilityStat stat) {
        int currentModifier = ability.GetModifierStatValue(stat);
        int newModifier = currentModifier - 1;

        int oldPointsGained = ability.GetPointsGained();

        ability.SetModifierStatValue(stat, newModifier);

        float newFinalValue = ability.GetCurrentStatValue(stat);
        int newPointsGained = ability.GetPointsGained();
        int disadvantageChange = newPointsGained - oldPointsGained;

        if (newFinalValue < 1) {
            ability.SetModifierStatValue(stat, currentModifier);
            return false;
        }

        if (disadvantageChange > 0 && usedDisadvantagePoints + disadvantageChange > maxDisadvantagePoints) {
            ability.SetModifierStatValue(stat, currentModifier);
            return false;
        }

        RecomputeStats();
        return true;
    }

    public void ResetAllAbilities() {
        foreach (AbilityData ability in allTrackedAbilities) {
            ability.ResetModifiers();
        }
        RecomputeStats();
    }

    #region TempSave
    public void SaveStats() {
        PlayerPrefs.SetInt("AdvantagePoints", Advantage);
        PlayerPrefs.SetInt("DisadvantagePoints", MaxDisadvantage);
        foreach (AbilityData ability in allTrackedAbilities) {
            if (ability == null) continue;
            string abilityKey = ability.name;
            foreach (AbilityStat stat in Enum.GetValues(typeof(AbilityStat))) {
                string playerPrefsKey = abilityKey + "_" + stat.ToString();
                int modifierValue = ability.GetModifierStatValue(stat);
                PlayerPrefs.SetInt(playerPrefsKey, modifierValue);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Habilidades salvas no PlayerPrefs.");
    }

    public void LoadStats() {
        advantagePoints = PlayerPrefs.GetInt("AdvantagePoints", AbilityPointData.StartAdvantagePoints);
        maxDisadvantagePoints = PlayerPrefs.GetInt("DisadvantagePoints", AbilityPointData.StartDisadvantagePoints);
        foreach (AbilityData ability in allTrackedAbilities) {
            if (ability == null) continue;
            string abilityKey = ability.name;
            foreach (AbilityStat stat in Enum.GetValues(typeof(AbilityStat))) {
                string playerPrefsKey = abilityKey + "_" + stat.ToString();
                int loadedValue = PlayerPrefs.GetInt(playerPrefsKey, 0);
                ability.SetModifierStatValue(stat, loadedValue);
            }
        }
        Debug.Log("Habilidades carregadas do PlayerPrefs.");
        RecomputeStats();
    }

    public void DeleteSavedStats() {
        foreach (AbilityData ability in allTrackedAbilities) {
            if (ability == null) continue;
            string abilityKey = ability.name;

            foreach (AbilityStat stat in Enum.GetValues(typeof(AbilityStat))) {
                string playerPrefsKey = abilityKey + "_" + stat.ToString();
                PlayerPrefs.DeleteKey(playerPrefsKey);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Dados de habilidades salvos foram deletados do PlayerPrefs.");
        ResetAllAbilities();
    }
    #endregion
}