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
    LayerMask _layerMaskMouse;

    public CastController(IUpdateSubscriptionService updateSubscriptionService, ICommandFactory commandFactory,
        IActionPointsService actionPointsService, LayerMask layerMaskMouse) {
        _updateSubscriptionService = updateSubscriptionService;
        _commandFactory = commandFactory;
        _actionPointsService = actionPointsService;
        _layerMaskMouse = layerMaskMouse;
    }

    public void ManagedUpdate() {
        Vector3 mousePos = GetMouseWorld();
        HitPreviewGO.transform.rotation = Quaternion.LookRotation(new Vector3(mousePos.x, 0, mousePos.z));
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMaskMouse)) {
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
        Debug.Log("Fast EchoCasted");
        if (_actionPointsService.CanSpend(_currentAbilityView.AbilityData.Cost)) {
            echoController.CreateFastEcho(_currentAbilityView, caster);
            _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
            CancelAbilityUse();
        }
    }

    public void UseSlowEcho(IEchoController echoController, Transform caster) {
        if (_currentAbilityView == null) return;
        if (_actionPointsService.CanSpend(_currentAbilityView.AbilityData.Cost)) {
            Debug.Log("Slow EchoCasted");
            echoController.CreateSlowEcho(_currentAbilityView, caster);
            _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
            CancelAbilityUse();
        }
    }
}
