using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
    public class AbilityData : ScriptableObject {
        public string Name;
        public string Description;
        public int Cost;
        public GameObject HitPreviewPrefab;
        public AnimationClip animationClip;
        [Range(0.1f, 4f)] public float castTime = 2f;
        public ShapeType TypeShape;
        public ShapeTransformType TransformationType;
        //To-Do Adicionar VFXController
        //To-Do Adicionar AbilityModifier
        //To-Do Adicionar audioClip quando tivermos

        [SerializeReference] public List<AbilityEffect> Effects;
    }
}