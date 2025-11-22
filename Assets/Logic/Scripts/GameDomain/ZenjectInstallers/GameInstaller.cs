using Logic.Scripts.GameDomain.GameInitiator;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.States;
using System.Collections.Generic;
using Zenject;
using Logic.Scripts.Services.AudioService;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Logic.Scripts.GameDomain.ZenjectInstallers {
    public class GameInstaller : MonoInstaller {
        public List<AbilityData> Abilities;
        public AbilityPointData PointData;

        [SerializeField] private AudioClipsScriptableObject _gameplayAudioClips;

        public override void InstallBindings() {
            Container.Bind<IGameInitiator>().To<GameInitiator.GameInitiator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<AbilityPointService>().AsSingle().WithArguments(Abilities, PointData).NonLazy();
            Container.BindFactory<GamePlayInitatorEnterData, GamePlayState, GamePlayState.Factory>();
            Container.BindFactory<LobbyInitiatorEnterData, LobbyState, LobbyState.Factory>().AsSingle().NonLazy();

            Container.Bind<IAudioService>()
                .To<AudioService>()
                .FromComponentInHierarchy()
                .AsSingle()
                .IfNotBound();

            if (_gameplayAudioClips != null) {
                Container.BindInstance(_gameplayAudioClips).IfNotBound();
            }
#if UNITY_EDITOR
            else {
                var pack = AssetDatabase.LoadAssetAtPath<AudioClipsScriptableObject>("Assets/Logic/Scripts/GameDomain/Audio/GameplayAudioClips.asset");
                if (pack != null) Container.BindInstance(pack).IfNotBound();
            }
#endif
        }
    }
}
