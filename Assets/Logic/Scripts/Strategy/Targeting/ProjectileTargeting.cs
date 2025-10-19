using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class ProjectileTargeting : TargetingStrategy {
    public ProjectileController ProjectilePrefab;
    public GameObject previewInstance;
    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        if (ProjectilePrefab != null) {
            Vector3 flatForward = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)).normalized;
            Quaternion forwardRotation = Quaternion.LookRotation(flatForward);
            ProjectileController projectile = Object.Instantiate(ProjectilePrefab.gameObject,
                new Vector3(caster.GetReferenceTransform().position.x, (caster.GetReferenceTransform().position.y + 1), caster.GetReferenceTransform().position.z),
                forwardRotation).GetComponent<ProjectileController>();
        }
    }
}
