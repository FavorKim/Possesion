using UnityEngine;

public class TestNormalState : TestPlayerState
{
    public TestNormalState(TestPlayer playerController) : base(playerController) { }

    // 빙의하지 않은 상태로 진입 시,
    public override void Enter()
    {
        // 스킬을 초기화한다.
        SkillManager.ResetSkill();

        playerController.MoveSpeed = moveSpeed;
        playerController.JumpPower = jumpPower;
    }

    public override void Exit()
    {
        playerOutfit.SetActive(false);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Jump()
    {
        base.Jump();
    }

    public override void Shift()
    {
        playerController.GetComponent<Animator>().SetTrigger("Shift");
    }

    public override void Attack() { }

    public override void Skill01() { }

    public override void Skill02() { }
}
