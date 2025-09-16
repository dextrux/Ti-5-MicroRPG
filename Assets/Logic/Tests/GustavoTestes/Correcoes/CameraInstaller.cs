using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    public GameObject cameraPrefab;

    public override void InstallBindings()
    {
        Container.Bind<CameraModel>().AsSingle();
        Container.Bind<CameraController>().AsSingle();

        CameraView view = Container.InstantiatePrefabForComponent<CameraView>(cameraPrefab);
        Container.Bind<CameraView>().FromInstance(view).AsSingle();
    }
}
