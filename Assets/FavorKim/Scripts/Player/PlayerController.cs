using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variable

    #region Mono

    private CharacterController CC;
    PlayerStateMachine state;
    Animator anim;
    Animator monAnim;

    [SerializeField] Transform lookAtTransform;
    [SerializeField] HatManager hatM;

    [SerializeField] TextMeshProUGUI t_fullHP;
    [SerializeField] TextMeshProUGUI t_curHP;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider skill1Gauge;
    [SerializeField] Slider skill2Gauge;

    [SerializeField] Slider durationGauge;

    [SerializeField] GameObject plantOF;
    [SerializeField] GameObject goblinOF;
    [SerializeField] GameObject playerOF;

    SkillManager sM;

    #endregion

    #region Vector
    public Vector3 MoveDir { get; private set; }
    #endregion

    #region float
    [SerializeField]
    private float moveSpeed;
    //[SerializeField]
    //private float maxSpeed;
    //[SerializeField]
    //private float minSpeed;


    [SerializeField]
    private float gravityScale;
    [SerializeField]
    private float jumpForce;
    [SerializeField] float rayDist;
    [SerializeField] float fullHP;
    [SerializeField] float curHP;

    [SerializeField] float duration;

    #endregion


    [SerializeField] public bool isGround { get; private set; }
    bool isDead = false;

    Dictionary<string, GameObject> outFits = new Dictionary<string, GameObject>();

    #endregion


    #region Getter
    public CharacterController GetCC() { return CC; }
    public Animator GetAnimator() { return anim; }
    public Slider GetDurationGauge() { return durationGauge; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetGravityScale() { return gravityScale; }
    public float GetJumpForce() { return jumpForce; }
    public float GetDuration() { return duration; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
        state = new PlayerStateMachine(this);
        sM = new SkillManager(skill1Gauge, skill2Gauge);

        //skill1 = new Skill("test1", 5, () => { Debug.Log("skill1"); }, skill1Gauge);

        outFits.Add("Goblin", goblinOF);
        outFits.Add("Plant", plantOF);
        outFits.Add("Player", playerOF);
        OnDead += DeadCheck;


        t_fullHP.text = fullHP.ToString();
    }



    void Update()
    {
        if (isDead) return;
        state.StateUpdate();
        Look();
        SetHPUI();

    }

    private void FixedUpdate()
    {
        CheckLand();
    }
    #endregion

    #region Method


    void Look()
    {
        transform.LookAt(lookAtTransform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void SetOutFit(string name)
    {
        outFits["Goblin"].SetActive(false);
        outFits["Plant"].SetActive(false);
        outFits["Player"].SetActive(false);

        outFits[name].SetActive(true);

        monAnim = outFits[name].GetComponent<Animator>();
        //Debug.Log(monAnim.ToString());
    }


    void SetHPUI()
    {
        t_curHP.text = curHP.ToString();
        hpBar.value = curHP / fullHP;
    }

    void CheckLand()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDist, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, rayDist))
            isGround = true;
        else
            isGround = false;
    }

    public void ThrowHat()
    {
        hatM.ShootHat();
    }

    public void SetState(string name)
    {
        state.ChangeState(name);
        playerOF.SetActive(true);
    }

    public void SetState(Monsters mon)
    {
        state.ChangeState(mon);
        playerOF.SetActive(false);
    }

    public void GetDamage(int dmg)
    {
        curHP -= dmg;
        anim.SetTrigger("Hit");
        OnDead();
    }

    void DeadCheck()
    {
        if (curHP <= 0)
        {
            curHP = 0;
            isDead = true;
            anim.SetBool("isDead", true);
            anim.SetTrigger("Dead");
        }
    }

    #endregion

    #region Event

    void OnMove(InputValue val)
    {
        
        Vector2 dir = val.Get<Vector2>();
        MoveDir = new Vector3(dir.x, 0, dir.y);

        if (MoveDir != Vector3.zero)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }


        anim.SetFloat("vecX",  dir.x);
        anim.SetFloat("vecY",  dir.y);

    }

    void OnJump(InputValue val) { if (val.isPressed) state.StateOnJump(); }

    void OnAttack(InputValue val) { if (val.isPressed) state.StateOnAttack(); }

    void OnThrowHat(InputValue val)
    {
        if (val.isPressed)
        {
            state.StateOnHat();
        }
    }

    void OnSkill1(InputValue val)
    {
        if (val.isPressed)
        {
            state.StateOnSkill1();
        }
    }

    void OnSkill2(InputValue val)
    {
        if (val.isPressed)
        { state.StateOnSkill2(); }
    }

    event Action OnDead;

    #endregion
}

public class Skill
{
    public Skill(string skillName, float maxCD, Action effect)
    {
        this.effect = effect;
        this.skillName = skillName;
        this.maxCD = maxCD;
    }

    string skillName;

    public Slider gauge;

    float maxCD;
    float curCD;

    public bool CanUse() { return curCD > maxCD; }

    Action effect;

    public void UseSkill()
    {
        if (!CanUse()) return;
        effect();
        SetCurCDto0();
    }

    public void SetCurCDto0()
    {
        curCD = 0;
    }

    public void SetCurCD() { curCD += Time.deltaTime; gauge.value = curCD / maxCD; }
}

public class SkillManager
{
    public SkillManager(Slider socket1, Slider socket2)
    {
        SkillManager.socket1 = socket1;
        SkillManager.socket2 = socket2;
    }
    public static Slider socket1;
    public static Slider socket2;

    public static void SetSkill(Skill skill, int socket)
    {
        socket1.gameObject.SetActive(true);
        socket2.gameObject.SetActive(true);

        if (socket == 1)
            skill.gauge = socket1;
        else if (socket == 2)
            skill.gauge = socket2;

        skill.SetCurCDto0();
    }
    public static void ResetSkill()
    {
        socket1.gameObject.SetActive(false);
        socket2.gameObject.SetActive(false);
    }
}
