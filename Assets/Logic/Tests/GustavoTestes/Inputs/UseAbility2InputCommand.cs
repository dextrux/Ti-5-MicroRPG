using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility2InputCommand : BaseCommand, ICommandVoid {
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private ICastController _castController;
    private const int ONE_INT_CONST = 1;
    public override void ResolveDependencies() {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        if (_abilityController.ActiveAbilities[ONE_INT_CONST].AbilityData.TransformationType == ShapeTransformType.Rotation) {
        _castController.TryUseAbility(_abilityController.ActiveAbilities[ONE_INT_CONST], _naraController.NaraViewGO.transform);
        }
        else {
        _castController.TryUseAbility(_abilityController.ActiveAbilities[ONE_INT_CONST], _naraController.NaraSkillSpotTransform);
        }
        return;
    }
}
