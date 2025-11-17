using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class LoadLevelCommand : BaseCommand, ICommandAsync {
    public override void ResolveDependencies() {
        throw new System.NotImplementedException();
    }

    public Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        throw new System.NotImplementedException();
    }
}
