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

    [SerializeField] public bool isGround { get;  private set; }


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
    }



    void Update()
    {
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

    void OnSkill(InputValue val) { if (val.isPressed) state.StateOnSkill(); /*anim.SetTrigger("Throw");*/ }
    #endregion
}
