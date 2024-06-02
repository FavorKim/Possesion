using UnityEngine;

public class TestNormalState : TestPlayerState
{
    #region Fields

    private float moveSpeed;
    private float rotateSpeed;
    private float jumpPower;

    private Skill skill00;
    private Skill skill01;
    private Skill skill02;

    #endregion Fields

    // 생성자
    public TestNormalState(TestPlayer playerController) : base(playerController)
    {
        // 플레이어의 기본 속성 값을 저장해 둔다.
        playerController.GetSpeeds(out moveSpeed, out rotateSpeed, out jumpPower);
        playerController.GetSkills(out skill00, out skill01, out skill02);
    }

    // 빙의하지 않은 상태로 진입 시,
    public override void Enter()
    {
        // 플레이어의 속성 값을 초기화한다. (값 형식)
        playerController.SetSpeeds(moveSpeed, rotateSpeed, jumpPower);

        // 플레이어의 스킬을 초기화한다. (참조 형식이)
        skill00 = null;
        skill01 = null;
        skill02 = null;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
