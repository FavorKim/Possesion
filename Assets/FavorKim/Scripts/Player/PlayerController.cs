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
    [SerializeField] Transform lookAtTransform;
    [SerializeField] HatManager hatM;

    [SerializeField] TextMeshProUGUI t_fullHP;
    [SerializeField] TextMeshProUGUI t_curHP;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider skill1Gauge;

    Skill skill1;
    Skill skill2;


    #endregion

    #region Vector
    public Vector3 MoveDir { get; private set; }
    #endregion

    #region float
    /*[SerializeField] */
    public float moveSpeed;
    /*[SerializeField] */
    public float gravityScale;
    /*[SerializeField] */
    public float jumpForce;
    [SerializeField] float rayDist;
    [SerializeField] float fullHP;
    [SerializeField] float curHP;


    #endregion

    [SerializeField] public bool isGround { get; private set; }


    #endregion


    #region Getter
    public CharacterController GetCC() { return CC; }
    public Animator GetAnimator() { return anim; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetGravityScale() { return gravityScale; }
    public float GetJumpForce() { return jumpForce; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        state = new PlayerStateMachine(this);
        skill1 = new Skill("test1", 5, () => { Debug.Log("skill1"); }, skill1Gauge);
    }



    void Update()
    {
        state.StateUpdate();
        Look();
        SetHPUI();
        skill1.SetCurCD();
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
    #endregion

    #region Event
    void OnMove(InputValue val)
    {
        Vector2 dir = val.Get<Vector2>();
        MoveDir = new Vector3(dir.x, 0, dir.y);
        if (MoveDir != Vector3.zero)
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);
    }

    void OnJump(InputValue val) { if (val.isPressed) state.StateOnJump(); }

    void OnAttack(InputValue val) { if (val.isPressed) state.StateOnAttack(); /*anim.SetTrigger("Attack");*/ }

    void OnThrowHat(InputValue val) { if (val.isPressed) state.StateOnHat(); /*anim.SetTrigger("Throw");*/ }

    void OnSkill1(InputValue val) 
    {
        if (val.isPressed) 
        {
            skill1.UseSkill();
        }
    }
    #endregion
}

public class Skill 
{
    public Skill(string skillName, float maxCD, Action effect, Slider gauge)
    {
        this.effect = effect; 
        this.skillName = skillName;
        this.maxCD = maxCD;
        this.gauge = gauge;

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

    void SetCurCDto0()
    {
        curCD = 0;
    }

    public void SetCurCD() { curCD += Time.deltaTime; gauge.value = curCD / maxCD; }
}
