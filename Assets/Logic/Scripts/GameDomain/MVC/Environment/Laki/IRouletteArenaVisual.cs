namespace Logic.Scripts.GameDomain.MVC.Environment.Laki
{
	public interface IRouletteArenaVisual
	{
		void RefreshFrom(RouletteArenaService service);
		void SetEmphasis(System.Collections.Generic.ICollection<int> tileIndices, float t01, float extraIntensity = 0.75f);
	}
}


