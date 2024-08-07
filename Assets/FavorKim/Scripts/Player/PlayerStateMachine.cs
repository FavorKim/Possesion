using Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStateMachine
{
    public PlayerStateMachine(PlayerController controller)
    {
        player = controller;
        possessState = new PossessState(player);
        states.Add("Normal", new NormalState(player));
        states.Add("Possess", possessState);
        curState = states["Normal"];

        //OnJump += curState.Jump;
        //OnAttack += curState.Attack;
        //OnThrowHat += curState.Skill1;
    }
    private PlayerState curState; // 노말
    private PlayerController player;
    private Dictionary<string, PlayerState> states = new Dictionary<string, PlayerState>();


    protected PossessState possessState;


    public void StateUpdate()
    {
        // 노말무브
        curState.Move();

        curState.StateUpdate();
    }


    //public void StateFixedUpdate()
    //{

    //}

    public void StateOnJump() { curState.Jump(); }
    public void StateOnAttack() { curState.Attack(); }
    public void StateOnHat() { curState.Shift(); }
    public void StateOnSkill1() { curState.Skill1(); }
    public void StateOnSkill2() { curState.Skill2(); }

    public void ChangeState(string nextState)
    {
        curState.Exit();
        states[nextState].Enter();
        curState = states[nextState];
    }
    public void ChangeState(Monsters mon)
    {
        curState.Exit();
        curState = possessState;
        possessState.GetMonster(mon);

        //player.transform.position = mon.transform.position;
    }

    public bool IsPossessing() { return curState == possessState; }
}


public abstract class PlayerState : IState
{
    public PlayerState(PlayerController player)
    {
        this.player = player;
        stateCC = player.GetCC();
        moveSpeed = player.GetMoveSpeed();
        gravityScale = player.GetGravityScale();
        jumpForce = player.GetJumpForce();
        anim = player.GetComponent<Animator>();
        orgJumpForce = jumpForce;
        //isGround = player.isGround;
    }

    protected PlayerController player;
    protected CharacterController stateCC;
    protected Animator anim;
    protected Monsters mon;

    protected Vector3 moveDir => player.MoveDir;

    protected float moveSpeed;
    protected float gravityScale;
    protected float jumpForce;

    protected bool isGround => player.isGround;

    protected float orgJumpForce;
    protected bool isJumping = false;

    public virtual void Move()
    {
        //if (!Input.GetMouseButton(1) && moveDir != Vector3.zero)
        //    player.transform.rotation = Quaternion.FromToRotation(player.transform.position, moveDir * Time.deltaTime);

        //if (moveDir != Vector3.zero)
        //      stateCC.Move(player.transform.forward * moveSpeed * Time.deltaTime);
        stateCC.Move(moveDir);
    }

    public virtual void StateUpdate() 
    {
        stateCC.SimpleMove(-player.transform.up * gravityScale * Time.deltaTime);
        if (isJumping)
        {
            NormJump();
        }
    }

    public virtual void Jump() 
    {
        if (!isGround) return;
        anim.SetTrigger("Jump");
        isJumping = true;
    }
    public abstract void Attack();
    public abstract void Skill1();
    public abstract void Skill2();
    public abstract void Shift();

    public abstract void Enter();
    public abstract void Exit();

    void NormJump()
    {
        stateCC.Move(player.transform.up * jumpForce * Time.deltaTime);
        jumpForce -= Time.deltaTime * jumpForce;
        if (jumpForce < 10)
        {
            jumpForce = orgJumpForce;
            isJumping = false;
        }
    }
}

public class NormalState : PlayerState
{
    public NormalState(PlayerController controller) : base(controller) { orgJumpForce = jumpForce; }

    public override void Enter()
    {
        SkillManager.ResetSkill();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Jump()
    {
        base.Jump();
    }

    public override void Attack()
    {
        Debug.Log("atk");
    }

    public override void Skill1()
    {

    }

    public override void Skill2()
    {

    }

    public override void Shift()
    {
        anim.SetTrigger("Shift");
    }

    public override void Exit()
    {

    }

}



public class PossessState : PlayerState
{

    public PossessState(PlayerController controller) : base(controller)
    {
        durationGauge = player.GetDurationGauge();
    }
    Slider durationGauge;
    float playerOrgJumpForce;
    bool isFirst = true;
    
