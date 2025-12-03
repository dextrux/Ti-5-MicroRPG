using UnityEngine;
using Unity.Cinemachine;

public class WorldCameraView : MonoBehaviour {
    [SerializeField] private CinemachineCamera _cineCam;
    [SerializeField] private Transform _target;
    [SerializeField] private float _velocidade = 50f;
    private float _horizontalAngle = 0f;

    [SerializeField] private CinemachineOrbitalFollow _orbital;

    public void SetNewTarget(Transform target) {
        _orbital = _cineCam.GetComponent<CinemachineOrbitalFollow>();
        _target = target;
        _cineCam.Follow = _target;
    }

    public void UpdateCameraRotation(float mouseDeltaX, float deltaTime) {
        _horizontalAngle += mouseDeltaX * _velocidade * deltaTime;
        _orbital.HorizontalAxis.Value = _horizontalAngle;
        _orbital.FollowTarget.position = _target.position;
    }

    public void SetTargetNull() {
        _target = null;
    }
}
