using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    public virtual void Move() 
    {
        stateCC.Move(player.transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    public abstract void StateUpdate();

    public abstract void Jump();
    public abstract void Attack();
    public abstract void Skill1();
    public abstract void Skill2();
    public abstract void Shift();

    public abstract void Enter();
    public abstract void Exit();
}

public class NormalState : PlayerState
{
    public NormalState(PlayerController controller) : base(controller) { orgJumpForce = jumpForce; }
    private float speed;

    float orgJumpForce;
    bool isJumping = false;
    public override void Enter()
    {
        SkillManager.ResetSkill();
    }

    public override void Move()
    {
        base.Move();
        if (isJumping)
        {
            NormJump();
        }
    }

    public override void StateUpdate()
    {
        stateCC.SimpleMove(-player.transform.up * gravityScale * Time.deltaTime);
    }

    public override void Jump()
    {
        if (!isGround) return;
        anim.SetTrigger("Jump");
        isJumping = true;
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



public class PossessState : PlayerState
{
    public PossessState(PlayerController controller) : base(controller) 
    {
        durationGauge = player.GetDurationGauge(); 
    }
    Slider durationGauge;

    // MonsterController monCon;
    // MonsterController 다른 몬스터들이 상속을 받아
    // virtual(abstract) Move()
    // 좀비컨트롤러 : 몬스터컨트롤러
    // override move(){};


    // 행동 패턴 메서드를 대리자로 선언해서 


    // 몬스터는 Input으로 움직이지 않는다.
    // 플레이어는 Input으로 움직인다.


    /*
    이동에 필요한게 벡터.

    - 이동
    플레이어 : 벡터를 플레이어는 인풋으로 설정을 하고 이동

    몬스터 : 플레이어와 본인의 위치를 기준으로 계산을 해서 설정.
    
    Ai처리 후 / 행동(계산된 값)
    행동 메서드가 계산된 값을 매개로 받아(이동이라면, 벡터)

    Move(vector3 dir);

    계산 + 이동;
    
    MonsterAIMove() <- 빙의 되기 전
    {
        MonsterMove(calcul());
    }

    몬스터는 빙의 되고 나서 할 게 없음 <- 없어짐

    몬스터는 빙의가 된지 안된지 어케 암?
    -> 플레이어가 모자를 몬스터에 맞추면 몬스터 오브젝트를 비활성화
      -> 이러면 몬스터는 어케 이동 함수를 줌?

    인풋은 어쨌든 플레이어 내부에서 벡터로 바뀌긴 함.

    vector3 calcul(); -> 여기서 방향을 산출

    void MonsterMove(Vector3 dir) -> 매개로 전달받은 벡터값을 향해서 이동
    
    
    몬스터 -> AI -> 행동


    void Attack(); <<< 

    플레이어는 검사를 안 해도 되지?
    검사 대신이 Input
    플레이어는 공격키를 누르면 애니메이션 켜준다.
    
    저 Attack에 필요한 변수들. 얘는 어떻게 넘기느냐?
    animator << 

    몬스터가 자기거니까 자기껄 주는
    플레이어가 몬스터한테서 받으려면 몬스터의 변수가 퍼블릭이어야함.
    
    몬스터 변수는 프라이빗이고, 넘겨주는 함수만 퍼블릭이면
    
    플레이어는 불가능한데 몬스터는 가능한 거 (AI에 따른 이동)
     

    플레이어가 몬스터에 대한 정보를 다 갖고있다면, << 몬스터 매니저(몬스터에 대한 정보를 다 가지고있는 녀석)
    플레이어와 몬스터의 행동 패턴이 같다.
    플레이어가 조작하는 오브젝트의 대상만 바꿔서 행동을 한다.

    
                  PlayerInputManager
    플레이어는 플레이어의 행동을 인풋을기반으로 행동을 하고
    몬스터는 몬스터의 행동을 인풋을 기반으로 행동시키면 됨.
    
    박쥐는 침 뱉음
    오우거는 몽둥이를 휘두른다.


    플레이어가 박쥐로 빙의하면 플레이어는 공격버튼을 누르면 침을 뱉는다.
    오우거면 몽둥이를 휘두른다.

    플레이어가 됐을 때 호출할 공격 ex)가래침뱉기 식으로 강화해서 플레이어에게 넘겨줄 수도.

    몬스터는 사실 플레이어가 극복하지 못하는 플랫폼을 언제든 극복할 수 있어
    다만 얘가 그러려고 하지 않을 뿐.
    그래서? 의지가 있는 플레이어가
    그렇게 하는 것.


    맨 처음 생각으로 돌아가자면

    정해야할 것은.
    플레이어가 몬스터로 변했을 때
    무엇을 사용할 것인가.

    플레이어 : 공격, 스킬1, 스킬2, 점프, 이동, 모자 던지기(빙의 스킬 버튼<- 빙의 됐을 때 제한시간이 다 되지않더라도 빙의를 끝내기)
    몬스터 : 얘네를 다 할 것인지. 일부만 할 것인지 (이동은 그냥 플레이어 이동대로 해)

    플레이어가 빙의했을 때. 플레이어가 사용할 몬스터의 기능은?

    공격, 스킬1, 스킬2

    나머지 행동 패턴은 플레이어가 알아서

    커비
    자동차가 되면 제동, 부스터?
    계단 돌리기, 눕기

    몬스터 틀. 공격, 스킬1, 스킬2 (추상화)
    몬스터 1이 추상화된 위에 것들을 (구체화)
    
    근데 매개가 몬스터 틀
    플레이어는 몬스터에 참조를 걸어서 그 기능들을 쓰는거죠
    
    






    
    같은몬스턴데 다른 스크립트
    필드 몬스터는 필드 몬스터 스크립트
    빙의 몬스터는 빙의 몬스터 변수

    



     */

    /*
    1. 몬스터에게서 빙의에 필요한 정보만 가져온다.
    플레이어가 몬스터가 된다.
    
    ㅇ -> X
    모자 X

    움직일 때 모자 몬스터가 움직임.
    플레이어가 외형을 바꾼다. <- 이것도 한 번 생각해 봄
    플레이어가 외형을 여러가지고 있다.

    몬스터를 컨트롤 <- 나도 이걸 처음 생각했어 이걸 하고싶었으나? 답이 안 나옴
    근데? 막막함.

    플레이어가 몬스터가 된다. 
     */



    /*
    위에 거 너무 머리 아픔 나중에 개인으로 해야지...
    일부만 수정하는거? <- 일단 난 못 함...
    모자만 붙이는거? <- 나도 함
    위에 거 하려면 시간 더 있어야지.

    위처럼 모자를 씌웠을 때

    플레이어는 몬스터의 어떤 것을 흡수할까요?

    ex) 공격, 이동

    공격하는 방식도 몬스터마다 다른데 <- 흠
    
    몬스터 매니저 <- 이걸 쓰는 것도 좋을 것 같은데
    몬스터 매니저가 모든 몬스터의 정보를 가지고 있어서
    플레이어 -> 몬스터 매니저
    플레이어 : 나 ㅇㅇ몬스터 맞췄어요
    몬스터 매니저 : 그래 그러면 ㅇㅇ몬스터의 (스킬, 이동, 공격 ...) 줄게 이거 갖고 잘 써먹으렴

    플레이어 내부 (빙의 상호작용을 책임지는 녀석)
     - 플레이어 (프리팹) 온
     - 몬스터 1 (프리팹 몬스터1의 행동을 담은 스크립) 오프<스크립트 x 외형 정보 + 애니메이터
    

    몬스터 1은 필드에 돌아다니는 몬스터 1에 모자만 씌운 프리팹 <- 얘는 AI대로 움직임.

    그러면 몬스터 1에 플레이어가 이동하는 것 처럼 이동하는 스크립트와 <- 이건 이미 플레이어에 구현되어있는데?
    AI로 이동하는 스크립트를 둘 다 따로 만들어줘야하나?

     - 몬스터 2 (프리팹 몬스터2의 행동을 담은 스크립) 오프
     - 

    몬스터 1은 Input으로 안 움직임.

    빙의 했을 때 



    

    */

    public override void Enter()
    {
        /*
        애니메이션 호출

         */


        mon.SetSkill();
        durationGauge.gameObject.SetActive(true);
        durationGauge.value = 1;
    }


    public void GetMonster(Monsters _mon)
    {
        mon = _mon;
        
        player.GetCC().Move(mon.transform.position - player.transform.position);

        _mon.transform.parent = player.transform;
        _mon.transform.localPosition = Vector3.zero;
        _mon.transform.localEulerAngles = Vector3.zero;
        
        Enter();
    }


    public override void Move()
    {
        //mon.Move();
        base.Move();
    }

    public override void StateUpdate()
    {
        mon.skill1.SetCurCD();
        mon.skill2.SetCurCD();
        SetDuration();
    }

    public override void Jump()
    {
        // mon.Jump();
    }
    public override void Attack()
    {
        mon.Attack();
    }
    public override void Skill1()
    {

        mon.skill1.UseSkill();
    }
    public override void Skill2()
    {
        mon.skill2.UseSkill();
    }

    public override void Shift()
    {
        player.SetState("Normal");
    }

    public override void Exit()
    {
        mon.transform.parent = null;
        

        durationGauge.gameObject.SetActive(false);
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