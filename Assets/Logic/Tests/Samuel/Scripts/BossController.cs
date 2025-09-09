using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private ArenaPosReference _arena;

    [SerializeField] private BossAttack[] _attackList;

    BossAttack _currentAttack;

    [ContextMenu("Prepare")]
    public void ContextPrepare()
    {
        PrepareAttack(_attackList[0], transform.position, _arena);
    }

    [ContextMenu("Execute")]
    public void ContextExecute()
    {
        ExecuteAttack();
    }

    public void PrepareAttack(BossAttack attack, Vector3 position, ArenaPosReference arena)
    {
        _currentAttack = Instantiate(attack, position, transform.rotation).GetComponent<BossAttack>();
        _currentAttack.Prepare(arena);
    }

    public void ExecuteAttack()
    {
        if (_currentAttack == null) return;

        _currentAttack.Execute();
        _currentAttack = null;
    }
}
