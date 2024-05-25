using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossDryad : MonoBehaviour, IDamagable
{
    #region Boss Status
    public enum BossState
    {
        IDLE,
        PATTERN,
        ATTACK,
        DEAD
    }

    public BossState state = BossState.IDLE;

    [SerializeField] private float curHP = 500f; // 몬스터의 현재 체력
    [SerializeField] private float maxHP = 500f; // 몬스터의 최대 체력
    [SerializeField] public float shootSpeed = 800.0f; // 발사 속도
    [SerializeField] public float spreadRange = 100.0f; // 뿌리는 범위

    [SerializeField] bool EnfPhased = false;
    private float invincibleTime = 1.0f;
    private bool isInvincible = false; // 무적 여부
    private bool isDie = false;
    private int pattern = -1;

    float[] pattern_Cooltime = { 3, 20, 15, 30 };
    float[] pattern_CurCooltime = { 3, 20, 15, 30 };

    [SerializeField] float traceDistance = 50f;
    /*
     패턴 표
     0 = 일반         4 = 전방 바람
     1 = 전방탄막     5 = 탄막비
     2 = 몹 소환      6 = 강화회전
     3 = 회전공격
    */

    #endregion

    #region Load Componant

    protected Transform enemyTrf; // 몬스터(자신)의 위치(Transform)
    protected Transform playerTrf; // 플레이어의 위치(Transform)
    protected NavMeshAgent agent; // 네비게이션(NavMesh)
    protected Animator animator; // 애니메이터(Animator)
    protected Rigidbody rb; // 리지드바디(Rigidbody)

    [SerializeField] PlayerController player;
    [SerializeField] public GameObject projectile;
    [SerializeField] public GameObject bullets;
    [SerializeField] public GameObject instantMonster;
    [SerializeField] public GameObject windStorm;
    [SerializeField] public GameObject vineObject;
    public Transform[] spawnPositions; // 탄막 발사 위치

    public StateMachine stateMachine;

    Slider bossHPSlider;
    #endregion

    #region Animator

    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashSkill = Animator.StringToHash("animation");
    readonly int hashPhaseChange = Animator.StringToHash("IsPhaseChange");
    readonly int hashGroggy = Animator.StringToHash("IsGroggy");
    readonly int hashDie = Animator.StringToHash("IsDie");

    #endregion

    #region Getter
    public float GetHP() { return curHP; }
    public float GetMaxHP() { return curHP; }

    #endregion

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerTrf = player.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyTrf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        stateMachine = gameObject.AddComponent<StateMachine>();
        bossHPSlider = gameObject.GetComponentInChildren<Slider>();
        bossHPSlider.gameObject.SetActive(false);

        stateMachine.AddState(BossState.IDLE, new IdleState(this));
        stateMachine.AddState(BossState.PATTERN, new PartternState(this));
        stateMachine.AddState(BossState.ATTACK, new AttackState(this));
        stateMachine.AddState(BossState.DEAD, new DeadState(this));
        stateMachine.InitState(BossState.IDLE);

        agent.destination = playerTrf.position;
    }

    void Start()
    {
        StartCoroutine(CoolTimeCheck());
        StartCoroutine(BossPattern());
    }

    IEnumerator CoolTimeCheck()
    {
        while (!isDie)
        {
            for (int i = 0; i < pattern_CurCooltime.Length; i++)
            {
                if (pattern_CurCooltime[i] > 0f)
                {
                    pattern_CurCooltime[i] -= Time.deltaTime;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator BossPattern()
    {
        float patternStart = 3.0f;
        bool boolStart = false;

        while(!boolStart)
        {
            float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
            if (distance >= traceDistance)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                bossHPSlider.gameObject.SetActive(true);
                stateMachine.ChangeState(BossState.IDLE);
                state = BossState.IDLE;
                boolStart = true;
            }
        }
        

        while (!isDie)
        {
            if (patternStart > 0f)
                patternStart -= Time.deltaTime;

            if (patternStart <= 0f)
            {
                pattern = -1;
                if (curHP <= maxHP / 2)
                    EnfPhased = true;
                
                if (pattern_CurCooltime[3] <= 0f)
                    pattern = 3;
                else if (pattern_CurCooltime[2] <= 0f)
                    pattern = 2;
                else if (pattern_CurCooltime[1] <= 0f)
                    pattern = 1;
                else if (pattern_CurCooltime[0] <= 0f)
                    pattern = 0;

                if (pattern > 0)
                {
                    patternStart = pattern_CurCooltime[0];
                    stateMachine.ChangeState(BossState.PATTERN);
                    state = BossState.PATTERN;
                }
                else if (pattern == 0)
                {
                    stateMachine.ChangeState(BossState.ATTACK);
                    state = BossState.ATTACK;
                }
            }
            else
            {
                if(state != BossState.IDLE)
                {
                    stateMachine.ChangeState(BossState.IDLE);
                    state = BossState.IDLE;
                }
            }

            if (curHP <= 0)
            {
                stateMachine.ChangeState(BossState.DEAD);
                state = BossState.DEAD;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Update()
    {
        //playerTrf = player.transform;
    }

    #region State Pattern
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
            owner.agent.isStopped = false;
            owner.agent.SetDestination(owner.playerTrf.position);
            
            //그로기
            /*if (owner.i == 6)
            {
                owner.animator.SetTrigger(owner.hashGroggy);
                owner.i = 0;
            }*/
        }
    }

    class AttackState : BaseEnemyState
    {
        public AttackState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.Attack();
        }
    }

    class PartternState : BaseEnemyState
    {
        public PartternState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;

            switch (owner.pattern)
            {
                case 1:
                    owner.Skill1();
                    break;
                case 2:
                    owner.Skill2();
                    break;
                case 3:
                    owner.Skill3();
                    break;
                case 4:
                    owner.Skill4();
                    break;
            }
            
        }
    }

    class DeadState : BaseEnemyState
    {
        public DeadState(BossDryad owner) : base(owner) { }

        public override void Enter()
        {
            owner.agent.isStopped = true;
            owner.animator.SetTrigger(owner.hashDie);
            owner.isDie = true;
        }
    }

    #endregion 

    #region Attack

    void Attack()
    {
        animator.SetBool(hashAttack, true);
    }
    void Attack_Event()
    {
        StartCoroutine(Attack_crt());
    }
    IEnumerator Attack_crt()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject pd1 = Instantiate(bullets, spawnPositions[0].position, Quaternion.identity);
            GameObject pd2 = Instantiate(bullets, spawnPositions[1].position, Quaternion.identity);
            pd1.transform.LookAt(playerTrf.localPosition);
            pd1.GetComponent<Rigidbody>().AddForce(pd1.transform.forward * shootSpeed);
            pd1.GetComponent<Rigidbody>().AddForce(pd1.transform.up * 10.0f);
            yield return new WaitForSeconds(0.2f);

            pd2.transform.LookAt(playerTrf.localPosition);
            pd2.GetComponent<Rigidbody>().AddForce(pd2.transform.forward * shootSpeed);
            pd2.GetComponent<Rigidbody>().AddForce(pd2.transform.up * 10.0f);
            yield return new WaitForSeconds(0.2f);
        }

        pattern_CurCooltime[0] = pattern_Cooltime[0];
        animator.SetBool(hashAttack, false);
        yield return new WaitForSeconds(1.0f);
    }
    #endregion

    #region skill1

    void Skill1()
    {
        animator.SetInteger(hashSkill, 1);
    }

    void Skill_1_Event()
    {
        
        StartCoroutine(Skill_1_crt());
    }

    IEnumerator Skill_1_crt()
    {
        Quaternion q = Quaternion.Euler(new Vector3(0, 20, 0));
        int percent = 0;
        if (EnfPhased)
        {
            percent = Random.Range(0, 100);
        }

        if (percent < 40)
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 5; i++)
                {
                    float range = 1.0f;
                    float posX = Random.Range(-range, range);
                    float posY = Random.Range(-range, range);

                    GameObject pd1 = Instantiate(projectile, spawnPositions[0].position + new Vector3(-2.5f + i, 0,-1f + k*2), Quaternion.LookRotation(gameObject.transform.forward) * q); ;

                    pd1.GetComponent<Rigidbody>().AddForce(posX * spreadRange * pd1.transform.up);
                    pd1.GetComponent<Rigidbody>().AddForce(posY * spreadRange * pd1.transform.right);
                    pd1.GetComponent<Rigidbody>().AddForce(shootSpeed * Random.Range(0.5f, 1.8f) * pd1.transform.forward);
                }
                yield return new WaitForSeconds(0.4f);
            }
            animator.SetInteger(hashSkill, 0);
        }

        else 
        {
            GameObject pd = Instantiate(windStorm, transform.position, Quaternion.LookRotation(-gameObject.transform.right) * q);

            animator.SetInteger(hashSkill, 0);
            yield return new WaitForSeconds(5.0f);
            GameManager.Instance.Player.isKnockBack = false;
            Destroy(pd);
        }
        pattern_CurCooltime[1] = pattern_Cooltime[1];
        yield return new WaitForSeconds(2.0f);
    }

    #endregion

    #region Skill2

    void Skill2()
    {
        animator.SetInteger(hashSkill, 2);
    }
    void SKill_2_Event()
    {
        StartCoroutine(Skill_2_crt());
    }

    IEnumerator Skill_2_crt()
    {
        int percent = 0;
        if (EnfPhased)
        {
            percent = Random.Range(0, 100);
        }

        if (percent < 70)
        {
            float posX = -1f;
            float posY = -1f;
            
            for (int i = 0; i < 2; i++)
            {
                GameObject pd = Instantiate(instantMonster, spawnPositions[0].position + new Vector3(posX, 0, posY + i*2), Quaternion.identity);

                pd.GetComponent<Rigidbody>().AddForce(posX * spreadRange * Vector3.forward);
                pd.GetComponent<Rigidbody>().AddForce((posY + i * 2) * spreadRange * Vector3.right);
                pd.GetComponent<Rigidbody>().AddForce(shootSpeed * Random.Range(0.8f, 1.2f) * Vector3.up);
            }
            
        }
        else
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 10; i++)
                {
                    float range = 1.0f;
                    float posX = Random.Range(-range, range);
                    float posY = Random.Range(-range, range);

                    GameObject pd = Instantiate(projectile, spawnPositions[0].position + new Vector3(posX * 2f, 0, posY * 2f), Quaternion.identity) as GameObject;

                    pd.GetComponent<Rigidbody>().AddForce(posX * spreadRange * Vector3.forward);
                    pd.GetComponent<Rigidbody>().AddForce(posY * spreadRange * Vector3.right);
                    pd.GetComponent<Rigidbody>().AddForce(shootSpeed * Random.Range(0.8f, 1.2f) * Vector3.up);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        pattern_CurCooltime[2] = pattern_Cooltime[2];
        animator.SetInteger(hashSkill, 0);
        yield return new WaitForSeconds(4.0f);
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
        Vector3[] vector3s = new Vector3[4];

        float height = 0.8f;
        vector3s[0] = new Vector3(2, height, 0);
        vector3s[1] = new Vector3(0, height, -2);
        vector3s[2] = new Vector3(-2, height, 0);
        vector3s[3] = new Vector3(0, height, 2);

        List<GameObject> pds = new List<GameObject>();

        if (EnfPhased)
        {
            for (int i = 0; i < 8; i++)
            {
                pds.Add(Instantiate(windStorm, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, i * 45, 0))));
            }
        }

        for (int k = 0; k < 15; k++)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject pd = Instantiate(bullets, gameObject.transform.position + vector3s[i], gameObject.transform.rotation * Quaternion.Euler(new Vector3(0, i * 90, 0)));
                pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * 400f);
            }
            yield return new WaitForSeconds(0.2f);
        }

        if (EnfPhased)
        {
            for (int i = 0; i < 8; i++)
            {
                Destroy(pds[i]);
            }
        }
        animator.SetInteger(hashSkill, 0);
        pattern_CurCooltime[3] = pattern_Cooltime[3];

        yield return new WaitForSeconds(4.0f);
        GameManager.Instance.Player.isKnockBack = false;
    }
    #endregion

    #region Skill4_PhaseChange
    public void Skill4()
    {
        animator.SetTrigger(hashPhaseChange);
        StartCoroutine(Skill_4_crt());
        
    }
    IEnumerator Skill_4_crt()
    {
        yield return new WaitForSeconds(1.0f);
        vineObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        isInvincible = false;
        curHP -= 1;
    }

    #endregion
    public void GetDamage(int damage)
    {
        // 무적 상태가 아닐 경우,
        if (!isInvincible)
        {
            if (curHP - damage < maxHP / 2 && !EnfPhased)
            {
                pattern = 4;
                stateMachine.ChangeState(BossState.PATTERN);
                state = BossState.PATTERN;
            }

            // 대미지만큼 체력을 감소시킨다.
            if (curHP > damage)
                curHP -= damage;
            else
                curHP = 0;

            StartCoroutine(CorInvincible());
        }
    }

    IEnumerator CorInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
