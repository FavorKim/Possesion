using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class MonsterPlant : Monsters
{
    public enum MonsterState
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }

    [SerializeField] PlayerController player;

    [SerializeField] public GameObject projectile;
    public Transform spawnPosition;
    public MonsterState state = MonsterState.IDLE;

    [SerializeField] public float shootSpeed = 800.0f;

    float skill1_curCooltime = 0f;
    float skill2_curCooltime = 0f;

    Transform enemyTrf;
    Transform playerTrf;
    NavMeshAgent agent;
    Animator animator;

    public StateMachine stateMachine;

    //private Dictionary<MonsterState, IState> dicState = new Dictionary<MonsterState, IState>();

    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

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

    // Start is called before the first frame update
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
        if(!isPlayer)
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

            //원거리 공격가능 상태
            bool is_pjtAtk = skillDistance < distance && skill1_curCooltime <= 0f;
            if (distance <= attackDistance || is_pjtAtk)
            {
                stateMachine.ChangeState(MonsterState.ATTACK);
                state = MonsterState.ATTACK;
                yield return new WaitForSeconds(1f);
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
        if(isDie)
        {
            stateMachine.ChangeState(MonsterState.DEAD);
            state = MonsterState.DEAD;
        }
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
        /*enemyTrf
        animator.SetFloat("FloatX", gameObject.transform.position.z);
        animator.SetFloat("FloatY", gameObject.transform.position.x);*/
    }

    class BaseEnemyState : BaseState
    {
        protected MonsterPlant owner;
        public BaseEnemyState(MonsterPlant owner)
        {
            this.owner = owner;
        }
    }

    class IdleState : BaseEnemyState
    {
        public IdleState(MonsterPlant owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashTrace, false);
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class TraceState : BaseEnemyState
    {
        public TraceState(MonsterPlant owner) : base(owner) { }

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
        public AttackState(MonsterPlant owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            float distance = Vector3.Distance(owner.playerTrf.position, owner.enemyTrf.position);
            
            //여기까지 왔다면 원/근 파트는 나눠 뒀음.
            if (distance >= owner.attackDistance && owner.skill1_curCooltime <= 0f)
            {
                owner.Skill1();
            }
            else if(owner.skill2_curCooltime <= 0f)
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
        public DeadState(MonsterPlant owner) : base(owner) { }

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
        float distance;
        if (!isPlayer)
        {
            distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        }
        else
        {
            distance = 3.0f;
        }

        GameObject pd = Instantiate(projectile, spawnPosition.position, Quaternion.identity) as GameObject;
        pd.transform.LookAt(playerTrf.localPosition);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);

        //여기서 스킬을 발사 해줘야 하거든?
        skill1_curCooltime = mstSkill1Cooltime;

        animator.SetBool(hashAttack, true);

    }
    public void Skill2()
    {
        animator.SetTrigger(hashSkill2);
        skill2_curCooltime = mstSkill2Cooltime;
    }
}


