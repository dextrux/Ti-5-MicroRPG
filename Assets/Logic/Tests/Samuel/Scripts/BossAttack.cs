using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private AbilityData _abilityData;

    [SerializeField] private CompositedAreaShapeFactory _areaShape;
    private AreaShape _effectArea;

    private ArenaPosReference _arena;
    private IEffectable _castter;

    public void Setup(ArenaPosReference arena, IEffectable castter)
    {
        _arena = arena;
        _castter = castter;

        AreaShape auxShape = _areaShape.CreateAreaShape();

        if (auxShape != null)
            _effectArea = auxShape;
        else
            Debug.LogWarning("Null ShapeArea created. Not allowed!");
    }

    public void Prepare(ArenaPosReference arena)
    {
        _arena = arena;
    }

    public void Execute()
    {
        if (_effectArea == null)
        {
            Debug.LogWarning("The skill cannot have a null ShapeArea!");
            return;
        }

        bool isHit = _effectArea.IsInArea(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena.GetPlayerArenaPosition());
        if (isHit)
        {
            Debug.Log($"BossAttack HIT: {_abilityData?.Name ?? "<no ability>"} by {_castter}");
            _arena.NaraController.ExecuteAbility(_abilityData, _castter);
        }
        else
        {
            Debug.Log($"BossAttack MISS: {_abilityData?.Name ?? "<no ability>"}");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_effectArea == null) return;

        _effectArea.VisualGizmo(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);
    }
}
