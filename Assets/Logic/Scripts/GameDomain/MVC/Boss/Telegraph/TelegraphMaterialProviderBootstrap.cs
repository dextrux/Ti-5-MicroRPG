using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss.Telegraph
{
	// Publica o provider globalmente após todos os installers concluírem.
	public sealed class TelegraphMaterialProviderBootstrap : IInitializable
	{
		private readonly ITelegraphMaterialProvider _provider;

		public TelegraphMaterialProviderBootstrap(ITelegraphMaterialProvider provider)
		{
			_provider = provider;
		}

		public void Initialize()
		{
			TelegraphMaterialService.Provider = _provider;
			UnityEngine.Debug.Log("[TelegraphProvider] Published provider to TelegraphMaterialService");
		}
	}
}


