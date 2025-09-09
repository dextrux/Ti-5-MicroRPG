using Zenject;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class ArthurTurnInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TurnEventBus>().AsSingle();

            Container.BindInterfacesAndSelfTo<ActionPointsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EchoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BossActionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurnFlowController>().AsSingle();

        }
    }
}


