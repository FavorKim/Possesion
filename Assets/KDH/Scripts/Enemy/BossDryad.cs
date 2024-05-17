using ObjectPool;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossDryad : Monsters
{
    public enum BossState
    {
        IDLE,
        PATTERN,
        ATTACK,
        DEAD
    }


    

    // 플레이어 정보를 받아야 NevMesh를 따라 추적이 가능함.
    [SerializeField] PlayerController player;

    public BossState state = BossState.IDLE;

    float skill1_curCooltime = 0f;
    float skill2_curCooltime = 0f;

    Transform enemyTrf;
    Transform playerTrf;
    NavMeshAgent agent;
    Animator animator;

    public ParticleSystem rollAttack;
    public StateMachine stateMachine;

    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill1 = Animator.StringToHash("IsSkill1");
    readonly int hashDefend = Animator.StringToHash("IsDefend");

    Rigidbody rb;
    #region 스킬 등등
    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;

    public float traceDistance = 10f;
    public float skillDistance = 10f;
    public float attackDistance = 2f;

    public bool isDie = false;

    #endregion

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        enemyTrf = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        /*stateMachine.AddState(MonsterState.IDLE, new IdleState(this));
        stateMachine.AddState(MonsterState.TRACE, new TraceState(this));
        stateMachine.AddState(MonsterState.ATTACK, new AttackState(this));*/
        stateMachine.InitState(BossState.IDLE);

        agent.destination = playerTrf.position;
    }
    protected virtual void Start()
    {
        StartCoroutine(BossPattern());
    }

    protected virtual IEnumerator BossPattern()
    {
        /* 
        1페이즈
        기본 공격 - 탄막 발사 

        몹 소환 
        덩굴 소환으로 진행 막기
        

        2페이즈 밀쳐내기
        기본 공격 - 강화 탄막 발사 
        
        폭탄씨앗 발사 투사체 주위를 터뜨림
        
        발사 중심으로 쭉 밀어내기.
        +1페이즈까지

        2페이즈 시 맵 바깥은 사라지지 않는 가시 추가.
       
        */
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            /*if (state == MonsterState.DEAD)
            {
                stateMachine.ChangeState(MonsterState.DEAD);
                yield break;
            }*/

            bool isPatterned = false;
            // 특수 공격가능 상태
            /*if (isPatterned)
            {
                if(state != MonsterState.ATTACK)
                stateMachine.ChangeState(MonsterState.DEAD);
            }*/
        }
        /*stateMachine.ChangeState(MonsterState.DEAD);
        state = MonsterState.DEAD;*/
    }

    private void Update()
    {
        /*if (skill1_curCooltime > 0f)
        {
            skill1_curCooltime -= Time.deltaTime;
        }
        if (skill2_curCooltime > 0f)
        {
            skill2_curCooltime -= Time.deltaTime;
        }*/
    }

    class BaseEnemyState : BaseState
    {
        protected BossDryad owner;
        public BaseEnemyState(BossDryad owner)
        {
            this.owner = owner;
        }
    }

    class IdleState : BaseEnemyState
    {
        public IdleState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashTrace, false);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class TraceState : BaseEnemyState
    {
        public TraceState(BossDryad owner) : base(owner) { }

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
        public AttackState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.Attack();

            /*float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);

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
            }*/
        }
    }

    class DeadState : BaseEnemyState
    {
        public DeadState(BossDryad owner) : base(owner) { }

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
        //기왕이면 다 돌아가고 나서 스킬 발동?
        /*float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        animator.SetBool(hashSkill1, true);*/
    }

    /*void RAttack()
    {
        StartCoroutine(RollingAttack());
    }
    IEnumerator RollingAttack()
    {
        agent.isStopped = true;
        ParticleSystem ps = Instantiate(rollAttack, this.transform); //, Quaternion.identity
        rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.8f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //skill1_curCooltime = mstSkill1Cooltime;
        Destroy(ps);
        animator.SetBool(hashSkill1, false);
    }*/

    public void Skill2()
    {
        //skill2_curCooltime = mstSkill2Cooltime;
        /*StartCoroutine(Defend());

        IEnumerator Defend()
        {
            animator.SetBool(hashDefend, true);
            yield return new WaitForSeconds(1.5f);
            animator.SetBool(hashDefend, false);
        }*/
    }
}
