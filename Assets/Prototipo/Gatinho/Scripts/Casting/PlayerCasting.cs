using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] private PlayerEchoAction _echoAction;
    [SerializeField] private List<PlayerSkillSet> _skillSets;
    [SerializeField] private int _skillDuration;
    [SerializeField] private int _echoDuration;

    [Space]

    [SerializeField, ReadOnly] private int _selectedSetID = 0;
    [SerializeField, ReadOnly] private int _selectedSkillID = 0;

    private void OnEnable()
    {
        InputObserver.Instance.OnNum1Down += () => { SetSkillID(0); };
        InputObserver.Instance.OnNum2Down += () => { SetSkillID(1); };
        InputObserver.Instance.OnNum3Down += () => { SetSkillID(2); };

        InputObserver.Instance.OnQDown += RemoveSetID;
        InputObserver.Instance.OnEDown += AddSetID;

        InputObserver.Instance.OnSpaceDown += CastEcho;

        InputObserver.Instance.OnMouse0Down += CastSelectedSkill;
    }

    private void OnDisable()
    {
        InputObserver.Instance.OnNum1Down -= () => { SetSkillID(0); };
        InputObserver.Instance.OnNum2Down -= () => { SetSkillID(1); };
        InputObserver.Instance.OnNum3Down -= () => { SetSkillID(2); };

        InputObserver.Instance.OnQDown -= RemoveSetID;
        InputObserver.Instance.OnEDown -= AddSetID;

        InputObserver.Instance.OnSpaceDown -= CastEcho;

        InputObserver.Instance.OnMouse0Down -= CastSelectedSkill;
    }

    private void CastEcho()
    {
        if (!_skillSets[_selectedSetID].able)
        {
            Debug.LogWarning("Set dissabled");
            return;
        }

        PlayerSkill selectedSkill = GetSelectedSkill();
        PlayerSkill skillInstance = Instantiate(selectedSkill.gameObject, transform.position, transform.rotation).GetComponent<PlayerSkill>();
        skillInstance.Setup(_echoDuration);

        _skillSets[_selectedSetID].able = false;
        skillInstance.OnCast += () => { _skillSets[_selectedSetID].able = true; };

        _echoAction.PlaceEcho(skillInstance);
    }

    private void CastSelectedSkill()
    {
        if (!_skillSets[_selectedSetID].able)
        {
            Debug.LogWarning("Set dissabled");
            return;
        }

        PlayerSkill skill = GetSelectedSkill();       
        PlayerSkill skillInstance = Instantiate(skill.gameObject, transform.position, transform.rotation).GetComponent<PlayerSkill>();
        skillInstance.Setup(_skillDuration);
    }

    #region // Selected ID

    public PlayerSkill GetSelectedSkill()
    {
        if (_selectedSetID >= _skillSets.Count)
        {
            Debug.LogWarning("Skillset ID out of range");
            return null;
        }

        if (_selectedSkillID >= _skillSets[_selectedSetID].Skills.Count)
        {
            Debug.LogWarning("Skill ID out of range");
            return null;
        }

        return _skillSets[_selectedSetID].Skills[_selectedSkillID];
    }

    private void SetSkillID(int id)
    {
        _selectedSkillID = id;
    }

    private void AddSetID()
    {
        AddID(ref _selectedSetID, _skillSets.Count);
        SetSkillID(0);
    }

    private void RemoveSetID()
    {
        RemoveID(ref _selectedSetID, _skillSets.Count);
        SetSkillID(0);
    }
    
    private void AddID(ref int id, int limit)
    {
        if (id + 1 >= limit)
            id = 0;
        else
            id++;
    }

    private void RemoveID(ref int id, int limit)
    {
        if (id - 1 < 0)
            id = limit - 1;
        else
            id--;
    }

    #endregion
}
