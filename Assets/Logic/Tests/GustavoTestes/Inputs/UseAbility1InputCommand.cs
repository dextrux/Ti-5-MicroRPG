using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using System.Diagnostics;

public class UseAbility1InputCommand : BaseCommand, ICommandVoid
{
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
        LogService.Log("Is Nara Nul: " + (_castController == null));
        if (_castController.TryUseAbility(_abilityController.ActiveAbilities[ONE_INT_CONST], _naraController.NaraSkillSpotTransform)) {

        }
        return; 
    }
}
