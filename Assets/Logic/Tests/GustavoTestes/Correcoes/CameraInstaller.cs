using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    public GameObject cameraPrefab;

    public override void InstallBindings()
    {
        Container.Bind<CameraModel>().AsSingle();

        GameInputActions inputActions = new GameInputActions();
        Container.Bind<GameInputActions>().FromInstance(inputActions).AsSingle();

        Container.Bind<CameraController>().AsSingle();

        var prefabInstance = Container.InstantiatePrefab(cameraPrefab);
        CameraView view = prefabInstance.GetComponentInChildren<CameraView>();
        Container.Inject(view);
        Container.Bind<CameraView>().FromInstance(view).AsSingle();
    }
}
