using ObjectPool;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

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

    //public ParticleSystem rollAttack;
    public StateMachine stateMachine;

    [SerializeField] public GameObject projectile;
    public Transform[] spawnPositions;
    [SerializeField] public float shootSpeed = 800.0f;

    public ParticleSystem spinAttack;

    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill = Animator.StringToHash("animation");

    Rigidbody rb;
    #region 스킬 등등
    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;

    public float traceDistance = 1000f;

    public bool isDie = false;

    int i = 0;
    #endregion

    public override void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyTrf = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(BossState.IDLE, new IdleState(this));
        stateMachine.AddState(BossState.PATTERN, new TraceState(this));
        stateMachine.AddState(BossState.ATTACK, new AttackState(this));
        stateMachine.AddState(BossState.DEAD, new TraceState(this));
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
            yield return new WaitForSeconds(0.1f);

            if (Input.GetKeyDown("1"))
            {
                i = 1;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("2"))
            {
                i = 2;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("3"))
            {
                i = 3;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else if (Input.GetKeyDown("4"))
            {
                i = 4;
                stateMachine.ChangeState(BossState.ATTACK);
            }
            else
            {
                agent.SetDestination(playerTrf.position);
                agent.isStopped = false;
            }

            /*if (state == MonsterState.DEAD)
            {
                stateMachine.ChangeState(MonsterState.DEAD);
                yield break;
            }*/

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
            
            owner.animator.SetBool(owner.hashAttack, false);
        }
    }

    class TraceState : BaseEnemyState
    {
        public TraceState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.animator.SetBool(owner.hashAttack, false);
            
        }
    }

    class AttackState : BaseEnemyState
    {
        public AttackState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {

            if(owner.i == 1)
            {
                owner.Skill1();
            }
            else if (owner.i == 2)
            {
                owner.Skill2();
            }
            else if (owner.i == 3)
            {
                owner.Skill3();
            }
            else
            {
                owner.Attack();
            }
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
    #region Attack
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
    }
    void Attack_Event()
    {
        StartCoroutine(Attack_crt());
    }
    IEnumerator Attack_crt()
    {
        float distance;

        distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

        GameObject pd = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        //GameObject pd2 = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        pd.transform.LookAt(playerTrf.localPosition);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);

        yield return new WaitForSeconds(1.0f);
        animator.SetInteger(hashSkill, 0);
    }
    #endregion

    #region Skill2

    public override void Skill2()
    {
        animator.SetInteger(hashSkill, 2);
    }
    void SKill_2_Event()
    {
        StartCoroutine(Skill_2_crt());
    }

    IEnumerator Skill_2_crt()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject pd = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
            float posX = Random.Range(-1, 1);
            float posY = Random.Range(-1, 1);
            pd.transform.Translate(posX, 0, posY);

            pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * posX * 100.0f);
            pd.GetComponent<Rigidbody>().AddForce(pd.transform.right * posY * 100.0f);
            pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * 1000.5f);
        }
        
        yield return new WaitForSeconds(3.0f);
        animator.SetInteger(hashSkill, 0);

    }
    #endregion

    #region Skill3

    public void Skill3()
    {
        animator.SetInteger(hashSkill, 3);
    }
    void SKill_3_Event()
    {
        StartCoroutine(Skill_3_crt());
    }

    IEnumerator Skill_3_crt()
    {
        ParticleSystem ps = Instantiate(spinAttack, this.transform); //, Quaternion.identity


        /*
        float time = 0;
        while (time > 3f)
        {
            time += Time.deltaTime;
            player.transform.Translate(Vector3.back);
        
        }*/
        yield return new WaitForSeconds(3.0f);
        Destroy(ps);
        animator.SetInteger(hashSkill, 0);
        
    }
    #endregion


    #region skill1

    public override void Skill1()
    {
        animator.SetInteger(hashSkill, 1);
    }

    void Skill_1_Event()
    {
        StartCoroutine(Skill_1_crt());
    }

    IEnumerator Skill_1_crt()
    {
        float distance;

        distance = Vector3.Distance(playerTrf.position, enemyTrf.position);

        GameObject pd = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        //GameObject pd2 = Instantiate(projectile, spawnPositions[0].position, Quaternion.identity) as GameObject;
        pd.transform.LookAt(playerTrf.localPosition);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 15.5f);

        yield return new WaitForSeconds(1.0f);
        animator.SetInteger(hashSkill, 0);
    }

    #endregion

}
