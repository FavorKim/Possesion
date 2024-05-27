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
    public float JumpPower { get; set; } // 점프 강도

    public int AttackDamage { get; set; } // 기본 공격력
    public float AttackCoolTime { get; set; } // 기본 공격 재사용 대기 시간

    public Skill Skill01 { get; set; } // 스킬 1
    public int Skiil01Damage { get; set; } // 스킬1 공격력
    public float Skill01CoolTime { get; set; } // 스킬1 재사용 대기 시간

    public Skill Skill02 { get; set; } // 스킬 2
    public int Skill02Damage { get; set; } // 스킬2 공격력
    public float Skill02CoolTime { get; set; } // 스킬2 재사용 대기 시간

    public float AttackRange { get; set; } // 공격 범위
    public float DetectRange { get; set; } // 플레이어 탐지 범위

    #endregion Fields

    #region Methods

    public abstract void UseAttack();

    public abstract void UseSkill01();
    public abstract void UseSkill02();

    public void GetDamage()
    {
        // 피격 관련 함수를 정의한다.
    }



    #endregion Methods
}

// 플레이어 클래스
public class TestPlayer : Entity
{
    #region Components

    private CharacterController characterController; // 캐릭터 컨트롤러(Character Controller)
    private Animator animator; // 애니메이터(Animator)

    #endregion Components

    #region Fields

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
    [SerializeField] private Transform cameraTransform;

    // 이동의 속도
    [SerializeField] private float moveSpeed;
    // 회전의 속도
    [SerializeField] private float rotateSpeed;

    // 상태 클래스에서 필요한 변수를 참조하기 위한 함수
    public void GetMoveFieldRefs(Transform playerTransform, Transform cameraTransform, ref float moveSpeed, ref float rotateSpeed)
    {
        playerTransform = transform;
        cameraTransform = this.cameraTransform;
        moveSpeed = this.moveSpeed;
        rotateSpeed = this.rotateSpeed;
    }

    #endregion Move (Arrows / WASD)

    #region Jump (Space)

    // 점프의 강도
    [SerializeField] private float jumpPower;
    // 중력 가속도; 기본 값은 -9.81f
    [SerializeField] private float gravityScale = Physics.gravity.y;

    public void GetJumpFieldRefs(ref float jumpPower, ref float gravityScale)
    {
        jumpPower = this.jumpPower;
        gravityScale = this.gravityScale;
    }

    #endregion Jump (Space)

    #region Throw Hat (Left Shift)

    private void OnThrowHat(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            playerStateMachine.StateOnHat();
        }
    }

    #endregion Throw Hat (Left Shift)

    #region Attack (Left Ctrl)

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            playerStateMachine.StateOnAttack();
        }
    }

    #endregion Attack (Left Ctrl)

    #region Skills (Q / E)

    private void OnSkill01(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            playerStateMachine.StateOnSkill01();
        }
    }

    private void OnSkill02(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            playerStateMachine.StateOnSkill02();
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

    #region Is Grounded

    public bool IsGrounded { get; private set; } = false;

    private void CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.3f))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }

    #endregion Is Grounded

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
}

// 몬스터 클래스
public class TestMonster : Entity
{

}

