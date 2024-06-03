using UnityEngine;

public class TestPossessingState : TestPlayerState
{
    #region Fields

    // 빙의한 몬스터
    private TestMonster possessingMonster;

    private float pM_moveSpeed;
    private float pM_rotateSpeed;
    private float pM_jumpPower;

    private Skill pM_skill00;
    private Skill pM_skill01;
    private Skill pM_skill02;

    private ParticleSystem poExitParticle;

    #endregion Fields

    public TestPossessingState(TestPlayer playerController) : base(playerController)
    {
        poExitParticle = playerController.GetPoExitParticle();
    }

    public override void Enter()
    {
        // 플레이어의 모습을 감춘다.
        playerController.PlayerOutfit.SetActive(false);

        // 몬스터의 스탯을 적용한다.
        playerController.SetSpeeds(pM_moveSpeed, pM_rotateSpeed, pM_jumpPower);
        playerController.SetSkills(pM_skill00, pM_skill01, pM_skill02);

        // 플레이어의 빙의 지속 가능 시간을 나타낸다.
        playerController.DurationGauge.gameObject.SetActive(true);
        playerController.DurationGauge.value = 1;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        // 효과를 재생한다.
        poExitParticle.Play();

        // 몬스터를 자식에서 해제시킨다.
        possessingMonster.transform.parent = null;

        // 빙의 상태일 때만 사용하는 UI를 해제한다.
        playerController.DurationGauge.gameObject.SetActive(false);
    }


    public void GetMonster(TestMonster monster)
    {
        // 빙의한 몬스터를 받아온다.
        possessingMonster = monster;

        // 몬스터에 빙의한다.
        monster.transform.SetParent(playerController.transform);
        monster.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // 빙의한 몬스터로부터 필요한 스탯을 가져온다.
        possessingMonster.GetSpeeds(out pM_moveSpeed, out pM_rotateSpeed, out pM_jumpPower);
        possessingMonster.GetSkills(out pM_skill00, out pM_skill01, out pM_skill02);
    }
}
