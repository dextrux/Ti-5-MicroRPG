using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public interface ICastController {
    public void InitEntryPoint(INaraController naraController);
    public bool TryUseAbility(int index, IEffectable caster);
    public void UseAbility(IEffectable caster);
    public void UseFastEcho(IEchoController echoController, Transform caster);
    public void UseSlowEcho(IEchoController echoController, Transform caster);
    public void CancelAbilityUse();
    public bool GetCanUseAbility();
    public void SetCanUseAbility(bool b);
}
