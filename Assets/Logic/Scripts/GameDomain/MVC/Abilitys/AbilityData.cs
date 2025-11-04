// AbilityData.cs
using Logic.Scripts.Services.UpdateService;
using System.Collections.Generic;
using UnityEngine;
using System; // <<< ADICIONADO para Math.Max

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
    public class AbilityData : ScriptableObject {
        public string Name;
        public string Description;
        public Sprite Icon;
        [SerializeField] private AudioClip _sfx;

        public int Damage;
        public int Cooldown;
        public int Cost;
        public int Range;
        public int Casts;
        public int Area;

        [SerializeField] private int _baseDamage;
        [SerializeField] private int _baseCost;
        [SerializeField] private int _baseCooldown;
        [SerializeField] private int _baseRange;
        [SerializeField] private int _baseArea;
        [SerializeField] private int _baseCasts;

        [SerializeReference] public List<AbilityEffect> Effects;
        [SerializeReference] public TargetingStrategy TargetingStrategy;

        public PlotTwistData PlotData;

        //To-Do Adicionar VFXController
        //To-Do Tocar audioClip quando tivermos

        public void SetUp(IUpdateSubscriptionService updateSubscriptionService) {
            TargetingStrategy.SetUp(updateSubscriptionService);
        }

        public void Aim(IEffectable caster) {
            TargetingStrategy.Initialize(this, caster);
        }
        public void Cast(IEffectable caster) {
            IEffectable[] targets;
            Vector3 castPoint = TargetingStrategy.LockAim(out targets);
            foreach (AbilityEffect effect in Effects) {
                effect.SetUp(castPoint);
                if (effect.IsAutoCast) {
                    effect.Execute(this, caster);
                }
                else if (targets != null) {
                    foreach (IEffectable target in targets) {
                        effect.Execute(this, caster, target);
                    }
                }
            }
        }
        public void Cancel() {
            TargetingStrategy.Cancel();
        }

        #region GettersFinalValues
        public int GetDamage() {
            return _baseDamage + Damage;
        }

        public float GetRange() {
            return _baseRange + (Range / 2.0f);
        }

        public float GetArea() {
            return _baseArea + (Area / 2.0f);
        }

        public int GetCasts() {
            return _baseCasts + Casts;
        }

        public int GetCost() {
            return _baseCost - Cost;
        }

        public int GetCooldown() {
            return _baseCooldown - Cooldown;
        }
        #endregion

        #region SuportMethods
        public void ResetModifiers() {
            Damage = 0;
            Cooldown = 0;
            Cost = 0;
            Range = 0;
            Casts = 0;
            Area = 0;
        }

        public int GetPointsSpent() {
            int total = 0;
            total += Math.Max(0, Damage);
            total += Math.Max(0, Cooldown);
            total += Math.Max(0, Cost);
            total += Math.Max(0, Range);
            total += Math.Max(0, Casts);
            total += Math.Max(0, Area);
            return total;
        }

        public int GetPointsGained() {
            int total = 0;
            total += Math.Max(0, -Damage);
            total += Math.Max(0, -Cooldown);
            total += Math.Max(0, -Cost);
            total += Math.Max(0, -Range);
            total += Math.Max(0, -Casts);
            total += Math.Max(0, -Area);
            return total;
        }

        public int GetBaseStatValue(AbilityStat stat) {
            switch (stat) {
                case AbilityStat.Damage: return _baseDamage;
                case AbilityStat.Cooldown: return _baseCooldown;
                case AbilityStat.Cost: return _baseCost;
                case AbilityStat.Range: return _baseRange;
                case AbilityStat.Casts: return _baseCasts;
                case AbilityStat.Area: return _baseArea;
                default: return 0;
            }
        }

        public float GetCurrentStatValue(AbilityStat stat) {
            switch (stat) {
                case AbilityStat.Damage: return GetDamage();
                case AbilityStat.Cooldown: return GetCooldown();
                case AbilityStat.Cost: return GetCost();
                case AbilityStat.Range: return (_baseRange + Range);
                case AbilityStat.Casts: return GetCasts();
                case AbilityStat.Area: return (_baseArea + Area);
                default: return 0;
            }
        }

        public int GetModifierStatValue(AbilityStat stat) {
            switch (stat) {
                case AbilityStat.Damage: return Damage;
                case AbilityStat.Cooldown: return Cooldown;
                case AbilityStat.Cost: return Cost;
                case AbilityStat.Range: return Range;
                case AbilityStat.Casts: return Casts;
                case AbilityStat.Area: return Area;
                default: return 0;
            }
        }

        public void SetModifierStatValue(AbilityStat stat, int newValue) {
            switch (stat) {
                case AbilityStat.Damage: Damage = newValue; break;
                case AbilityStat.Cooldown: Cooldown = newValue; break;
                case AbilityStat.Cost: Cost = newValue; break;
                case AbilityStat.Range: Range = newValue; break;
                case AbilityStat.Casts: Casts = newValue; break;
                case AbilityStat.Area: Area = newValue; break;
            }
        }
        #endregion
    }
}