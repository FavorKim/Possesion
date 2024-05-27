using UnityEngine.InputSystem;
using UnityEngine;

interface IPlayerState
{
    // State 클래스에 있어야 적절
    void Enter(); // 상태에 들어갈 때의 함수
    void Execute(); // 상태를 유지하는 동안 수행할 함수
    void Exit(); // 상태를 나갈 때의 함수

    // 아래의 함수들은 Entity 클래스에 있어야 적절
    void Move(); // 이동할 때의 함수
    void Jump(); // 점프할 때의 함수
    void Shift(); // 모자를 던지거나(빙의 전), 빙의를 그만둘 때(빙의 후)의 함수
    void Attack(); // 일반 공격할 때의 함수
    void Skill01(); // 스킬 01을 사용할 때의 함수
    void Skill02(); // 스킬 02를 사용할 때의 함수
}

public abstract class TestPlayerState : IPlayerState
{
    // 컴포넌트 (Component)
    protected TestPlayer playerController;
    protected CharacterController characterController;
    protected Animator animator;

    protected GameObject playerOutfit;

    // 생성자 (Constructor)
    public TestPlayerState(TestPlayer player)
    {
        playerController = player;
        characterController = player.GetComponent<CharacterController>();
        animator = player.GetComponent<Animator>();
    }

    #region Interface Methods

    public abstract void Enter();

    public virtual void Execute()
    {
        // 이동과 점프는 지속적으로 실행되어야 한다.
        Move();
        Jump();
    }

    public abstract void Exit();

    #region Move (Arrows / WASD)

    // 플레이어의 위치 값
    protected Transform playerTransform;
    // 플레이어를 비추는 카메라; 시네머신(Cinemachine)이 부착된 메인 카메라를 지정한다.
    protected Transform cameraTransform;

    // 방향 키의 입력 값
    protected Vector2 inputVector;
    // 실제 이동 벡터; 아래의 점프 함수에도 적용받는다.
    protected Vector3 moveVector;

    // 이동의 속도
    protected float moveSpeed;
    // 회전의 속도
    protected float rotateSpeed;

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
        // 필요한 변수를 PlayerController의 변수를 '참조'하도록 한다.
        playerController.GetMoveFieldRefs(playerTransform, cameraTransform, ref moveSpeed, ref rotateSpeed);

        // 저장한 입력 값을 카메라의 시야를 기준으로 하여 Vector3로 변환한다. 또한, 값을 정규화하여 대각선으로의 이동을 정상화한다.
        Vector3 vector = Vector3.Normalize(inputVector.x * cameraTransform.right + inputVector.y * cameraTransform.forward) * moveSpeed;

        // 카메라의 시야는 위, 아래를 향할 수 있으나, 캐릭터는 그 방향으로는 움직이지 않아야 한다.
        vector.y = 0f;

        // 이동하고 있는지를 판별한다.
        bool isMove = (vector != Vector3.zero);

        // 이동하는 것이라면,
        if (isMove)
        {
            // 캐릭터를 그 방향으로 회전시킨다. (이 코드는 조건을 걸지 않을 경우, 입력 값이 없을 때 항상 원점으로 회전하기 때문에 조건문이 필요하다.)
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, Quaternion.LookRotation(vector), rotateSpeed * Time.deltaTime);
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
    protected bool isJumping = false;

    // 점프의 강도
    protected float jumpPower;
    // 중력 가속도
    protected float gravityScale;

    // 인풋 시스템(Input System)으로 값을 입력 받아, 점프를 수행하는 함수
    private void OnJump(InputValue inputValue)
    {
        // 플레이어가 땅에 닿아 있을 때만,
        if (characterController.isGrounded)
        {
            // 점프한다.
            isJumping = true;
        }
    }

    // 플레이어의 점프를 구현하는 함수
    public virtual void Jump()
    {
        // 필요한 변수를 PlayerController의 변수를 '참조'하도록 한다.
        playerController.GetJumpFieldRefs(ref jumpPower, ref gravityScale);

        // 점프 키를 눌렀다면
        if (isJumping)
        {
            // 위로 점프한다.
            moveVector.y = Mathf.Sqrt(jumpPower * -2.0f * gravityScale);

            // 연속으로 점프할 수 없다.
            isJumping = false;
        }

        // 캐릭터가 공중에 있을 경우,
        if (!characterController.isGrounded)
        {
            // 아래로 점점 떨어진다.
            moveVector.y += gravityScale * Time.deltaTime;
        }
    }

    #endregion Jump

    public abstract void Shift();
    public abstract void Attack();
    public abstract void Skill01();
    public abstract void Skill02();

    #endregion Interface Methods
}
