using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class StartLevelCommand : BaseCommand, ICommandAsync {
    public override void ResolveDependencies() {
        
    }

    public async Task<Awaitable> Execute(CancellationTokenSource cancellationTokenSource) {
        await ;
    }
}
