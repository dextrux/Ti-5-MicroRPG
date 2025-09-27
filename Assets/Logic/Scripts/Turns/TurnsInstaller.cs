using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;

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
            Container.BindInterfacesAndSelfTo<TurnFlowController>().AsSingle();
        }
    }
}


