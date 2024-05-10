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
    }

    public void StateFixedUpdate()
    {
        curState.Gravity();
    }
    
    public void StateOnJump() { OnJump(); }
    public void StateOnAttack() {  OnAttack(); }
    public void StateOnSkill() {  OnSkill(); }

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
        moveDir = player.GetMoveDir();
        moveSpeed = player.GetMoveSpeed();
        gravityScale = player.GetGravityScale();
        jumpForce = player.GetJumpForce();
    }
    protected PlayerController player;
    protected CharacterController stateCC;

    protected Vector2 moveDir;
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
    public NormalState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        
    }
    public override void Move()
    {
        stateCC.Move(moveDir * moveSpeed);
    }
    public override void Gravity()
    {
        stateCC.Move(-player.transform.up * gravityScale);
    }
    public override void Jump()
    {
        stateCC.Move(player.transform.up * jumpForce);
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
}

public class PossessState : PlayerState
{
    public PossessState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        // 몬스터 정보 얻어오기
    }

    public override void Move()
    {
        // 얻어온 몬스터의 업데이트 함수 호출하기
    }

    public override void Gravity()
    {
        
    }

    public override void Jump()
    {

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
