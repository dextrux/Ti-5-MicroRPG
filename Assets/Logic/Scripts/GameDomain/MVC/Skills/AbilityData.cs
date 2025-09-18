using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData")]
    class AbilityData : ScriptableObject {
        public string Name;
        public string Description;
        public AnimationClip animationClip;
        [Range(0.1f, 4f)] public float castTime = 2f;
        //Adicionar VFXController
        //Adicionar AbilityModifier
        //Adicionar ShapeCalculator

        [SerializeReference] public List<AbilityEffect> effects;
    }
}