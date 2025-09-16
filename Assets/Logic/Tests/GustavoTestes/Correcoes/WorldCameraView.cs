using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class WorldCameraView : MonoBehaviour
{
    public CinemachineCamera cineCam;

    private CinemachineOrbitalFollow orbital;
    private WorldCameraController controller;

    public Transform target;

    [Inject]
    public void Construct(WorldCameraController controller)
    {
        this.controller = controller;
    }

    void Awake()
    {
        cineCam = GetComponentInChildren<CinemachineCamera>();
        orbital = cineCam.GetComponent<CinemachineOrbitalFollow>();
        target = controller.GetTarget();
    }

    void Update()
    {
        if (controller == null || orbital == null) return;

        controller.UpdateAngles(Time.deltaTime);

        orbital.HorizontalAxis.Value = controller.HorizontalAngle;

        orbital.FollowTarget.position = target.position;
    }
}
