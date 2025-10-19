using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Echo;
using UnityEngine;

public interface ICastController {
    public bool TryUseAbility(AbilityData abilityData, IEffectable caster);
    public void UseAbility(IAbilityController abilityController, Transform caster);
    public void UseFastEcho(IEchoController echoController, Transform caster);
    public void UseSlowEcho(IEchoController echoController, Transform caster);
    public void CancelAbilityUse();
}
