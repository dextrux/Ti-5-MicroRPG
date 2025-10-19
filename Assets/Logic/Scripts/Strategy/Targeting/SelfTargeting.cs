using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.UpdateService;
using System;
using UnityEngine;
using Zenject;

[Serializable]
public class SelfTargeting : TargetingStrategy {
    public GameObject SelfCastPrefab;
    private GameObject previewInstance;
    [Inject]
    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        if (SelfCastPrefab != null) {
            previewInstance = GameObject.Instantiate(SelfCastPrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
        }
    }
    public override void Cancel() {
        base.Cancel();
        if (previewInstance != null) {
            UnityEngine.Object.Destroy(previewInstance);
        }
    }
    protected override void LockAim(out IEffectable target) {
        base.LockAim(out target);
        target = Caster;
    }
    public override void LockAim(out IEffectable[] targets) {
        targets = new IEffectable[1];
        LockAim(out targets[0]);
        Cancel();
    }
}
