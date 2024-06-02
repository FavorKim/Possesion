using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    #region Components
    // 카메라(Camera); 세니머신(Cinemachine)이 부착된 메인 카메라를 지정한다.
    [SerializeField] private Transform cameraTransform;

    // 캐릭터 컨트롤러(Character Controller)
    private CharacterController characterController;
    // 애니메이터(Animator)
    private Animator animator;
    #endregion Components

    // 이동에 필요한 변수들(Fields)
    private Vector2 inputVector; // 입력 값
    private Vector3 moveVector; // 이동 값

    [SerializeField] private float moveSpeed; // 이동의 속도
    [SerializeField] private float rotateSpeed; // 회전의 속도
    [SerializeField] private float jumpPower; // 점프의 강도
    [SerializeField] private float gravityScale = Physics.gravity.y; // 중력 가속도; 기본 값은 -9.81f

    private bool isJumping = false; // 점프 유무
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 캐릭터를 이동/점프시킨다.
        CheckMove();
        CheckJump();
        characterController.Move(moveVector * Time.deltaTime);


    }

    // 인풋 시스템(Input System)으로 값을 입력 받아, 캐릭터의 이동을 구현하는 함수
    private void OnMove(InputValue inputValue)
    {
        // 이 함수는 입력 값이 들어오거나 나가는 순간에만 호출되므로, 입력 값을 받는 작업만 수행한다.

        // 입력 받은 값을 Vector2로 변환하여 저장한다.
        inputVector = inputValue.Get<Vector2>();
    }

    // 인풋 시스템(Input System)으로 값을 입력 받아, 캐릭터의 점프를 구현하는 함수
    private void OnJump(InputValue inputValue)
    {
        // 캐릭터가 땅에 닿아 있을 때만,
        if (characterController.isGrounded)
        {
            // 점프한다.
            isJumping = true;
        }
    }

    // 캐릭터의 이동을 구현하는 함수
    private void CheckMove()
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
            // 캐릭터를 그 방향으로 회전시킨다. (이 코드는 조건을 걸지 않을 경우, 입력 값이 없을 때 항상 원점으로 회전하기 때문에 조건문이 필요하다.)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), rotateSpeed * Time.deltaTime);
        }

        // 적절한 애니메이션을 재생한다.
        animator.SetBool("isRun", isMove); // bool 매개변수를 사용할 경우
        // animator.SetFloat("MoveSpeed", moveVector.normalized.magnitude); // float 매개변수를 사용할 경우; Blend Tree를 활용할 수 있다.

        // y 값을 제외한 나머지 값을 이동 벡터에 저장해 둔다.
        moveVector = new Vector3(vector.x, moveVector.y, vector.z);
    }

    // 캐릭터의 점프를 구현하는 함수
    private void CheckJump()
    {
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
}
