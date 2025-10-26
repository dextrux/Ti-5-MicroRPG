using UnityEngine;

public class ResetAttackTypeOnExit : StateMachineBehaviour
{
    [SerializeField] private string parameterName = "AKY_AttackType";
    [SerializeField] private int resetValue = 0;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(parameterName, resetValue);
    }
}


