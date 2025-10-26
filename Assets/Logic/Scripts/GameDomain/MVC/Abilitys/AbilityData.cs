using Logic.Scripts.Services.UpdateService;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
    public class AbilityData : ScriptableObject {
        public string Name;
        public string Description;
        public Sprite Icon;
        [SerializeReference] public List<AbilityEffect> Effects;
        [SerializeReference] public TargetingStrategy TargetingStrategy;
        [HideInInspector] public int Damage;
        [HideInInspector] public int Cost;
        [HideInInspector] public int Cooldown;
        [HideInInspector] public int Range;
        [HideInInspector] public int Area;
        [HideInInspector] public int Casts;
        public bool HasActivePlot;
        [SerializeField] private int BaseCost;
        [SerializeField] private int BaseCooldown;
        [SerializeField] private int BaseRange;
        [SerializeField] private int BaseArea;
        [SerializeField] private int BaseCasts;

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
                    Debug.Log("Actual Target :" + target);
                    foreach (AbilityEffect effect in Effects) {
                        Debug.Log("Actual effect: " + effect + " on " + target);
                        effect.Execute(this, caster, target);
                    }
                }
            }
        }
        public void Cancel() {
            TargetingStrategy.Cancel();
        }

        public int GetDamage() {
            return Damage;
        }

        public float GetRange() {
            return BaseRange + (Range / 2.0f);
        }

        public float GetArea() {
            return BaseArea + (Area / 2.0f);
        }

        public int GetCasts() {
            return BaseCasts + Casts;
        }

        public int GetCost() {
            return BaseCost + Cost;
        }

        public int GetCooldown() {
            return BaseCooldown + Cooldown;
        }
    }
}