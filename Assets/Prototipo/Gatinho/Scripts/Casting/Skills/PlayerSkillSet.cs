using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkillSet", menuName = "Scriptable Objects/PlayerSkillSet")]
public class PlayerSkillSet : ScriptableObject
{
    [HideInInspector] public bool able = true;
    [SerializeField] private List<PlayerSkill> _skills;
    
    public List<PlayerSkill> Skills { get { return _skills; } }
}
