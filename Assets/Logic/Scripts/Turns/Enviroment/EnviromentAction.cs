using Zenject;

namespace Logic.Scripts.Turns
{
    public class EnviromentActionService : IEnviromentActionService
    {
		private readonly System.Collections.Generic.IEnumerable<IEnvironmentCommand> _commands;
		private readonly System.Collections.Generic.IEnumerable<IEnvironmentAsyncCommand> _asyncCommands;
		private readonly IEnvironmentActorsRegistry _actorsRegistry;

		public EnviromentActionService(
			System.Collections.Generic.IEnumerable<IEnvironmentCommand> commands,
			System.Collections.Generic.IEnumerable<IEnvironmentAsyncCommand> asyncCommands,
			IEnvironmentActorsRegistry actorsRegistry)
        {
			_commands = commands;
			_asyncCommands = asyncCommands;
			_actorsRegistry = actorsRegistry;
        }

        public async void ExecuteEnviromentTurn()
        {
            await ExecuteEnviromentTurnAsync();
        }

        public async System.Threading.Tasks.Task ExecuteEnviromentTurnAsync()
        {
            UnityEngine.Debug.Log("[Environment] Begin ExecuteEnviromentTurnAsync");
			// Executa comandos síncronos
			if (_commands != null)
            {
				foreach (IEnvironmentCommand command in _commands)
				{
					if (command == null) continue;
					UnityEngine.Debug.Log($"[Environment] Execute command: {command.GetType().Name}");
					command.Execute();
				}
            }
			// Await comandos assíncronos
			int asyncCount = 0;
			if (_asyncCommands != null)
            {
				foreach (IEnvironmentAsyncCommand c in _asyncCommands) asyncCount++;
				UnityEngine.Debug.Log($"[Environment] Awaiting async commands (injected): {asyncCount}");
				foreach (IEnvironmentAsyncCommand command in _asyncCommands)
                {
					if (command == null) continue;
					UnityEngine.Debug.Log($"[Environment] ExecuteAsync command: {command.GetType().Name}");
					await command.ExecuteAsync();
                }
            }
			// Executa atores dinâmicos registrados
			System.Collections.Generic.IReadOnlyList<IEnvironmentTurnActor> snapshot = _actorsRegistry != null ? _actorsRegistry.Snapshot() : null;
			if (snapshot != null && snapshot.Count > 0)
            {
				System.Collections.Generic.List<IEnvironmentTurnActor> toRemove = new System.Collections.Generic.List<IEnvironmentTurnActor>();
				for (int i = 0; i < snapshot.Count; i++)
                {
					IEnvironmentTurnActor actor = snapshot[i];
					if (actor == null) continue;
					UnityEngine.Debug.Log($"[Environment] Execute actor: {actor.GetType().Name}");
					await actor.ExecuteAsync();
					if (actor.RemoveAfterRun) toRemove.Add(actor);
				}
				for (int i = 0; i < toRemove.Count; i++)
				{
					_actorsRegistry.Remove(toRemove[i]);
                }
            }
            UnityEngine.Debug.Log("[Environment] End ExecuteEnviromentTurnAsync");
        }
    }
}
