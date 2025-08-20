using UnityEngine;

public class BossSkill : MonoBehaviour {
    public HealthSystem PlayerHealth;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (other.TryGetComponent<HealthSystem>(out HealthSystem playerHealthSystem)) {
                PlayerHealth = playerHealthSystem;
            }
            Debug.Log("Is colliding with Player");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerHealth = null;
            Debug.Log("Is exiting colliding with Player");
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            if (other.gameObject.TryGetComponent<HealthSystem>(out HealthSystem playerHealthSystem)) {
                PlayerHealth = playerHealthSystem;
            }
            Debug.Log("Is colliding with Player");
        }
    }
    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealth = null;
            Debug.Log("Is exiting colliding with Player");
        }
    }
}
