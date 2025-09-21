using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class CreateCopy1InputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;
    private ICastController _castController;
    private IEchoController _echoController;

    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
        _echoController = _diContainer.Resolve<IEchoController>();
    }

    public void Execute() {
        LogService.Log("Copy1 pressed");
        _castController.UseFastEcho(_echoController, _naraController.NaraViewGO.transform);
    }
}
