using Logic.Scripts.Services.UpdateService;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
    public class AbilityData : ScriptableObject {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [SerializeReference] public List<AbilityEffect> Effects;
        [SerializeReference] public TargetingStrategy TargetingStrategy;
        [SerializeField] private int BaseCost;
        [SerializeField] private int BaseCooldown;
        [SerializeField] private int BaseRange;
        [SerializeField] private int BaseArea;
        [SerializeField] private int BaseCasts;
        private AbilityModifierSO firstAbilityModifier;
        private AbilityModifierSO secondAbilityModifier;
        //private PlotData PlotTwistPrefab
        //To-Do public AnimationClip animationClip;
        //To-Do [Range(0.1f, 4f)] public float castTime = 2f;
        //To-Do Adicionar VFXController
        //To-Do Adicionar audioClip quando tivermos

        public void SetUp(IUpdateSubscriptionService updateSubscriptionService) {
            TargetingStrategy.SetUp(updateSubscriptionService);
        }

        public void Aim(IEffectable caster) {
            Debug.Log("Aiming");
            TargetingStrategy.Initialize(this, caster);
        }
        public void Cast(IEffectable caster) {
            IEffectable[] targets;
            TargetingStrategy.LockAim(out targets);
            Debug.Log("Targets :" + targets.Length);
            if (targets != null) {
                foreach (IEffectable target in targets) {
                    foreach (AbilityEffect effect in Effects) {
                        effect.Execute(this, caster, target);
                    }
                }
            }
        }
        public void Cancel() {
            TargetingStrategy.Cancel();
        }

        public int GetDamage() {
            int value = 0;
            HasModification(ModificationType.Damage, out value);
            return value;
        }

        public int GetRange() {
            int value = 0;
            HasModification(ModificationType.Range, out value);
            return BaseRange + value;
        }

        public int GetArea() {
            int value = 0;
            HasModification(ModificationType.Area, out value);
            return BaseArea + value;
        }

        public int GetCasts() {
            int value = 0;
            HasModification(ModificationType.Cast, out value);
            return BaseCasts + value;
        }

        public int GetCost() {
            int value = 0;
            HasModification(ModificationType.Cost, out value);
            return BaseCost + value;
        }

        public int GetCooldown() {
            int value = 0;
            HasModification(ModificationType.CountdownReduction, out value);
            return BaseCooldown + value;
        }

        public bool TryAddModifier(AbilityModifierSO abilityToAdd) {
            if (firstAbilityModifier == null) {
                firstAbilityModifier = abilityToAdd;
                return true;
            }
            else if (secondAbilityModifier == null) {
                secondAbilityModifier = abilityToAdd;
                return true;
            }
            else {
                return false;
            }
        }

        public bool TryRemoveModifier(AbilityModifierSO abilityToRemove) {
            if (firstAbilityModifier != null && firstAbilityModifier == abilityToRemove) {
                firstAbilityModifier = null;
                return true;
            }
            else if (secondAbilityModifier != null && secondAbilityModifier == abilityToRemove) {
                secondAbilityModifier = null;
                return true;
            }
            else {
                return false;
            }
        }

        public bool HasModification(ModificationType modifiationToSearch, out int value) {
            if (firstAbilityModifier != null) {
                foreach (Modification entry in firstAbilityModifier.Modifications) {
                    if (entry.Type == modifiationToSearch) {
                        value = entry.Value;
                        return true;
                    }
                }
            }
            if (secondAbilityModifier != null) {
                foreach (Modification entry in secondAbilityModifier.Modifications) {
                    if (entry.Type == modifiationToSearch) {
                        value = entry.Value;
                        return true;
                    }
                }
            }
            value = 0;
            return false;
        }
    }
}