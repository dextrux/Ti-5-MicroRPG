using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class CameraView : MonoBehaviour
{
    public CinemachineCamera cineCam;

    private CinemachineOrbitalFollow orbital;
    private CameraController controller;

    [Inject]
    public void Construct(CameraController controller)
    {
        this.controller = controller;
    }

    void Awake()
    {
        cineCam = GetComponentInChildren<CinemachineCamera>();
        orbital = cineCam.GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        if (controller == null || orbital == null) return;

        controller.UpdateAngles(Time.deltaTime);

        orbital.HorizontalAxis.Value = controller.HorizontalAngle;
    }
}
