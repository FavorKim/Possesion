using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.UI.GridLayoutGroup;

public class MonsterPlant : MonoBehaviour
{
    // 플레이어 정보를 받아야 NevMesh를 따라 추적이 가능함.
    [SerializeField] protected Player player;

    //State랑 스킬이랑 별개로 분류하고, 

    public enum MonsterState
    { 
        IDLE,
        TRACE,
        ATTACK,
        SKILL1,
        SKILL2,
        DEAD
    }
    
    public float traceDistance = 10f;
    public float skillDistance = 10f;
    public float attackDistance = 2f;

    public MonsterState state = MonsterState.IDLE;
    public bool isDie = false;

    public float skill1_totalCooltime = 3f;
    public float skill1_curCooltime = 0f;

    Transform enemyTrf;
    Transform playerTrf;
    NavMeshAgent agent;
    Animator animator;

    public StateMachine stateMachine;

    private Dictionary<MonsterState, IState> dicState = new Dictionary<MonsterState, IState>();

    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");

    // Start is called before the first frame update
    void Awake()
    {
        player = Player.Instance;
        playerTrf = player.transform;
        enemyTrf = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(MonsterState.IDLE, new IdleState(this));
        stateMachine.AddState(MonsterState.TRACE, new TraceState(this));
        stateMachine.AddState(MonsterState.SKILL1, new Skill1State(this));
        
        stateMachine.AddState(MonsterState.ATTACK, new AttackState(this));
        stateMachine.InitState(MonsterState.IDLE);

        agent.destination = playerTrf.position;
    }
    protected virtual void Start()
    {
        StartCoroutine(CheckEnemyState());
    }

    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

           if (state == MonsterState.DEAD)
            {
                stateMachine.ChangeState(MonsterState.DEAD);
                yield break;
            }

            float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

            if (distance <= skillDistance && skill1_curCooltime <= 0f)
            {
                stateMachine.ChangeState(MonsterState.SKILL1);
                state = MonsterState.SKILL1;
            }
            else if (distance <= attackDistance)
            {
                stateMachine.ChangeState(MonsterState.ATTACK);
                state = MonsterState.ATTACK;
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

    class BaseEnemyState : BaseState
    {
        protected MonsterPlant owner;
        public BaseEnemyState(MonsterPlant owner)
        {
            this.owner = owner;
        }
    }

    private void Update()
    {
        if(skill1_curCooltime > 0f)
        {
            skill1_curCooltime -= Time.deltaTime;
        }
        
        /*enemyTrf
        animator.SetFloat("FloatX", gameObject.transform.position.z);
        animator.SetFloat("FloatY", gameObject.transform.position.x);*/
    }

    class IdleState : BaseEnemyState
    {
        public IdleState(MonsterPlant owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashTrace, false);
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

    class Skill1State : BaseEnemyState
    {
        public Skill1State(MonsterPlant owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashAttack, true);

            owner.skill1_curCooltime = owner.skill1_totalCooltime;
        }

    }
    class AttackState : BaseEnemyState
    {
        public AttackState(MonsterPlant owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetBool(owner.hashAttack, true);
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
}

