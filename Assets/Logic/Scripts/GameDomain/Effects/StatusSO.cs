using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusSO", menuName = "Scriptable Objects/StatusSO")]
public class StatusSO: ScriptableObject
{
    public string Name;
    public string Description;
    private int Duration;
    private IEffectable Target;
    private Action<IEffectable> EffectAction;

    public void SetStatus(int duration, IEffectable target, Action<IEffectable> tickAction) {
        Duration = duration;
        Target = target;
        EffectAction = null;
        EffectAction += tickAction;
    }
}
