using UnityEngine;

public class ProjectileLineController : ProjectileController {

    public override void ManagedFixedUpdate() {
        transform.Translate(transform.forward * (InitialSpeed * Time.deltaTime));
    }
}
