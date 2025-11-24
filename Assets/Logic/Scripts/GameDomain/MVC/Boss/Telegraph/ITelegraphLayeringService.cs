namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	public interface ITelegraphLayeringService
	{
		public struct TelegraphLayer
		{
			public int Id;
			public float Y;
			public int QueueAdd;
		}

		TelegraphLayer Register(bool preferTop);
		void Unregister(int id);
	}
}

