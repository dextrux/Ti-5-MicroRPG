using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    public Transform player;
    public TurnoTatico playerTurn;
    public float arenaRadius = 30f;
    public Collider[] BossColliders;

    public enum BossAttack {
        None,
        Dash,
        Cone,
        CirclePlayer,
        CircleSelf,
        DonutSelf,
        Line,
        HalfMoon,
        HalfMoon1,
        HalfMoon2,
        HalfMoon3,
    }

    [System.Serializable]
    public class AttackData {
        public BossAttack attack;
        public int minDamage;
        public int maxDamage;
        public float radius;
        public float radiusInner;
        public float angle;
        public float height;
        public float width;
    }

    public List<AttackData> attacks = new List<AttackData>();

    private Queue<BossAttack> attackSequence = new Queue<BossAttack>();
    private BossAttack preparedAttack = BossAttack.None;
    private BossAttack currentAttack = BossAttack.None;
    private int turnCounter = 0;

    public bool turnoBoss = false;

    private bool canCastHalfMoon = true;

    #region Movement
    public float minDistance = 5f;
    public float speed;
    public float rotSpeed;
    private Vector3 dir;
    private bool needMovePlayer = false;
    private Transform targetMovement;

    private void FixedUpdate() {
        if (needMovePlayer) {
            UpdateDirection(targetMovement);
            SmoothRotate();
            Move();
            if (Vector3.Distance(playerTurn.transform.position, transform.position) < minDistance) {
                int randomlagem = Random.Range(1, 2);
                if (randomlagem == 1) {
                    preparedAttack = BossAttack.Cone;
                    currentAttack = preparedAttack;
                    PrepareAttack(BossAttack.Cone);
                }
                else {
                    preparedAttack = BossAttack.Cone;
                    currentAttack = preparedAttack;
                    PrepareAttack(BossAttack.CircleSelf);
                }
                EndBossTurn();
                needMovePlayer = false;
            }
        }
    }

    private void Move() {
        transform.position += dir * Time.fixedDeltaTime;
    }

    public void UpdateDirection(Transform target) {
        dir = target.position - transform.position;
        if (dir.sqrMagnitude > (minDistance * minDistance)) {
            dir = dir.normalized * speed;
        }
        else {
            dir = Vector3.zero;
        }
    }
    public void SmoothRotate() {
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * rotSpeed);
    }
    #endregion

    void Start() {
        attackSequence.Enqueue(BossAttack.CirclePlayer);
        attackSequence.Enqueue(BossAttack.Dash);
        attackSequence.Enqueue(BossAttack.HalfMoon);
        attackSequence.Enqueue(BossAttack.Cone);
        attackSequence.Enqueue(BossAttack.Line);
        attackSequence.Enqueue(BossAttack.Cone);
        attackSequence.Enqueue(BossAttack.CircleSelf);
        attackSequence.Enqueue(BossAttack.DonutSelf);
        attackSequence.Enqueue(BossAttack.HalfMoon);
        attackSequence.Enqueue(BossAttack.HalfMoon);
        attackSequence.Enqueue(BossAttack.HalfMoon);
        attackSequence.Enqueue(GetRandomAttack());
        attackSequence.Enqueue(GetRandomAttack());
        attackSequence.Enqueue(GetRandomAttack());
        attackSequence.Enqueue(GetRandomAttack());
        playerTurn.OnTurnEnd += StartBossTurn;
    }

    private void StartBossTurn() {
        turnoBoss = true;
        NextTurn();
    }

    private void EndBossTurn() {
        turnoBoss = false;
        playerTurn.IniciarTurno();
    }

    public void NextTurn() {
        if (!turnoBoss) return;

        turnCounter++;

        if (preparedAttack != BossAttack.None) {
            ExecuteAttack(preparedAttack);
            preparedAttack = BossAttack.None;
        }

        if (attackSequence.Count > 0) {
            currentAttack = attackSequence.Dequeue();
        }
        else {
            //Matar o jogador
        }

        StartCoroutine(PrepareAttack(currentAttack));
        Debug.Log($"[Turno {turnCounter}] Boss preparou: {preparedAttack}");
    }

    private void ExecuteAttack(BossAttack atk) {
        AttackData data = attacks.Find(a => a.attack == atk);
        if (data == null) {
            Debug.LogWarning("Ataque não configurado: " + atk);
            return;
        }

        int damage = Random.Range(data.minDamage, data.maxDamage + 1);

        switch (atk) {
            case BossAttack.Dash:

                break;
            case BossAttack.Cone:
                BossColliders[1].gameObject.SetActive(true);
                break;
            case BossAttack.CirclePlayer:
                BossColliders[6].gameObject.SetActive(false);
                break;
            case BossAttack.CircleSelf:
                BossColliders[0].gameObject.SetActive(false);
                break;
            case BossAttack.DonutSelf:
                BossColliders[7].gameObject.SetActive(false);
                break;
            case BossAttack.Line:
                BossColliders[8].gameObject.SetActive(false);
                break;
        }
        canCastHalfMoon = true;
        for (int i = 2; i <= 5; i++) {
            BossColliders[i].gameObject.SetActive(false);
        }
    }

    private bool GetNeedMove() {
        return needMovePlayer;
    }
    private bool GetWaitHalfMoon() {
        return canCastHalfMoon;
    }

    private IEnumerator PrepareAttack(BossAttack atk) {
        preparedAttack = currentAttack;
        AttackData data = attacks.Find(a => a.attack == atk);
        if (data == null) {
            Debug.LogWarning("Ataque não configurado: " + atk);
            EndBossTurn();
            yield return null;
        }
        switch (atk) {
            case BossAttack.Dash:
                targetMovement = playerTurn.transform;
                needMovePlayer = true;
                break;
            case BossAttack.Cone:
                BossColliders[1].gameObject.SetActive(true);
                BossColliders[1].transform.rotation = Quaternion.LookRotation((playerTurn.transform.position - transform.position));
                EndBossTurn();
                break;
            case BossAttack.CirclePlayer:
                BossColliders[6].gameObject.SetActive(true);
                BossColliders[6].transform.position = playerTurn.transform.position;
                EndBossTurn();
                break;
            case BossAttack.CircleSelf:
                BossColliders[0].gameObject.SetActive(true);
                EndBossTurn();
                break;
            case BossAttack.DonutSelf:
                BossColliders[7].gameObject.SetActive(true);
                EndBossTurn();
                break;
            case BossAttack.Line:
                BossColliders[8].gameObject.SetActive(true);
                EndBossTurn();
                break;
            case BossAttack.HalfMoon:
                StartCoroutine(HalfMoonCicle());
                EndBossTurn();
                break;
        }
    }

    private IEnumerator HalfMoonCicle() {
        for (int i = 1; i <= 3; i++) {
            int randomize = Random.Range(1, 4);
            switch (randomize) {
                case 1:
                    BossColliders[2].gameObject.SetActive(true);
                    break;
                case 2:
                    BossColliders[3].gameObject.SetActive(true);
                    break;
                case 3:
                    BossColliders[4].gameObject.SetActive(true);
                    break;
                default:
                    BossColliders[5].gameObject.SetActive(true);
                    break;
            }
            yield return new WaitUntil(GetWaitHalfMoon);
        }
    }

    private BossAttack GetRandomAttack() {
        BossAttack[] options = new BossAttack[] {
            BossAttack.DonutSelf, BossAttack.Cone, BossAttack.Dash,
            BossAttack.Line, BossAttack.CircleSelf
        };
        return options[Random.Range(0, options.Length)];
    }
}
