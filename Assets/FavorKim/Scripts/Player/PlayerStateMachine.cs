using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerStateMachine(PlayerController controller)
    {
        states.Add("Normal", new NormalState(controller));
        states.Add("Possess", new PossessState(controller));
        curState = states["Normal"];

        OnJump += curState.Jump;
        OnAttack += curState.Attack;
        OnSkill += curState.Skill;
    }
    private PlayerState curState;
    private Dictionary<string, PlayerState> states = new Dictionary<string, PlayerState>();


    public void StateUpdate()
    {
        curState.Move();
        curState.Gravity();
        
    }

    //public void StateFixedUpdate()
    //{
    //}

    public void StateOnJump() { OnJump(); }
    public void StateOnAttack() { OnAttack(); }
    public void StateOnSkill() { OnSkill(); }

    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnSkill;

    public void ChangeState(string nextState)
    {
        curState.Exit();
        states[nextState].Enter();
        curState = states[nextState];
    }
}


public abstract class PlayerState : IState
{
    public PlayerState(PlayerController player)
    {
        this.player = player;
        stateCC = player.GetCC();
        moveSpeed = player.moveSpeed;
        gravityScale = player.gravityScale;
        jumpForce = player.jumpForce;
        //anim = player.GetAnimator();
    }

    protected PlayerController player;
    protected CharacterController stateCC;
    //protected Animator anim;

    protected Vector3 moveDir => player.MoveDir;

    protected float moveSpeed;
    protected float gravityScale;
    protected float jumpForce;

    public abstract void Move();

    public abstract void Gravity();

    public abstract void Jump();
    public abstract void Attack();
    public abstract void Skill();

    public abstract void Enter();
    public abstract void Exit();
}

public class NormalState : PlayerState
{
    
    public NormalState(PlayerController controller) : base(controller) { orgJumpForce = jumpForce; }

    float orgJumpForce;
    bool isJumping = false;

    public override void Enter()
    {

    }
    public override void Move()
    {
        stateCC.Move(player.transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);

        if (isJumping)
            NormJump();
    }
    public override void Gravity()
    {
        stateCC.Move(-player.transform.up * gravityScale * 9.81f * Time.deltaTime);
    }
    public override void Jump()
    {
        isJumping = true;
    }
    public override void Attack()
    {

    }
    public override void Skill()
    {

    }
    public override void Exit()
    {

    }

    //IEnumerator CorJump()
    //{
    //    while (true)
    //    {
    //        stateCC.Move(player.transform.up * jumpForce * Time.deltaTime);
    //        jumpForce *= 0.99f;
    //        if (jumpForce < 0.01f) break;
    //        yield return null;
    //    }
    //    jumpForce = orgJumpForce;
    //    player.StopCoroutine(CorJump());
    //}

    void NormJump()
    {
        stateCC.Move(player.transform.up * jumpForce * Time.deltaTime);
        jumpForce *= 0.99f;
        if (jumpForce < 0.1f)
        {
            jumpForce = orgJumpForce;
            isJumping = false;
        }
    }

}

public class PossessState : PlayerState
{
    public PossessState(PlayerController controller) : base(controller) { }

    // Monster mon;

    public override void Enter()
    {
        /*
        애니메이션 호출

         */
    }

    /*
    public void GetMonster(Monster mon)
    {
        this.mon = mon;
        Enter();
    }
    */
    public override void Move()
    {
        // mon.Move();
        
    }

    public override void Gravity()
    {
        // mon.Gravity();
    }

    public override void Jump()
    {
        // mon.Jump();
    }
    public override void Attack()
    {
        // mon.Attack();
    }
    public override void Skill()
    {
        // mon.Skill();
    }

    public override void Exit()
    {
        // mon = null;
    }
}

public interface IState
{
    void Move();
    void Jump();
    void Enter();
    void Exit();
    void Attack();
    void Skill();

}
