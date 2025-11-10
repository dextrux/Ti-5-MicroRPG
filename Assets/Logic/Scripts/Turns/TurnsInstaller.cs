using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.GameDomain.MVC.Environment.Orb;
using Logic.Scripts.Turns.Actors;

namespace Logic.Scripts.Turns
{
    public class TurnsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TurnStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActionPointsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EchoService>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnviromentActionService>().AsSingle();
            Container.BindInterfacesTo<OrbEnvironmentRule>().AsSingle();

			// Novos bindings para o sistema de Atores
			Container.BindInterfacesAndSelfTo<TurnActivityTracker>().AsSingle();
			Container.BindInterfacesAndSelfTo<PlayerTurnGate>().AsSingle();
			Container.BindInterfacesAndSelfTo<DefaultTurnActorProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<TurnFlowController>().AsSingle();
        }
    }
}


