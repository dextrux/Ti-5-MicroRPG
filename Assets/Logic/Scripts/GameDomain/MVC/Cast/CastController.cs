using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;
using System.Collections.Generic;
using UnityEngine;

public class CastController : ICastController {
    private readonly IUpdateSubscriptionService _updateSubscriptionService;
    private readonly ICommandFactory _commandFactory;
    private readonly IActionPointsService _actionPointsService;
    private AbilityData _currentAbility;

    public CastController(IUpdateSubscriptionService updateSubscriptionService, ICommandFactory commandFactory,
        IActionPointsService actionPointsService) {
        _updateSubscriptionService = updateSubscriptionService;
        _commandFactory = commandFactory;
        _actionPointsService = actionPointsService;
    }

    public bool TryUseAbility(AbilityData abilityData, IEffectable caster) {
        if (_actionPointsService.CanSpend(abilityData.GetCost())) {
            abilityData.Aim(caster);
            _currentAbility = abilityData;
            return true;
        }
        else {
            return false;
        }
    }

    public void CancelAbilityUse() {
        _currentAbility?.Cancel();
        _currentAbility = null;
    }

    public void UseAbility(IAbilityController abilityController, Transform caster) {
        if (_currentAbility == null) return;
        int index = abilityController.FindIndexAbility(_currentAbility);
        if (index < 0) return;
        _actionPointsService.Spend(_currentAbility.GetCost());
        //abilityController.CreateAbility(_fatherObject, index);
        CancelAbilityUse();
    }

    public void UseFastEcho(IEchoController echoController, Transform caster) {
        if (_currentAbility == null) return;
        if (_actionPointsService.CanSpend(_currentAbility.GetCost())) {
            //echoController.CreateFastEcho(_currentAbility, caster);
            _actionPointsService.Spend(_currentAbility.GetCost());
            CancelAbilityUse();
        }
    }

    public void UseSlowEcho(IEchoController echoController, Transform caster) {
        if (_currentAbility == null) return;
        if (_actionPointsService.CanSpend(_currentAbility.GetCost())) {
            //echoController.CreateSlowEcho(_currentAbilityView, caster);
            _actionPointsService.Spend(_currentAbility.GetCost());
            CancelAbilityUse();
        }
    }
}
