using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;
using UnityEngine;

public class CastController : IUpdatable, ICastController {
    private readonly IUpdateSubscriptionService _updateSubscriptionService;
    ICommandFactory _commandFactory;
    IActionPointsService _actionPointsService;
    AbilityView _currentAbilityView;
    GameObject HitPreviewGO;

    public CastController(IUpdateSubscriptionService updateSubscriptionService, ICommandFactory commandFactory,
        IActionPointsService actionPointsService) {
        _updateSubscriptionService = updateSubscriptionService;
        _commandFactory = commandFactory;
        _actionPointsService = actionPointsService;
    }

    public void ManagedUpdate() {
        Vector3 mousePos = GetMouseWorld();
        Vector3 rot = Quaternion.LookRotation(mousePos).eulerAngles;
        HitPreviewGO.transform.eulerAngles = new Vector3(0, rot.y, 0);
    }

    public bool TryUseAbility(AbilityView abilityView, Transform caster) {
        if (_actionPointsService.CanSpend(abilityView.AbilityData.Cost)) {
            HitPreviewGO = Object.Instantiate(abilityView.AbilityData.HitPreviewPrefab, caster.position, caster.rotation);
            _currentAbilityView = abilityView;
            _updateSubscriptionService.RegisterUpdatable(this);
            return true;
        }
        else {
            return false;
        }
    }

    public void CancelAbilityUse() {
        _updateSubscriptionService.UnregisterUpdatable(this);
        _currentAbilityView = null;
        Object.Destroy(HitPreviewGO);
        HitPreviewGO = null;
    }

    private Vector3 GetMouseWorld() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }

        return Camera.main.transform.forward * 60;
    }

    public void UseAbility(IAbilityController abilityController, Transform caster) {
        if (_currentAbilityView == null) return;
        int index = abilityController.FindIndexAbility(_currentAbilityView);
        if (index < 0) return;
        _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
        abilityController.CreateAbility(caster, index);
        CancelAbilityUse();
    }

    public void UseFastEcho(IEchoController echoController, Transform caster) {
        if (_currentAbilityView == null) return;
        echoController.CreateFastEcho(_currentAbilityView, caster);
        _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
        CancelAbilityUse();
    }

    public void UseSlowEcho(IEchoController echoController, Transform caster) {
        if (_currentAbilityView == null) return;
        echoController.CreateSlowEcho(_currentAbilityView, caster);
        _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
        CancelAbilityUse();
    }
}
