using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 플레이어(Player)와 몬스터(Monster)의 공통 속성을 묶기 위한 최상위 클래스
public abstract class Entity : MonoBehaviour
{
    #region Fields

    public float CurrentHealthPoint { get; set; } // 현재 체력
    public float MaxHealthPoint { get; set; } // 최대 체력

    public float MoveSpeed { get; set; } // 이동 속도
    public float RotateSpeed { get; set; } // 회전(몸을 돌리는) 속도
    public float JumpPower { get; set; } // 점프 강도

    public int AttackDamage { get; set; } // 기본 공격력
    public float AttackCoolTime { get; set; } // 기본 공격 재사용 대기 시간

    protected Skill Skill01 { get; set; } // 스킬 1
    public int Skiil01Damage { get; set; } // 스킬1 공격력
    public float Skill01CoolTime { get; set; } // 스킬1 재사용 대기 시간

    public Skill Skill02 { get; set; } // 스킬 2
    public int Skill02Damage { get; set; } // 스킬2 공격력
    public float Skill02CoolTime { get; set; } // 스킬2 재사용 대기 시간

    public float AttackRange { get; set; } // 공격 범위
    public float DetectRange { get; set; } // 플레이어 탐지 범위

    #endregion Fields
}

// 플레이어 클래스
public class TestPlayer : Entity
{
    #region Components

    private CharacterController characterController; // 캐릭터 컨트롤러(Character Controller)
    private Animator animator; // 애니메이터(Animator)

    #endregion Components

    #region Fields

    private Entity possessingMonster; // 몬스터 클래스; 빙의 상태의 몬스터에 접근하기 위한 변수
    private TestMonster b;

    [SerializeField] private Slider durationGauge;
    public Slider DurationGauge { get { return durationGauge; } }

    #endregion Fields

    #region Life Cycle Methods

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 상태를 갱신한다.
        playerStateMachine.UpdateState();

