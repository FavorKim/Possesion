using ObjectPool;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Monsters
{
    public enum MonsterState
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }
    // 플레이어 정보를 받아야 NevMesh를 따라 추적이 가능함.
    [SerializeField] PlayerController player;
    [SerializeField] public GameObject projectile;
    [SerializeField] public float shootSpeed = 800.0f;

    public Transform spawnPosition;
    public MonsterState state = MonsterState.IDLE;

    float skill1_curCooltime = 0f;
    float skill2_curCooltime = 0f;

    Transform enemyTrf;
    Transform playerTrf;
    NavMeshAgent agent;
    Animator animator;

    public ParticleSystem stabAttack;
    public StateMachine stateMachine;

    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill1 = Animator.StringToHash("IsSkill1");
    readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

    Rigidbody rb;
    #region 스킬 등등
    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;
    public float mstSkill1Cooltime = 3.0f;
    public float mstSkill2Cooltime = 3.0f;

    public float traceDistance = 10f;
    public float skillDistance = 10f;
    public float attackDistance = 2f;

    public bool isDie = false;
    public bool isPlayer = false;
    #endregion

    void Awake()
    {
        isPlayer = gameObject.transform.parent != null;
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        enemyTrf = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(MonsterState.IDLE, new IdleState(this));
        stateMachine.AddState(MonsterState.TRACE, new TraceState(this));
        stateMachine.AddState(MonsterState.ATTACK, new AttackState(this));
        stateMachine.InitState(MonsterState.IDLE);

        agent.destination = playerTrf.position;
    }
    protected virtual void Start()
    {
        if (!isPlayer)
            StartCoroutine(CheckEnemyState());
    }

    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDie && !isPlayer)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == MonsterState.DEAD)
            {
                stateMachine.ChangeState(MonsterState.DEAD);
                yield break;
            }

            float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

            // 특수 공격가능 상태
            bool is_pjtAtk = skillDistance < distance && skill1_curCooltime <= 0f;
            if (distance <= attackDistance || is_pjtAtk)
            {
                stateMachine.ChangeState(MonsterState.ATTACK);
                state = MonsterState.ATTACK;
                yield return new WaitForSeconds(0.5f);
            }
            else if (distance <= traceDistance)
            {
                stateMachine.ChangeState(MonsterState.TRACE);
                state = MonsterState.TRACE;
            }
            else
            {
                stateMachine.ChangeState(MonsterState.IDLE);
                state = MonsterState.IDLE;
            }
        }
        stateMachine.ChangeState(MonsterState.DEAD);
        state = MonsterState.DEAD;
    }

    private void Update()
    {
        if (skill1_curCooltime > 0f)
        {
            skill1_curCooltime -= Time.deltaTime;
        }
        if (skill2_curCooltime > 0f)
        {
            skill2_curCooltime -= Time.deltaTime;
        }
    }

    class BaseEnemyState : BaseState
    {
        protected Skeleton owner;
        public BaseEnemyState(Skeleton owner)
        {
            this.owner = owner;
        }
    }

    class IdleState : BaseEnemyState
    {
        public IdleState(Skeleton owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashTrace, false);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class TraceState : BaseEnemyState
    {
        public TraceState(Skeleton owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.SetDestination(owner.playerTrf.position);

            owner.agent.isStopped = false;
            owner.animator.SetBool(owner.hashTrace, true);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class AttackState : BaseEnemyState
    {
        public AttackState(Skeleton owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);

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

    class DeadState : BaseEnemyState
    {
        public DeadState(Skeleton owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Dead");
        }

    }
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
    }
    public override void Skill1()
    {
        float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        skill1_curCooltime = mstSkill1Cooltime;
        
        StartCoroutine(SlashAttack());

        IEnumerator SlashAttack()
        {
            agent.isStopped = true;
            animator.SetBool(hashSkill1, true);
            GameObject pd = Instantiate(projectile, spawnPosition.position, Quaternion.identity) as GameObject;
            pd.transform.LookAt(playerTrf.localPosition);
            pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
           
            yield return new WaitForSeconds(1.5f);
            animator.SetBool(hashSkill1, false);
        }
    }

    void SAttack()
    {
        StartCoroutine(StabAttack());
    }
    public void Skill2()
    {
        animator.SetBool(hashSkill2, true);
    }

    IEnumerator StabAttack()
    {
        agent.isStopped = true;

        ParticleSystem ps = Instantiate(stabAttack, this.transform);
        ps.transform.LookAt(playerTrf.localPosition);
        rb.AddRelativeForce(Vector3.forward * 10f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.4f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        skill2_curCooltime = mstSkill2Cooltime;
        Destroy(ps);
        animator.SetBool(hashSkill2, false);

    }
}
