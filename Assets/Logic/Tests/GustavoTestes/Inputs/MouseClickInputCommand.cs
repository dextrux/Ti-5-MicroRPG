using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

public class MouseClickInputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;
    private ICastController _castController;
    private IAudioService _audio;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
        _audio = _diContainer.Resolve<IAudioService>();
    }

    public void Execute() {
        _castController.UseAbility((IEffectable)_naraController);
        if (_castController?.GetCanUseAbility() == true) {
            int currentAbilityIndex = _castController.GetAbilityName();
            PlayAbilityAudio(currentAbilityIndex);
            if (_naraController?.NaraMove is NaraTurnMovementController naraTurnMovement) {
                naraTurnMovement.RecalculateRadiusAfterAbility();
                naraTurnMovement.SetMovementRadiusCenter();
                naraTurnMovement.Refresh();
                _castController.SetCanUseAbility(false);
                _naraController.Unfreeeze();
            }
        }
    }

    private void PlayAbilityAudio(int ability)
    {
        if(ability <= -1) return;

        switch (ability)
        {
            case 0: _audio.PlayAudio(AudioClipType.Skill1ImpactSFX, AudioChannelType.Fx);
                break;
            case 1: _audio.PlayAudio(AudioClipType.Skill2ImpactSFX, AudioChannelType.Fx);
                break;
            case 2: _audio.PlayAudio(AudioClipType.Aoe1SFX, AudioChannelType.Fx);
                break;
            case 3: _audio.PlayAudio(AudioClipType.Totem, AudioChannelType.Fx);
                break;
            case 4: _audio.PlayAudio(AudioClipType.Teleport, AudioChannelType.Fx);
                break;
            default: _audio.PlayAudio(AudioClipType.Skill1ImpactSFX, AudioChannelType.Fx);
                break;
        }
    }
}