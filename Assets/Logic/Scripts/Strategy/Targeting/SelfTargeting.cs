using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using UnityEngine;

[Serializable]
public class SelfTargeting : TargetingStrategy {
    public GameObject SelfCastPrefab;
    private GameObject previewInstance;
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
    public override Vector3 LockAim(out IEffectable[] targets) {
        targets = new IEffectable[1];
        targets[0] = Caster;
        Cancel();
        return Caster.GetReferenceTransform().position;
    }
}
