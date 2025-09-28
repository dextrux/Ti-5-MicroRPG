using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;
using System.Collections.Generic;
using UnityEngine;

public class CastController : IUpdatable, ICastController {
    private readonly IUpdateSubscriptionService _updateSubscriptionService;
    private readonly ICommandFactory _commandFactory;
    private readonly IActionPointsService _actionPointsService;
    private AbilityView _currentAbilityView;
    private List<GameObject> _hitPreviewGOs;
    private Transform _fatherObject;
    private LayerMask _layerMaskMouse;

    public CastController(IUpdateSubscriptionService updateSubscriptionService, ICommandFactory commandFactory,
        IActionPointsService actionPointsService, LayerMask layerMaskMouse) {
        _updateSubscriptionService = updateSubscriptionService;
        _commandFactory = commandFactory;
        _actionPointsService = actionPointsService;
        _layerMaskMouse = layerMaskMouse;
    }

    public void ManagedUpdate() {
        Vector3 mousePos = GetMouseWorld();
        _fatherObject.rotation = Quaternion.LookRotation(new Vector3(mousePos.x, 0, mousePos.z));
    }

    public bool TryUseAbility(AbilityView abilityView, Transform caster) {
        if (_actionPointsService.CanSpend(abilityView.AbilityData.Cost)) {
            _hitPreviewGOs = ShapeSpawner.Instance.Spawn(abilityView.AbilityData.HitPreviewPrefab, caster, abilityView.AbilityData.TypeShape);
            _fatherObject = caster;
            foreach (GameObject obj in _hitPreviewGOs) {
                obj.transform.SetParent(_fatherObject);
            }
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
        if (_hitPreviewGOs == null) return;
        foreach (GameObject obj in _hitPreviewGOs) {
            Object.Destroy(obj);
        }
        _hitPreviewGOs = null;
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
        abilityController.CreateAbility(_fatherObject, index);
        CancelAbilityUse();
    }

    public void UseFastEcho(IEchoController echoController, Transform caster) {
        if (_currentAbilityView == null) return;
        if (_actionPointsService.CanSpend(_currentAbilityView.AbilityData.Cost)) {
            echoController.CreateFastEcho(_currentAbilityView, caster);
            _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
            CancelAbilityUse();
        }
    }

    public void UseSlowEcho(IEchoController echoController, Transform caster) {
        if (_currentAbilityView == null) return;
        if (_actionPointsService.CanSpend(_currentAbilityView.AbilityData.Cost)) {
            echoController.CreateSlowEcho(_currentAbilityView, caster);
            _actionPointsService.Spend(_currentAbilityView.AbilityData.Cost);
            CancelAbilityUse();
        }
    }
}
