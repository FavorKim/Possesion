using UnityEngine;

public class TestNormalState : TestPlayerState
{
    #region Fields

    private float moveSpeed;
    private float rotateSpeed;
    private float jumpPower;

    #endregion Fields

    // 생성자
    public TestNormalState(TestPlayer playerController) : base(playerController)
    {
        // 플레이어의 기본 속성 값을 저장해 둔다.
        playerController.GetSpeeds(out moveSpeed, out rotateSpeed, out jumpPower);
    }

    // 빙의하지 않은 상태로 진입 시,
    public override void Enter()
    {
        // 플레이어의 속성 값을 초기화한다.
        playerController.SetSpeeds(moveSpeed, rotateSpeed, jumpPower);

        // 플레이어의 스킬을 초기화한다. (기본 상태에서는 가지고 있는 기술이 없다.)
        playerController.SetSkills(null, null, null);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
