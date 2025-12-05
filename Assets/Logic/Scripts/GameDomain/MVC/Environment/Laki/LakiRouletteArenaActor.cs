using System.Threading.Tasks;
using UnityEngine;
using Logic.Scripts.Turns;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.GameDomain.MVC.Environment.Laki
{
	public sealed class LakiRouletteArenaActor : IEnvironmentTurnActor
	{
		private readonly ITurnStateReader _turnState;
		private readonly INaraController _nara;
		private readonly RouletteArenaService _arena;
		private readonly IEffectable _caster;
		private readonly IRouletteArenaVisual _visual;
		private Vector3 _centerWorld;

		public bool RemoveAfterRun => false;

		public LakiRouletteArenaActor(ITurnStateReader turnState, INaraController nara, RouletteArenaService arena, Vector3? centerWorld = null, IRouletteArenaVisual visual = null, IEffectable caster = null)
		{
			_turnState = turnState;
			_nara = nara;
			_arena = arena ?? new RouletteArenaService();
			_visual = visual;
			_caster = caster;
			_centerWorld = centerWorld ?? new Vector3(0f, 7f, 0f);

			int t = _turnState != null ? _turnState.TurnNumber : 0;
			_arena.RerollTiles(t, new System.Random(t * 7919 + 17));
			_visual?.RefreshFrom(_arena);
		}

		public async Task ExecuteAsync()
		{
			int turn = _turnState != null ? _turnState.TurnNumber : 0;

			Vector3 playerPos = (_nara != null && _nara.NaraViewGO != null) ? _nara.NaraViewGO.transform.position : Vector3.zero;
			int tileIndex = _arena.ComputeTileIndex(playerPos, _centerWorld);
			var type = _arena.GetTileEffect(tileIndex);
			string applied = _arena.ApplyEffectToPlayer(_caster, _nara, tileIndex, turn);
			UnityEngine.Debug.Log($"[LakiRouletteArena] Turn={turn} Tile={tileIndex} Type={type} Effect={(applied ?? "None")}");

			try
			{
				var echoes = UnityEngine.Object.FindObjectsByType<Logic.Scripts.GameDomain.MVC.Echo.EchoView>(FindObjectsSortMode.None);
				if (echoes != null && echoes.Length > 0)
				{
					System.Collections.Generic.HashSet<int> cloneTiles = new System.Collections.Generic.HashSet<int>();
					for (int i = 0; i < echoes.Length; i++)
					{
						var e = echoes[i];
						if (e == null) continue;
						int ct = _arena.ComputeTileIndex(e.transform.position, _centerWorld);
						if (ct < 0) continue;
						if (!cloneTiles.Add(ct)) continue;
					}
					foreach (int ct in cloneTiles)
					{
						var ctype = _arena.GetTileEffect(ct);
						string capplied = _arena.ApplyEffectToPlayer(_caster, _nara, ct, turn);
						UnityEngine.Debug.Log($"[LakiRouletteArena][CloneTile] Turn={turn} Tile={ct} Type={ctype} Effect={(capplied ?? "None")}");
					}
				}
			}
			catch { }

			for (int i = 0; i < 3; i++)
			{
				_arena.RandomizeVisualMapping(new System.Random((turn + i + 1) * 104729 + tileIndex));
				_visual?.RefreshFrom(_arena);
				await System.Threading.Tasks.Task.Delay(150);
			}

			int nextTurn = turn + 1;
			_arena.RerollTiles(nextTurn, new System.Random(nextTurn * 7919 + 17));
			_visual?.RefreshFrom(_arena);

			await Task.CompletedTask;
		}

		public void SetCenter(Vector3 centerWorld)
		{
			_centerWorld = centerWorld;
		}
	}
}