        // 플레이어를 이동시킨다.
        characterController.Move(moveVector * Time.deltaTime);
    }

    #endregion Life Cycle Methods

    #region Custom Methods

    #region State Machine

    private TestStateMachine playerStateMachine; // 플레이어의 상태 기계
    [SerializeField] private GameObject playerOutfit; // 플레이어의 캐릭터 모델
    public GameObject PlayerOutfit { get { return playerOutfit; } }

    public void SetState(string name)
    {
        playerStateMachine.ChangeState(name);
        
    }

    public void SetState(TestMonster monster)
    {
        playerStateMachine.ChangeState(monster);
    }

    #endregion State Machine

    #region Input System Methods

    #region Move (Arrows / WASD)

    // 플레이어를 비추는 카메라; 시네머신(Cinemachine)이 부착된 메인 카메라를 지정한다.
    protected Transform cameraTransform;

    // 방향 키의 입력 값
    protected Vector2 inputVector;
    // 실제 이동 벡터; 아래의 점프 함수에도 적용받는다.
    protected Vector3 moveVector;

    // 인풋 시스템(Input System)으로 값을 입력 받아, 저장하는 함수
    private void OnMove(InputValue inputValue)
    {
        // 인풋 시스템의 함수는 입력이 들어오거나 나가는 순간에만 호출되므로, 입력 값을 받는 작업만 수행한다.

        // 입력 받은 값을 Vector2로 변환하여 저장한다.
        inputVector = inputValue.Get<Vector2>();
    }

    // 캐릭터의 이동을 구현하는 함수
    public virtual void Move()
    {
        // 저장한 입력 값을 카메라의 시야를 기준으로 하여 Vector3로 변환한다. 또한, 값을 정규화하여 대각선으로의 이동을 정상화한다.
        Vector3 vector = Vector3.Normalize(inputVector.x * cameraTransform.right + inputVector.y * cameraTransform.forward) * MoveSpeed;

        // 카메라의 시야는 위, 아래를 향할 수 있으나, 캐릭터는 그 방향으로는 움직이지 않아야 한다.
        vector.y = 0f;

        // 이동하고 있는지를 판별한다.
        bool isMove = (vector != Vector3.zero);

        // 이동하는 것이라면,
        if (isMove)
        {
            // 플레이어를 그 방향으로 회전시킨다. (이 코드는 조건을 걸지 않을 경우, 입력 값이 없을 때 항상 원점으로 회전하기 때문에 조건문이 필요하다.)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), RotateSpeed * Time.deltaTime);
        }

        // 적절한 애니메이션을 재생한다.
        animator.SetBool("isRun", isMove); // bool 매개변수를 사용할 경우
        // animator.SetFloat("MoveSpeed", moveVector.normalized.magnitude); // float 매개변수를 사용할 경우; Blend Tree를 활용할 수 있다.

        // y 값을 제외한 나머지 값을 이동 벡터에 저장해 둔다.
        moveVector = new Vector3(vector.x, moveVector.y, vector.z);
    }

    #endregion Move (Arrows / WASD)

    #region Jump (Space)

    // 점프의 유무
    private bool isJumping = false;

    // 중력 가속도
    private float gravityScale = Physics.gravity.y;

    // 인풋 시스템(Input System)으로 값을 입력 받아, 점프를 수행하는 함수
    private void OnJump(InputValue inputValue)
    {
        // 플레이어가 땅에 닿아 있을 때만,
        if (isGrounded)
        {
            // 점프한다.
            isJumping = true;
        }
    }

    // 플레이어의 점프를 구현하는 함수
    public virtual void Jump()
    {
        // 땅에 닿아 있는지를 확인한다.
        CheckIsGrounded();

        // 점프 키를 눌렀다면
        if (isJumping)
        {
            // 위로 점프한다.
            moveVector.y = Mathf.Sqrt(JumpPower * -2.0f * gravityScale);

            // 연속으로 점프할 수 없다.
            isJumping = false;
        }

        // 캐릭터가 공중에 있을 경우,
        if (!isGrounded)
        {
            // 아래로 점점 떨어진다.
            moveVector.y += gravityScale * Time.deltaTime;
        }
    }

    #endregion Jump (Space)

    #region Throw Hat (Left Shift)

    private void OnThrowHat(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            // playerStateMachine.StateOnHat();
        }
    }

    #endregion Throw Hat (Left Shift)

    #region Attack (Left Ctrl)

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            // UseAttack();
        }
    }

    #endregion Attack (Left Ctrl)

    #region Skills (Q / E)

    private void OnSkill01(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            // UseSkill01();
        }
    }

    private void OnSkill02(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            // UseSkill02();
        }
    }

    #endregion Skills (Q / E)

    #endregion Input System Methods


    #region Knockback, 필요 없지 않아?

    //private bool isKnockback = false;

    //private void SetKnockBack(Transform user, float power)
    //{
    //    tempKnockBackdirect = user;
    //    tempKnockBack = power;
    //    isKnockBack = true;
    //}

    //void GetKnockback(Transform knockBackUser, float power)
    //{
    //    CC.Move((transform.position - knockBackUser.position).normalized * Time.deltaTime * power);
    //}

    //public void GetKnockback(Vector3 dir, float duration, float length)
    //{
    //    transform.DOMove((dir + transform.position).normalized * length, duration);
    //}

    #endregion ThrowHat

    #region isGrounded

    /* 
        캐릭터 컨트롤러(Character Controller) 컴포넌트의 isGrounded 프로퍼티가 매우 부정확한 이유로,
        레이캐스트(Raycast)를 사용하여 땅을 닿아 있는지를 확인하는 함수를 구현한다.
    */

    private bool isGrounded = false;

    private void CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.3f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    #endregion isGrounded

    #region Throw Hat

    private HatManager hatManager;

    private void ThrowHat()
    {
        hatManager.ShootHat(transform.forward);
    }

    #endregion Throw Hat

    #region Get Hit

    private float invincibleTime = 2.0f; // 무적 지속 시간
    public bool IsInvincible { get; private set; } = false; // 무적 상태 여부
    public bool IsDead { get; private set; } = false; // 사망 상태 여부

    [SerializeField] private ParticleSystem invinFX; // 무적 상태를 표현하는 파티클 시스템

    private event Action OnDamaged;

    private void GetDamage(int dmg)
    {
        if (IsInvincible || IsDead || dmg == 0) return;

        if (playerStateMachine.IsPossessing())
        {
            return;
        }

        CurrentHealthPoint -= dmg;
        animator.SetTrigger("Hit");
        StartCoroutine(CorInvincible());
        OnDamaged();
    }

    private IEnumerator CorInvincible()
    {
        IsInvincible = true;
        invinFX.gameObject.SetActive(true);
        invinFX.Play();

        yield return new WaitForSeconds(invincibleTime);

        invinFX.gameObject.SetActive(false);
        IsInvincible = false;
    }

    #endregion Get Hit

    #endregion Custom Methods

    #region Abstract Methods

    // 빙의 상태의 몬스터에 접근하여, 그 공격이나 스킬을 사용한다.


    #endregion Abstract Methods
}

// 몬스터 클래스
public class TestMonster : Entity
{

}