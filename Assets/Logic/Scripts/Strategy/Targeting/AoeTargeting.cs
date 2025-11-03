using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AoeTargeting : TargetingStrategy {
    public GameObject AoePrefab;
    public float AoeRadius = 5f;
    public LayerMask GroundLayerMask;

    private GameObject previewInstance;

    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        if (AoePrefab != null) {
            previewInstance = GameObject.Instantiate(AoePrefab, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            previewInstance.transform.localScale = new Vector3(data.GetArea(), data.GetArea(), data.GetArea());
        }
        SubscriptionService.RegisterUpdatable(this);
    }

    public override void ManagedUpdate() {
        //To-Do Alterar para new input system
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, GroundLayerMask)) {
            if (previewInstance != null) {
                previewInstance.transform.position = new Vector3(hit.point.x, (hit.point.y + 0.15f), hit.point.z);
            }
        }
    }

    public override void Cancel() {
        if (previewInstance != null) {
            UnityEngine.Object.Destroy(previewInstance);
        }
        base.Cancel();
    }

    public override Vector3 LockAim(out IEffectable[] targets) {
        //To-Do Alterar para new input system
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, GroundLayerMask)) {
            Collider[] colliders = Physics.OverlapSphere(hit.point, AoeRadius + Ability.GetArea());
            List<IEffectable> targetsList = new List<IEffectable>();
            foreach (Collider collider in colliders) {
                if (collider.TryGetComponent<IEffectable>(out IEffectable effectable)) {
                    targetsList.Add(effectable);
                }
            }
            targets = targetsList.ToArray();
            Cancel();
            return hit.point;
        }
        else {
            Cancel();
            return base.LockAim(out targets);
        }
    }
}
