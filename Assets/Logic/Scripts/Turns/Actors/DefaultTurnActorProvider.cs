using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.GameDomain.MVC.Environment.Orb;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.Turns.Actors
{
	public sealed class DefaultTurnActorProvider : ITurnActorProvider
	{
		private readonly IBossController _boss;
		private readonly INaraController _nara;
		private readonly IActionPointsService _ap;
		private readonly IPlayerTurnGate _gate;
		private readonly IEchoService _echoes;

		public DefaultTurnActorProvider(
			IBossController boss,
			INaraController nara,
			IActionPointsService ap,
			IPlayerTurnGate gate,
			IEchoService echoes)
		{
			_boss = boss;
			_nara = nara;
			_ap = ap;
			_gate = gate;
			_echoes = echoes;
		}

		public IReadOnlyList<ITurnActor> GetActorsForPhase(TurnPhase phase)
		{
			switch (phase)
			{
				case TurnPhase.BossAct:
				{
					return new ITurnActor[] { new BossActor(_boss) };
				}
				case TurnPhase.PlayerAct:
				{
					return new ITurnActor[] { new PlayerActor(_nara, _ap, _gate) };
				}
				case TurnPhase.EchoesAct:
				{
					// Placeholder agregador; substitua por actors individuais quando houver
					return new ITurnActor[] { new EchoesAggregatorActor(_echoes) };
				}
				case TurnPhase.EnviromentAct:
				{
					var list = new List<ITurnActor>(32);
					var orbs = OrbController.Instances;
					for (int i = 0; i < orbs.Count; i++)
					{
						var orb = orbs[i];
						if (orb != null) list.Add(new OrbEnvironmentActor(orb));
					}
					return list;
				}
				default:
					return System.Array.Empty<ITurnActor>();
			}
		}
	}
}


