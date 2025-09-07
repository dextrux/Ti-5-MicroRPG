using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private ArenaPosReference _arena;

    [SerializeField] private BossAttack[] _attackList;

    BossAttack _currentAttack;

    [ContextMenu("Prepare")]
    public void ContextPrepare()
    {
        PrepareAttack(_attackList[0], transform.position);
    }

    [ContextMenu("Execute")]
    public void ContextExecute()
    {
        ExecuteAttack(_arena);
    }

    public void PrepareAttack(BossAttack attack, Vector3 position)
    {
        _currentAttack = Instantiate(attack, position, Quaternion.identity).GetComponent<BossAttack>();
    }

    public void ExecuteAttack(ArenaPosReference arena)
    {
        if (_currentAttack == null) return;

        _currentAttack.Execute(arena);
        _currentAttack = null;
    }
}
