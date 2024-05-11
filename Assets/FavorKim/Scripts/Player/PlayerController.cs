using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variable

    #region Mono

    private CharacterController CC;
    PlayerStateMachine state;
    Animator anim;
    [SerializeField] Transform lookAtTransform;
    [SerializeField] Hat hat;
    

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


    #endregion

    #endregion

    #region Getter
    public CharacterController GetCC() { return CC; }
    public Hat GetHat() { return hat; }
    public Animator GetAnimator() { return anim; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetGravityScale() { return gravityScale; }
    public float GetJumpForce() { return jumpForce; }
    #endregion

    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        state = new PlayerStateMachine(this);
    }
    //void Start()
    //{

    //}



    void Update()
    {
        state.StateUpdate();
        Look();
    }
    //private void FixedUpdate()
    //{
    //    state.StateFixedUpdate();
    //}

    void Look()
    {
        transform.LookAt(lookAtTransform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
        
    }

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

    void OnSkill(InputValue val) { if (val.isPressed) state.StateOnSkill(); anim.SetTrigger("Throw"); }

    public void ThrowHat()
    {
        hat.ThrowHat();
    }
}
