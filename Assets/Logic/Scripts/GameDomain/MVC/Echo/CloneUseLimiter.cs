using Logic.Scripts.Turns;

namespace Logic.Scripts.GameDomain.MVC.Echo {
	public interface ICloneUseLimiter {
		bool CanUse();
		void MarkUsed();
		void ResetForPlayerTurn();
	}

	public class CloneUseLimiter : ICloneUseLimiter {
		private bool _usedThisPlayerTurn;

		public bool CanUse() {
			return !_usedThisPlayerTurn;
		}

		public void MarkUsed() {
			_usedThisPlayerTurn = true;
		}

		public void ResetForPlayerTurn() {
			_usedThisPlayerTurn = false;
		}
	}
}


