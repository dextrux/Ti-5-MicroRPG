using Logic.Scripts.Services.CommandFactory;

public class OnCustomizeInteractionCommand : BaseCommand, ICommandVoid {
    private ICustomizeUIController _customizeUIController;

    public override void ResolveDependencies() {
        _customizeUIController = _diContainer.Resolve<ICustomizeUIController>();
    }

    public void Execute() {
        _customizeUIController.ShowCustomize();
    }
}
