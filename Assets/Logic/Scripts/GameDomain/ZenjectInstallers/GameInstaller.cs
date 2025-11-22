using Logic.Scripts.GameDomain.GameInitiator;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.States;
using System.Collections.Generic;
using Zenject;

namespace Logic.Scripts.GameDomain.ZenjectInstallers {
    public class GameInstaller : MonoInstaller {
        public List<AbilityData> Abilities;
        public AbilityPointData PointData;
        public override void InstallBindings() {
            Container.Bind<IGameInitiator>().To<GameInitiator.GameInitiator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<AbilityPointService>().AsSingle().WithArguments(Abilities, PointData).NonLazy();
            Container.BindFactory<GamePlayInitatorEnterData, GamePlayState, GamePlayState.Factory>();
            Container.BindInterfacesTo<LevelsDataService>().AsSingle().NonLazy();
            Container.BindFactory<LobbyInitiatorEnterData, LobbyState, LobbyState.Factory>().AsSingle().NonLazy();
        }
    }
}