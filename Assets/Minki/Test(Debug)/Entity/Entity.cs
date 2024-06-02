using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 플레이어(Player)와 몬스터(Monster)의 공통 속성을 묶기 위한 최상위 클래스
public abstract class Entity : MonoBehaviour
{
    #region Fields

    #region HealthPoint

    protected float currentHealthPoint; // 현재 체력
    [SerializeField] protected float maxHealthPoint; // 최대 체력

    #endregion HealthPoint

    #region Speed

    [SerializeField] protected float moveSpeed; // 이동 속도
    [SerializeField] protected float rotateSpeed; // 회전(몸을 돌리는) 속도
    [SerializeField] protected float jumpPower; // 점프 강도

    public void GetSpeeds(out float moveSP, out float rotateSP, out float jumpSP)
    {
        moveSP = moveSpeed;
        rotateSP = rotateSpeed;
        jumpSP = jumpPower;
    }

    public void SetSpeeds(in float moveSP, in float rotateSP, in float jumpSP)
    {
        moveSpeed = moveSP;
        rotateSpeed = rotateSP;
        jumpPower = jumpSP;
    }

    #endregion Speed

    #region Skill

    protected Skill skill00; // 기본 공격
    protected Skill skill01; // 스킬 1
    protected Skill skill02; // 스킬 2

    public void GetSkills(out Skill sk00, out Skill sk01, out Skill sk02)
    {
        sk00 = skill00;
        sk01 = skill01;
        sk02 = skill02;
    }

    #endregion Skill

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

    // 몬스터 클래스; 빙의 상태의 몬스터에 접근하기 위한 변수
    private Entity possessingMonster;

    // 빙의를 유지할 수 있는 시간
    [SerializeField] private Slider durationGauge;
    public Slider DurationGauge { get { return durationGauge; } }

    // 스킬의 재사용 대기시간을 UI로 보여준다.
    [SerializeField] private Slider skill01CoolTimeGauge;
    [SerializeField] private Slider skill02CoolTimeGauge;

    #endregion Fields

    #region Life Cycle Methods

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerStateMachine = new TestStateMachine(this);

        // 첫 시작 시 스탯을 초기화한다.
        InitializeStatus();
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

    #region Initialize Status

    // 각종 스탯을 초기화한다.
    private void InitializeStatus()
    {
        // 처음에는 최대 체력으로 시작한다.
        currentHealthPoint = maxHealthPoint;
    }

    #endregion Initialize Status

    #region State Machine

    private TestStateMachine playerStateMachine; // 플레이어의 상태 기계
    [SerializeField] private GameObject playerOutfit; // 플레이어의 캐릭터 모델
    public GameObject PlayerOutfit { get { return playerOutfit; } }

    public void SetState(TestPlayer player)
    {
        playerStateMachine.ChangeState(player);
    }

    public void SetState(TestMonster monster)
    {
        playerStateMachine.ChangeState(monster);
    }

    #endregion State Machine

    #region Input System Methods

    // Input System으로 입력을 받아, 이동 / 공격 등의 행동을 취한다.

    #region Move (Arrows / WASD)

    // 플레이어의 이동을 구현한다. 여기에서는 이동의 방향만 저장하고, 실제 이동은 아래의 점프 유무까지 포함하여 Update문에서 실행한다.

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
        Vector3 vector = Vector3.Normalize(inputVector.x * cameraTransform.right + inputVector.y * cameraTransform.forward) * moveSpeed;

        // 카메라의 시야는 위, 아래를 향할 수 있으나, 캐릭터는 그 방향으로는 움직이지 않아야 한다.
        vector.y = 0f;

        // 이동하고 있는지를 판별한다.
        bool isMove = (vector != Vector3.zero);

        // 이동하는 것이라면,
        if (isMove)
        {
            // 플레이어를 그 방향으로 회전시킨다. (이 코드는 조건을 걸지 않을 경우, 입력 값이 없을 때 항상 원점으로 회전하기 때문에 조건문이 필요하다.)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), rotateSpeed * Time.deltaTime);
        }

