using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private PlayerData playerData;

    public override void InstallBindings()
    {
        Container.BindInstance(playerPrefab).WithId("PlayerPrefab");
        Container.Bind<PlayerData>().FromInstance(playerData).AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
    }
}
