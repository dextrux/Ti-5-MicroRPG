using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    Unity.Cinemachine.CinemachineVirtualCamera _virtualCamera;

    public override void InstallBindings()
    {
        Container.Bind<ICameraController>()
                 .To<CinemachineCameraController>()
                 .FromInstance(new CinemachineCameraController(_virtualCamera))
                 .AsSingle();
    }
}
