using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller {
    [SerializeField] private LobbyUiView _lobbyView;

    public override void InstallBindings() {
        Container.Bind<ILobbyInitiator>().To<LobbyInitiator>().AsSingle().NonLazy();
        Container.Bind<ILobbyController>().To<LobbyUiController>().AsSingle().WithArguments(_lobbyView).NonLazy();
    }
}
