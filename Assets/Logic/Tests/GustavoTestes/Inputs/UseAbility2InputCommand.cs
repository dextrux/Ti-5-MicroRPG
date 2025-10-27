using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

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
        if (_castController.TryUseAbility(_abilityController.ActiveAbilities[ONE_INT_CONST], (IEffectable)_naraController)) {
            Debug.Log("Utilizou abilidade slot 2");
        }
    }
}
