using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class MouseClickInputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;
    private ICastController _castController;
    private IAbilityController _abilityController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
        _abilityController = _diContainer.Resolve<IAbilityController>();
    }

    public void Execute() {
        LogService.Log("Left Mouse button pressed");
        _castController.UseAbility(_abilityController, (IEffectable)_naraController);
        if (_castController.GetCanUseAbility() == true)
        {
            _naraController.SetNewMovementArea();
            _castController.SetCanUseAbility(false);
        }
    }
}