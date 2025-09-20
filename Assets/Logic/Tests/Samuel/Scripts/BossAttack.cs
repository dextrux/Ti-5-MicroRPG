using System;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private SkillEffectFactory[] _skillEffects;
    private Action<EffectTarget_TEST, EffectParameter_TEST> _effect;

    [SerializeField] private CompositedAreaShapeFactory _areaShape;
    private AreaShape _effectArea;

    private ArenaPosReference _arena;

    private void Awake()
    {       
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < _skillEffects.Length; i++)
        {
            SkillEffect auxEffect = _skillEffects[i].CreateEffect();

            if (auxEffect != null)
                _effect += auxEffect.Effect;
            else
                Debug.LogWarning("Null SkillEffect created. Not allowed!");
        }

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

        if (_effectArea.IsInArea(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena.GetPlayerArenaPosition()))
        {
            _effect?.Invoke(null, new EffectParameter_TEST());
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_effectArea == null) return;

        _effectArea.VisualGizmo(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);
    }
}
