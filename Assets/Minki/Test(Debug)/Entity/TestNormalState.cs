using UnityEngine;

public class TestNormalState : TestPlayerState
{
    public TestNormalState(TestPlayer playerController) : base(playerController) { }

    // 빙의하지 않은 상태로 진입 시,
    public override void Enter()
    {
        // 스킬을 초기화한다.
        SkillManager.ResetSkill();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }


}
