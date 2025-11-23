using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.GameDomain.MVC.Environment.Orb;

namespace Logic.Scripts.Turns
{
    public class TurnsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TurnStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActionPointsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EchoService>().AsSingle();


			Container.BindInterfacesAndSelfTo<EnvironmentActorsRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnviromentActionService>().AsSingle();
            Container.BindInterfacesTo<OrbEnvironmentRule>().AsSingle();

            Container.BindInterfacesAndSelfTo<TurnFlowController>().AsSingle();
        }
    }
}


