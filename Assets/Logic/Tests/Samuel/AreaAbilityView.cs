using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.CommandFactory;

public class AreaAbilityView : MonoBehaviour
{
    [SerializeField] private AbilityData _data;
    [SerializeField] private CompositedAreaShapeFactory _areaShape;
    private AreaShape _effectArea;

    private ArenaPosReference _arena;

    private IEffectable _castter;
    private readonly ICommandFactory _commandFactory;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
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
            //_commandFactory.CreateCommandVoid<AreaAbilityHitCommand>().SetData(new SkillHitCommandData(_data, _castter));
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_effectArea == null) return;

        _effectArea.VisualGizmo(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);
    }
}
