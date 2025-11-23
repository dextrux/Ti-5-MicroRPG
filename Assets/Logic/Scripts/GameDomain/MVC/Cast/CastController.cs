using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;
using UnityEngine;

public class CastController : ICastController {
    private readonly IActionPointsService _actionPointsService;
    private readonly IUpdateSubscriptionService _subscriptionService;

    private AbilityData _currentAbility;

    private readonly AbilityData[] _abilities;
    public Transform PlayerTransform;

    private bool _canUseAbility;
	private IEffectable _currentCaster;

    public CastController(IUpdateSubscriptionService updateSubscriptionService, ICommandFactory commandFactory,
        IActionPointsService actionPointsService, AbilityData[] abilities) {
        _subscriptionService = updateSubscriptionService;
        _actionPointsService = actionPointsService;
        _abilities = abilities;
    }
    public void InitEntryPoint(INaraController naraController) {
        PlayerTransform = naraController.NaraViewGO.transform;
        foreach (AbilityData ability in _abilities) {
            ability.SetUp(_subscriptionService);
        }
    }

    public bool TryUseAbility(int index, IEffectable caster) {
        if (_actionPointsService.CanSpend(_abilities[index].GetCost())) {
            _abilities[index].Aim(caster);
            _currentAbility = _abilities[index];
			_currentCaster = caster;
            if (caster is INaraController naraController) {
				int attackType = _abilities[index] != null ? _abilities[index].AnimatorAttackType : 1;
				naraController.PlayAttackType(attackType);
            }
            return true;
        }
        else {
            return false;
        }
    }

    public void CancelAbilityUse() {
		if (_currentCaster is INaraController naraController) {
			naraController.TriggerCancel();
		}
        _currentAbility?.Cancel();
        _currentAbility = null;
		_currentCaster = null;
    }

    public void UseAbility(IEffectable caster)
    {
        if (_currentAbility == null) return;
        _canUseAbility = true;
        _actionPointsService.Spend(_currentAbility.GetCost());
        if (caster is INaraController naraController) {
            naraController.TriggerExecute();
        }
        _currentAbility.Cast(caster);
        CancelAbilityUse();
    }

    public bool GetCanUseAbility()
    {
        return _canUseAbility;
    }

    public void SetCanUseAbility(bool b)
    {
        _canUseAbility = b;
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
