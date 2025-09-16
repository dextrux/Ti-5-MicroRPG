using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    public GameObject cameraPrefab;

    public override void InstallBindings()
    {
        Container.Bind<CameraData>().AsSingle();

        GameInputActions inputActions = new GameInputActions();
        Container.Bind<GameInputActions>().FromInstance(inputActions).AsSingle();

        Container.Bind<WorldCameraController>().AsSingle();

        GameObject prefabInstance = Container.InstantiatePrefab(cameraPrefab);
        WorldCameraView view = prefabInstance.GetComponentInChildren<WorldCameraView>();
        Container.Inject(view);
        Container.Bind<WorldCameraView>().FromInstance(view).AsSingle();
    }
}