        // 적절한 애니메이션을 재생한다.
        animator.SetBool("isRun", isMove); // bool 매개변수를 사용할 경우
        // animator.SetFloat("MoveSpeed", moveVector.normalized.magnitude); // float 매개변수를 사용할 경우; Blend Tree를 활용할 수 있다.

        // y 값을 제외한 나머지 값을 이동 벡터에 저장해 둔다.
        moveVector = new Vector3(vector.x, moveVector.y, vector.z);
    }

    #endregion Move (Arrows / WASD)

    #region Jump (Space)

    // 플레이어의 점프를 구현한다.

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
            moveVector.y = Mathf.Sqrt(jumpPower * -2.0f * gravityScale);

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

    // 플레이어가 모자를 던진다. 던진 모자에 몬스터가 피격할 경우, 플레이어는 그 몬스터에 빙의한다.

    // 모자에 대한 기능은 HatManager 클래스에서 정의한다.
    private HatManager hatManager;

    private void OnThrowHat(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ThrowHat();
        }
    }

    private void ThrowHat()
    {
        hatManager.ShootHat(transform.forward);
    }

    #endregion Throw Hat (Left Shift)

    #region Attack (Left Ctrl)

    // 플레이어의 일반 공격(Skill00으로 정의)을 구현한다. 플레이어 자체는 공격이나 스킬을 가지고 있지 않고, 몬스터에 빙의하여 그 기술을 사용한다.

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            skill00?.UseSkill();
        }
    }

    #endregion Attack (Left Ctrl)

    #region Skills (Q / E)

    // 플레이어의 스킬(Skill01, 02, …으로 정의)을 구현한다.

    private void OnSkill01(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            skill01?.UseSkill();
        }
    }

    private void OnSkill02(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            skill02?.UseSkill();
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

    #endregion Knockback, 필요 없지 않아?

    #region isGrounded

    /* 
        캐릭터 컨트롤러(Character Controller) 컴포넌트의 isGrounded 프로퍼티가 매우 부정확한 이유로,
        레이캐스트(Raycast)를 사용하여 땅을 닿아 있는지를 확인하는 함수를 구현한다.
    */

    // 땅에 닿아 있는지를 판별하는 변수
    private bool isGrounded = false;

    // 땅에 닿아 있는지를 판별하는 함수
    private void CheckIsGrounded()
    {
        // 캐릭터의 아래로 짧게 레이(Ray)를 쏘아, 충돌할 경우 판별 변수를 참으로 한다.
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

    #region Get Hit (OnTriggerEnter)

    // 몬스터, 장애물 등에 피격했을 때의 함수를 구현한다.

    // 무적 상태를 표현하는 파티클 시스템
    [SerializeField] private ParticleSystem invincibleFX;

    private bool isInvincible = false; // 무적 상태 여부
    private float invincibleTime = 2.0f; // 무적 지속 시간

    // 트리거의 충돌로 피격을 구현한다.
    private void OnTriggerStay(Collider other)
    {
        // 충돌한 오브젝트가 대미지를 가하는 것일 경우, 무적 상태가 아니라면
        if (other.GetComponent<Obstacles>() && !isInvincible)
        {
            // 공격력만큼 대미지를 입는다.
            currentHealthPoint -= other.GetComponent<Obstacles>().Damage;

            // 피격 애니메이션을 재생한다.
            animator.SetTrigger("Hit");

            // 무적 상태에 돌입한다.
            StartCoroutine(CorInvincible());
        }
    }

    private IEnumerator CorInvincible()
    {
        // 무적 상태에 돌입하여, 무적 파티클 시스템을 활성화한다.
        isInvincible = true;
        invincibleFX.gameObject.SetActive(true);
        invincibleFX.Play();

        // 무적 지속 시간만큼 기다린다.
        yield return new WaitForSeconds(invincibleTime);

        // 무적 상태를 해제하여, 무적 파티클 시스템을 비활성화한다.
        isInvincible = false;
        invincibleFX.gameObject.SetActive(false);
    }

    #endregion Get Hit (OnTriggerEnter)

    #endregion Custom Methods
}

// 몬스터 클래스
public class TestMonster : Entity
{
    [SerializeField] private float attackRange;// 공격 범위
    [SerializeField] private float detectRange; // 플레이어 탐지 범위
}
