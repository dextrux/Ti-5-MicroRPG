using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private CharacterView characterView;
    [SerializeField] private int initialHealth = 100;
    [SerializeField] private int initialActionPoints = 10;

    public override void InstallBindings()
    {
        Container.Bind<CharacterModel>().AsSingle().WithArguments(initialHealth, initialActionPoints);
        Container.Bind<CharacterView>().FromInstance(characterView).AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterController>().AsSingle();
    }
}
