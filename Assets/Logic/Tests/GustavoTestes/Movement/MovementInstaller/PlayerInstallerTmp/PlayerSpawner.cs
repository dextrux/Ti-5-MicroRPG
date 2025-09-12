using UnityEngine;
using Zenject;

public class PlayerSpawner : IInitializable
{
    private readonly DiContainer _container;

    private readonly GameObject _playerPrefab;

    public PlayerSpawner(DiContainer container, [Inject(Id = "PlayerPrefab")] GameObject playerPrefab)
    {
        _container = container;
        _playerPrefab = playerPrefab;
    }

    public void Initialize()
    {
        GameObject player = _container.InstantiatePrefab(_playerPrefab);

        Rigidbody rb = _container.InstantiateComponent<Rigidbody>(player);
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _container.Bind<Rigidbody>().FromInstance(rb).AsSingle();

        RigidbodyMovement movement = _container.InstantiateComponent<RigidbodyMovement>(player);
        _container.Bind<RigidbodyMovement>().FromInstance(movement).AsSingle();
        _container.Bind<IMovement>().FromInstance(movement).AsSingle();

        PlayerInput input = _container.InstantiateComponent<PlayerInput>(player);
        _container.Bind<PlayerInput>().FromInstance(input).AsSingle();

        PlayerController controller = _container.InstantiateComponent<PlayerController>(player);
        _container.Bind<PlayerController>().FromInstance(controller).AsSingle();
    }
}
