using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using UnityEngine;

public class PreviousAbilitySetInputCommand : BaseCommand, ICommandVoid
{
    IAbilityController _abilityController;
    public override void ResolveDependencies() {
        _abilityController = _diContainer.Resolve<IAbilityController>();
    }

    public void Execute() {
        _abilityController.PreviousSet();
    }
}