    public override void Enter()
    {
        if (isFirst && SceneManager.GetActiveScene().name == "FavorKim")
        {
            player.GetPossessUI().SetActive(true);
            isFirst = false;
        }

        playerOrgJumpForce = orgJumpForce;
        if(mon is Slime)
        {
            orgJumpForce += 10;
            jumpForce += 10;
        }

        
        GameObject hatImg = player.GetHatManager().GetHatImg();
        hatImg.SetActive(true);
        

        player.CameraTransform = mon.transform;
        GameManager.Instance.SetCameraFollow(player.CameraTransform);
        GameManager.Instance.SetCameraLookAt(player.GetPlayerFoward());

        //mon.SetSkill();
        if (mon.skill1 != null)
            SkillManager.SetSkill(mon.skill1, 1);
        else
            SkillManager.socket1.gameObject.SetActive(false);
        if (mon.skill2 != null)
            SkillManager.SetSkill(mon.skill2, 2);
        else
            SkillManager.socket2.gameObject.SetActive(false);

        durationGauge.gameObject.SetActive(true);
        durationGauge.value = 1;
    }


    public void GetMonster(Monsters _mon)
    {
        mon = _mon;
        //FaceUIController.Instance.GetPossessedMonster(mon);
        player.GetCC().Move(mon.transform.position - player.transform.position);


        _mon.transform.parent = player.transform;
        _mon.transform.localPosition = Vector3.zero;
        _mon.transform.localEulerAngles = Vector3.zero;
        if(_mon.GetComponent<Rigidbody>() != null)
        {
            _mon.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _mon.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //_mon.GetComponent<Rigidbody>().isKinematic = true;
        }

        Enter();
    }


    public override void Move()
    {
        base.Move();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (mon.GetHP() <= 0) 
            player.SetState("Normal");
        if (mon.skill1 != null)
            mon.skill1.SetCurCD();
        if (mon.skill2 != null)
            mon.skill2.SetCurCD();
        SetDuration();

        //player.transform.position = mon.transform.position;
        //mon.transform.localPosition = Vector3.zero;
    }

    public override void Jump()
    {
        base.Jump();
        Debug.Log("poJump");
    }
    public override void Attack()
    {
        mon.Attack();
    }
    public override void Skill1()
    {

        mon.skill1?.UseSkill();
    }
    public override void Skill2()
    {
        mon.skill2?.UseSkill();
    }

    public override void Shift()
    {
        player.SetState("Normal");
    }

    public override void Exit()
    {
        mon.transform.parent = null;
        player.GetCC().Move(mon.transform.position - player.transform.position);

        mon.GetDamage((int)(mon.GetHP() / 10.0f));

        player.CameraTransform = player.transform;

        FXManager.Instance.PlayFX("PoExit", player.transform.position);

        GameManager.Instance.SetCameraFollow(player.CameraTransform);
        GameManager.Instance.SetCameraLookAt(player.GetPlayerFoward());

        durationGauge.gameObject.SetActive(false);

        orgJumpForce = playerOrgJumpForce;
        jumpForce = playerOrgJumpForce;
    }

    void SetDuration()
    {
        durationGauge.value -= Time.deltaTime / player.GetDuration();
        if (durationGauge.value <= 0) Shift();
    }


}

public interface IState
{
    void Move();
    void Jump();
    void Enter();
    void Exit();
    void Attack();
    void Skill1();
    void Skill2();
    void Shift();
}



/*
몬스터의 자율 행동 패턴


캐릭터가 없음 - Patrol 

캐릭터를 찾음 - Trace 

공격범위 내 - Attack 

몬스터가 필드 안에서 공격을 할 때 하나 만 쓴다? 
플레이어 입장에서 몬스터로 변신했을 때 어떤 스킬을 쓸 수 있는지 모름 << 
플레이어가 몬스터랑 싸우면서


아 얘는 이런이런 스킬을 쓰는구나 그러면? 이럴때 얘로 변신하면 이렇게 싸울 수 있겠구나.

투사체를 발사하려면 프리팹이 있어야지. 
투사체 관리자를 만든다.
몬스터도 투사체 관리자한테서 프리팹을 얻고
플레이어도 투사체 관리자한테서 프리팹을 얻고

 bullet Manager
투사체 관리자는 여러 몬스터의 투사체를 갖고있고
그걸 받아온다.

몬스터 별로 달라지는 공격 조건 몬스터 마다 다를 수 있고
필요하다면 넣는게 맞는데
굳이 싶으면 안 넣어도.
 

 보스가 소환패턴이 있다 그러면
보스가 거는 기믹을 소환패턴에서 나오는 몬스터들로 수행이 가능해야.

 */