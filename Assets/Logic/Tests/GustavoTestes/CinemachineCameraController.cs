using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraController : ICameraController
{
    private readonly CinemachineVirtualCamera _vcam;
    private readonly CinemachineOrbitalTransposer _orbital;

    public CinemachineCameraController(CinemachineVirtualCamera vcam)
    {
        _vcam = vcam;
        _orbital = _vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public void Follow(Transform target)
    {
        _vcam.Follow = target;
        _vcam.LookAt = target;
    }

    public void Rotate(Vector2 input)
    {
        if (_orbital == null) return;
        _orbital.m_XAxis.Value += input.x;
    }
}
