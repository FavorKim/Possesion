using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : Monsters
{
    #region Components

    protected Transform enemyTrf; // 몬스터(자신)의 위치(Transform)
    protected Transform playerTrf; // 플레이어의 위치(Transform)
    protected NavMeshAgent agent; // 네비게이션(NavMesh)
    protected Animator animator; // 애니메이터(Animator)
    protected Rigidbody rb; // 리지드바디(Rigidbody)

    #endregion Components

    #region Fields

    // 몬스터의 상태를 나타내는 enum (상태 패턴)
    protected enum MonsterState
    {
        IDLE, TRACE, ATTACK, DEAD
    }

    protected MonsterState state = MonsterState.IDLE; // 처음에는 IDLE 상태이다.

    // 유한 상태 기계 클래스
    private StateMachine stateMachine;

    // 몬스터의 사망 / 빙의 여부
    protected bool isDie = false;
    protected bool isPlayer = false;

    // 애니메이터의 해시(Hash), 각 하위 클래스에서 알맞게 추가 정의할 것.
    protected readonly int hashTrace = Animator.StringToHash("IsTrace");
    protected readonly int hashAttack = Animator.StringToHash("IsAttack");

    // 플레이어 정보를 받아야 NevMesh를 따라 추적이 가능함.
    [SerializeField] private PlayerController player;

    #endregion Fields

    #region Skills / Stats

    protected float mstATK;
    protected float mstSPD;

    // 몬스터의 스킬 재사용 대기 시간
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
        // 컴포넌트를 초기화한다.
        player = FindObjectOfType<PlayerController>();
        playerTrf = player?.transform;
        enemyTrf = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // 상태 기계를 초기화한다.
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(MonsterState.IDLE, new IdleState(this));
        stateMachine.AddState(MonsterState.TRACE, new TraceState(this));
        stateMachine.AddState(MonsterState.ATTACK, new AttackState(this));
        stateMachine.InitState(MonsterState.IDLE);

        // NavMesh를 초기화한다.
        agent.destination = playerTrf.position;

        // 몬스터의 스킬을 초기화한다.
        InitSkills();
    }

    private void Start()
    {
        // 몬스터의 상태 검사를 시작한다.
        StartCoroutine(CheckEnemyState());
    }

    private void Update()
    {
        // 스킬의 재사용 대기 시간을 계산한다.
        CalcCooltime();

        animator.SetFloat("FloatX", rb.velocity.x * 100f);
        animator.SetFloat("FloatY", rb.velocity.z * 100f);
    }

    #endregion Life Cycles (Awake / Start / Update)

    #region State Classes

    // 상태를 확인하는 코루틴 함수
    private IEnumerator CheckEnemyState()
    {
        // 죽지 않았거나, 빙의되지 않았다면 무한 반복한다.
        while (!isDie && !isPlayer)
        {
            // isDie, isPlayer 체크
            isDie = (state == MonsterState.DEAD);
            isPlayer = (gameObject.transform.parent != null);

            // 약 0.3초의 대기 시간
            yield return new WaitForSeconds(0.3f);

            // 몬스터의 상태가 DEAD라면,
            if (state == MonsterState.DEAD)
            {
                // 상태 기계의 상태를 DEAD로 바꾼다.
                stateMachine.ChangeState(MonsterState.DEAD);

                // 즉시 탈출한다.
                yield break;
            }

            // 몬스터(자신)과 플레이어의 거리를 계산한다.
            float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

            // 특수 공격가능 상태
            bool is_pjtAtk = skillDistance > distance && skill1_curCooltime <= 0f;

            if (distance <= attackDistance || is_pjtAtk) // 공격 가능한 상태라면 (일정 거리 이내)
            {
                stateMachine.ChangeState(MonsterState.ATTACK); // 공격 상태로 변경한다.
                state = MonsterState.ATTACK;

                yield return new WaitForSeconds(attackCooltime); // 재사용 대기 시간만큼 기다린다.
            }
            else if (distance <= traceDistance) // 추적 가능한 상태라면
            {
                stateMachine.ChangeState(MonsterState.TRACE); // 추적 상태로 변경한다.
                state = MonsterState.TRACE;
            }
            else // 그 외에는
            {
                stateMachine.ChangeState(MonsterState.IDLE); // 대기 상태로 변경한다.
                state = MonsterState.IDLE;
            }

            Debug.Log($"distance = {distance}, is_pjtAtk = {is_pjtAtk.ToString()}, state = {state.ToString()}");
        }

        // 그 외에는 (공격, 추적, 대기 상태 모두 불가능한 경우) 사망 상태로 변경한다.
        stateMachine.ChangeState(MonsterState.DEAD);
        state = MonsterState.DEAD;
    }

    // 기본 상태
    private class BaseEnemyState : BaseState
    {
        protected BaseMonster owner;
        public BaseEnemyState(BaseMonster owner)
        {
            this.owner = owner;
        }
    }

    // 대기(Idle) 상태
    private class IdleState : BaseEnemyState
    {
        public IdleState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            // 추적을 멈춘다.
            owner.agent.isStopped = true;

            // 추적 애니메이션과 공격 애니메이션을 중지한다.
            owner.animator.SetBool(owner.hashTrace, false);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    // 추적 상태
    private class TraceState : BaseEnemyState
    {
        public TraceState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            // 캐릭터의 위치를 지정하여 추적을 시작한다.
            owner.agent.SetDestination(owner.playerTrf.position);
            owner.agent.isStopped = false;

            // 추적 애니메이션을 재생하고 공격 애니메이션을 중지한다.
            owner.animator.SetBool(owner.hashTrace, true);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    // 공격 상태
    private class AttackState : BaseEnemyState
    {
        public AttackState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            // 추적을 멈춘다.
            owner.agent.isStopped = true;

            // 몬스터(자신)와 플레이어의 거리를 계산한다.
            float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);

            // 각 스킬의 재사용 대기 시간에 따라 적절한 공격을 취한다.
            if (distance >= owner.attackDistance && owner.skill1_curCooltime <= 0f)
            {
                owner.Skill1();
            }
            else if (owner.skill2_curCooltime <= 0f)
            {
                owner.Skill2();
            }
            else
            {
                owner.Attack();
            }
        }
    }

    // 사망 상태
    private class DeadState : BaseEnemyState
    {
        public DeadState(BaseMonster owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Dead");
        }

    }

    #endregion State Classes

    #region Abstract Methods

    // 스킬을 초기화한다. (각 자식 클래스에서 구체화한다.)
    protected abstract void InitSkills();

    // 스킬의 재사용 대기 시간을 계산한다.
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
}
