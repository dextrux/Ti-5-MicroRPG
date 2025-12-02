using UnityEngine;
using Logic.Scripts.Turns;
using Logic.Scripts.GameDomain.MVC.Environment.Orb;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.GameDomain.MVC.Echo {
	public class DestroyEchoAfterDelayAction : IEchoAction {
		private readonly GameObject _echoGo;

		public DestroyEchoAfterDelayAction(GameObject echoGo) {
			_echoGo = echoGo;
		}

		// Executed by EchoService when its delay expires (during EchoesAct phase)
		public void Execute(/*IAbilityController abilityController not used here*/) {
			if (_echoGo != null) {
				Object.Destroy(_echoGo);
			}
			// After this clone is gone, retarget orb back to the player if available.
			try {
				var arena = Object.FindFirstObjectByType<ArenaPosReference>(FindObjectsInactive.Exclude);
				var nara = arena != null ? arena.NaraController : null;
				var playerT = (nara != null && nara.NaraViewGO != null) ? nara.NaraViewGO.transform : null;
				if (playerT != null) {
					OrbController.RetargetAllTo(playerT);
				}
			} catch { }
		}
	}
}



