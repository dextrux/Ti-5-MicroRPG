using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] private PlayerEchoAction _echoAction;
    [SerializeField] private TurnoTatico _taticTurn;
    [SerializeField] private List<PlayerSkillSet> _skillSets;

    [Header("VisualAreas")]
    [SerializeField] private Material _ableToPlaceMaterial;
    [SerializeField] private Material _nonAbleToPlaceMaterial;
    [SerializeField] private Material _echoPlacedMaterial;

    [Header("DurationTimes")]
    [SerializeField] private int _skillDuration;
    [SerializeField] private int _echoShorterDuration = 1;
    [SerializeField] private int _echoLongerDuration = 2;

    [Space]

    [SerializeField, ReadOnly] private int _selectedSetID = 0;
    [SerializeField, ReadOnly] private int _selectedSkillID = 0;

    private void Start()
    {
        InputObserver.Instance.OnNum1Down += () => { SetSkillID(0); };
        InputObserver.Instance.OnNum2Down += () => { SetSkillID(1); };
        InputObserver.Instance.OnNum3Down += () => { SetSkillID(2); };

        InputObserver.Instance.OnEDown += AddSetID;
        InputObserver.Instance.OnQDown += RemoveSetID;

        InputObserver.Instance.OnMouse0Down += MarkArea;
    }

    private void OnDisable()
    {
        InputObserver.Instance.OnNum1Down -= () => { SetSkillID(0); };
        InputObserver.Instance.OnNum2Down -= () => { SetSkillID(1); };
        InputObserver.Instance.OnNum3Down -= () => { SetSkillID(2); };

        InputObserver.Instance.OnEDown -= AddSetID;
        InputObserver.Instance.OnQDown -= RemoveSetID;

        InputObserver.Instance.OnMouse0Down -= MarkArea;
    }

    private void MarkArea()
    {
        if (!_skillSets[_selectedSetID].able)
        {
            Debug.LogWarning("Set disabled");
            return;
        }

        PlayerSkill selectedSkill = GetSelectedSkill();

        PlayerSkill skill = Instantiate(selectedSkill.gameObject, transform.position, transform.rotation).GetComponent<PlayerSkill>();

        InputObserver.Instance.OnMouse0Down -= MarkArea;
        _taticTurn.CanMove = false;

        if (_taticTurn.pontosDeAcao < selectedSkill.Cost)
        {
            skill.VisualArea.material = _nonAbleToPlaceMaterial;
            StartCoroutine(MarkAreaCoroutine(skill, false));
        }
        else
        {
            skill.VisualArea.material = _ableToPlaceMaterial;
            StartCoroutine(MarkAreaCoroutine(skill, true));
        }
    }

    private IEnumerator MarkAreaCoroutine(PlayerSkill skill, bool canCast)
    {
        bool loop = true;

        Action cancelCast = () => { loop = false; Destroy(skill.gameObject); };
        Action castQuickEcho = null;
        Action castlaterEcho = null;
        Action castSelectedSkill = null;

        // Atribui eventos [OnEnable]
        InputObserver.Instance.OnMouse1Down += cancelCast;
        if (canCast)
        {
            castQuickEcho = () => { loop = false; CastQuickEcho(skill); };
            castlaterEcho = () => { loop = false; CastLaterEcho(skill); }; ;
            castSelectedSkill = () => { loop = false; CastSelectedSkill(skill, _skillDuration); };

            InputObserver.Instance.OnCDown += castQuickEcho;
            InputObserver.Instance.OnVDown += castlaterEcho;
            InputObserver.Instance.OnMouse0Down += castSelectedSkill;
        }

        // Update
        while (loop)
        {
            Vector3 mousePos = GetMouseWorld();
            Vector3 rot = Quaternion.LookRotation(mousePos).eulerAngles;
            skill.transform.eulerAngles = new Vector3(0, rot.y, 0);

            yield return null;
        }

        // Limpa atribuições de eventos [OnDisable]
        InputObserver.Instance.OnMouse1Down -= cancelCast;

        InputObserver.Instance.OnCDown -= castQuickEcho;
        InputObserver.Instance.OnVDown -= castlaterEcho;
        InputObserver.Instance.OnMouse0Down -= castSelectedSkill;

        InputObserver.Instance.OnMouse0Down += MarkArea;

        _taticTurn.CanMove = true;
    }

    private Vector3 GetMouseWorld()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Camera.main.transform.forward * 60;
    }

    #region // Cast

    private void CastQuickEcho(PlayerSkill skill)
    {
        CastEcho(skill, _echoShorterDuration);
    }

    private void CastLaterEcho(PlayerSkill skill)
    {
        CastEcho(skill, _echoLongerDuration);
    }

    private void CastEcho(PlayerSkill skill, int duration)
    {
        _taticTurn.pontosDeAcao -= skill.Cost;

        skill.Setup(_taticTurn, duration);

        _skillSets[_selectedSetID].able = false;
        skill.OnCast += () => { _skillSets[_selectedSetID].able = true; };

        _echoAction.PlaceEcho(skill);

        skill.VisualArea.material = _echoPlacedMaterial;
    }

    private void CastSelectedSkill(PlayerSkill skill, int duration)
    {
        _taticTurn.pontosDeAcao -= skill.Cost;
        skill.Setup(_taticTurn, duration);
    }

    #endregion

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
