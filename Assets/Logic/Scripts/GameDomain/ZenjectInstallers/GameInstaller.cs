using Logic.Scripts.GameDomain.GameInitiator;
using Logic.Scripts.GameDomain.States;
using Zenject;

namespace Logic.Scripts.GameDomain.ZenjectInstallers {
    public class GameInstaller : MonoInstaller {
        public override void InstallBindings() {
            Container.Bind<IGameInitiator>().To<GameInitiator.GameInitiator>().AsSingle().NonLazy();
            Container.BindFactory<GamePlayInitatorEnterData, GamePlayState, GamePlayState.Factory>();
            Container.BindFactory<LobbyInitiatorEnterData, LobbyState, LobbyState.Factory>().AsSingle().NonLazy();
        }
    }
}