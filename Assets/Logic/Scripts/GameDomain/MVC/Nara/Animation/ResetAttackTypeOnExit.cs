using UnityEngine;

public class ResetAttackTypeOnExit : StateMachineBehaviour
{
    [SerializeField] private string parameterName = "AKY_AttackType";
    [SerializeField] private int resetValue = 0;
    [SerializeField] private bool resetExecuteTrigger = true;
    [SerializeField] private bool resetCancelTrigger = true;
    [SerializeField] private string executeTriggerName = "Execute";
    [SerializeField] private string cancelTriggerName = "Cancel";

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(parameterName, resetValue);
        if (resetExecuteTrigger && !string.IsNullOrEmpty(executeTriggerName)) {
            animator.ResetTrigger(executeTriggerName);
        }
        if (resetCancelTrigger && !string.IsNullOrEmpty(cancelTriggerName)) {
            animator.ResetTrigger(cancelTriggerName);
        }
    }
}


