using Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class BaseMonster : Monsters
{
    #region Components

    protected Transform enemyTrf; // ����(�ڽ�)�� ��ġ(Transform)
    protected Transform playerTrf; // �÷��̾��� ��ġ(Transform)
    protected NavMeshAgent agent; // �׺���̼�(NavMesh)
    protected Animator animator; // �ִϸ�����(Animator)
    protected Rigidbody rb; // ������ٵ�(Rigidbody)
    protected Collider cd; // �ݶ��̴�(Collider)

    #endregion Components

    #region Fields

    // ������ ���¸� ��Ÿ���� enum (���� ����)
    protected enum MonsterState
    {
        IDLE, TRACE, ATTACK, DEAD
    }
    protected MonsterState state = MonsterState.IDLE; // ó������ IDLE �����̴�.

    // ���� ���� ��� Ŭ����
    private StateMachine stateMachine;

    // ������ ��� / ���� ����
    protected bool isDie = false;
    protected bool isPlayer = false;

    // �ִϸ������� �ؽ�(Hash), �� ���� Ŭ�������� �˸°� �߰� ������ ��.
    protected readonly int hashTrace = Animator.StringToHash("IsTrace");
    protected readonly int hashAttack = Animator.StringToHash("IsAttack");
    protected readonly int hashDie = Animator.StringToHash("IsDie");

    // �÷��̾� ������ �޾ƾ� NevMesh�� ���� ������ ������.
    [SerializeField] private PlayerController player;

    #endregion Fields

    #region Skills / Stats

    protected float mstATK;
    protected float mstSPD;

    // ������ ��ų ���� ��� �ð�
    protected float attackCooltime;
    protected float attack_curCooltime = 0f;
    protected float skill1Cooltime;
    protected float skill1_curCooltime = 0f;
    protected float skill2Cooltime;
    protected float skill2_curCooltime = 0f;
    protected float skill3Cooltime;
    protected float skill3_curCooltime = 0f;

    protected float traceDistance;
    protected float skillDistance;
    protected float attackDistance;

    #endregion Skills / Stats

    #region Life Cycles (Awake / Start / Update)

    protected override void Awake()
    {
        // ������Ʈ�� �ʱ�ȭ�Ѵ�.
        player = FindObjectOfType<PlayerController>();
        playerTrf = player?.transform;
        enemyTrf = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<Collider>();

        // ���� ��踦 �ʱ�ȭ�Ѵ�.
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(MonsterState.IDLE, new IdleState(this));
        stateMachine.AddState(MonsterState.TRACE, new TraceState(this));
        stateMachine.AddState(MonsterState.ATTACK, new AttackState(this));
        stateMachine.AddState(MonsterState.DEAD, new DeadState(this));
        stateMachine.InitState(MonsterState.IDLE);

        // NavMesh�� �ʱ�ȭ�Ѵ�.
        //agent.destination = playerTrf.position;

        // ������ ��ų�� �ʱ�ȭ�Ѵ�.
        InitSkills();
    }

    protected override void Start()
    {
        base.Start();
        // ������ ���� �˻縦 �����Ѵ�.
        StartCoroutine(CheckEnemyState());
    }

    private void Update()
    {
        // ��ų�� ���� ��� �ð��� ����Ѵ�.
        CalcCooltime();

        if(transform.parent != null)
        {
            animator.SetFloat("FloatX", player.MoveDir.normalized.x);
            animator.SetFloat("FloatY", player.MoveDir.normalized.z);
        }
    }

    #endregion Life Cycles (Awake / Start / Update)

    #region State Classes

    // ���¸� Ȯ���ϴ� �ڷ�ƾ �Լ�
    private IEnumerator CheckEnemyState()
    {
        // ���� �ʾҰų�, ���ǵ��� �ʾҴٸ� ���� �ݺ��Ѵ�.
        while (!isDie)
        {
            //���� �����϶� ���ǰ� ������ ������ ���� �ɾ���� ��.

            // isDie, isPlayer üũ
            isDie = (state == MonsterState.DEAD);
            isPlayer = (gameObject.transform.parent != null);

            // �� 0.3���� ��� �ð�
            yield return new WaitForSeconds(0.3f);

            // ������ ���°� DEAD���,
            if (state == MonsterState.DEAD)
            {
                // ���� ����� ���¸� DEAD�� �ٲ۴�.
                stateMachine.ChangeState(MonsterState.DEAD);
                state = MonsterState.DEAD;
                // ��� Ż���Ѵ�.
                yield break;
            }

            // ����(�ڽ�)�� �÷��̾��� �Ÿ��� ����Ѵ�.
            float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

            // Ư�� ���ݰ��� ����
            bool is_pjtAtk = skillDistance > distance && skill1_curCooltime <= 0f;

            if (distance <= attackDistance || is_pjtAtk) // ���� ������ ���¶�� (���� �Ÿ� �̳�)
            {
                stateMachine.ChangeState(MonsterState.ATTACK); // ���� ���·� �����Ѵ�.
                state = MonsterState.ATTACK;

                yield return new WaitForSeconds(attackCooltime); // ���� ��� �ð���ŭ ��ٸ���.
            }
            else if (distance <= traceDistance) // ���� ������ ���¶��
            {
                stateMachine.ChangeState(MonsterState.TRACE); // ���� ���·� �����Ѵ�.
                state = MonsterState.TRACE;
            }
            else // �� �ܿ���
            {
                stateMachine.ChangeState(MonsterState.IDLE); // ��� ���·� �����Ѵ�.
                state = MonsterState.IDLE;
            }

            if(isPlayer)
            {
                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
            }
            // �÷��̾� �����̸� �ƴ� ������ ��� ���ѷ���
            while (isPlayer)
            {
                yield return new WaitForSeconds(0.1f);
                if (gameObject.transform.parent == null)
                    isPlayer = false;
            }
        }

        // �� �ܿ��� (����, ����, ��� ���� ��� �Ұ����� ���) ��� ���·� �����Ѵ�.
        stateMachine.ChangeState(MonsterState.DEAD);
        state = MonsterState.DEAD;
    }

    // �⺻ ����
    private class BaseEnemyState : BaseState
    {
        protected BaseMonster owner;
        public BaseEnemyState(BaseMonster owner)
        {
            this.owner = owner;
        }
    }

    // ���(Idle) ����
    private class IdleState : BaseEnemyState
    {
        public IdleState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            // ������ �����.
            if (owner.agent.isActiveAndEnabled)
                owner.agent.isStopped = true;

            owner.rb.velocity = Vector3.zero;
            owner.rb.angularVelocity = Vector3.zero;

            // ���� �ִϸ��̼ǰ� ���� �ִϸ��̼��� �����Ѵ�.
            owner.animator.SetBool(owner.hashTrace, false);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    // ���� ����
    private class TraceState : BaseEnemyState
    {
        public TraceState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {

            // ĳ������ ��ġ�� �����Ͽ� ������ �����Ѵ�.
            owner.agent.SetDestination(owner.playerTrf.position);
            owner.agent.isStopped = false;

            // ���� �ִϸ��̼��� ����ϰ� ���� �ִϸ��̼��� �����Ѵ�.
            owner.animator.SetBool(owner.hashTrace, true);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    // ���� ����
    private class AttackState : BaseEnemyState
    {
        public AttackState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            // ������ �����.
            if (owner.agent.isActiveAndEnabled)
                owner.agent.isStopped = true;

            // ����(�ڽ�)�� �÷��̾��� �Ÿ��� ����Ѵ�.
            float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);

            // �� ��ų�� ���� ��� �ð��� ���� ������ ������ ���Ѵ�.
            if (distance >= owner.attackDistance && owner.skill1_curCooltime <= 0f)
            {
                owner.Skill1();
            }
            else if (owner.skill2_curCooltime <= 0f)
            {
                owner.Skill2();
            }
            else if (owner.attack_curCooltime <= 0f)
            {
                owner.Attack();
            }
        }
    }

    // ��� ����
    private class DeadState : BaseEnemyState
    {
        public DeadState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            if (owner.agent.isActiveAndEnabled)
                owner.agent.isStopped = true;

            owner.animator.SetTrigger(owner.hashDie);
            //owner.StartCoroutine(Die());
        }
        IEnumerator Die()
        {
            yield return new WaitForSeconds(2.0f);
            owner.gameObject.SetActive(false);
        }

    }

    #endregion State Classes

    #region Abstract Methods

    // ��ų�� �ʱ�ȭ�Ѵ�. (�� �ڽ� Ŭ�������� ��üȭ�Ѵ�.)
    protected abstract void InitSkills();

    // ��ų�� ���� ��� �ð��� ����Ѵ�.
    private void CalcCooltime()
    {
        if (attack_curCooltime > 0f)
        {
            attack_curCooltime -= Time.deltaTime;
        }

        if (skill1_curCooltime > 0f)
        {
            skill1_curCooltime -= Time.deltaTime;
        }

        if (skill2_curCooltime > 0f)
        {
            skill2_curCooltime -= Time.deltaTime;
        }
    }

    #endregion Abstract Methods

    #region Override Methods

    // ���� �Լ�
    public override void Attack()
    {
        StartCoroutine(NormalAttack());

        IEnumerator NormalAttack()
        {
            animator.SetBool(hashAttack, true);

            attack_curCooltime = attackCooltime;
            yield return new WaitForSeconds(0.2f);
            animator.SetBool(hashAttack, false);
        }
    }

    

    #endregion
}
