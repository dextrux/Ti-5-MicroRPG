using UnityEngine;
using Zenject;

public class MovementInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IMovement>().To<RigidbodyMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerInput>().FromComponentInHierarchy().AsSingle();
    }
}
