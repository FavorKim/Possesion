using UnityEngine;

public class TestNormalState : TestPlayerState
{
    private float moveSpeed;
    private float jumpPower;

    public TestNormalState(TestPlayer playerController) : base(playerController)
    {
        // 플레이어의 기본 속성 값을 저장해 둔다.
        moveSpeed = playerController.MoveSpeed;
        jumpPower = playerController.JumpPower;
    }

    // 빙의하지 않은 상태로 진입 시,
    public override void Enter()
    {
        // 플레이어의 속성 값을 초기화한다.
        playerController.MoveSpeed = moveSpeed;
        playerController.JumpPower = jumpPower;

        // 플레이어의 스킬을 초기화한다.
        playerController.Skill00 = null;
        playerController.Skill01 = null;
        playerController.Skill02 = null;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
