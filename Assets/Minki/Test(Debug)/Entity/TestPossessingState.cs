using UnityEngine;

public class TestPossessingState : TestPlayerState
{
    #region Fields

    // 플레이어의 스탯
    private float moveSpeed;
    private float rotateSpeed;
    private float jumpPower;

    private Skill skill00;
    private Skill skill01;
    private Skill skill02;


    // 빙의한 몬스터
    private TestMonster possessingMonster;

    private float pM_moveSpeed;
    private float pM_rotateSpeed;
    private float pM_jumpPower;

    private Skill pM_skill00;
    private Skill pM_skill01;
    private Skill pM_skill02;

    #endregion Fields

    public TestPossessingState(TestPlayer playerController) : base(playerController)
    {
        playerController.GetSpeeds(out moveSpeed, out rotateSpeed, out jumpPower);
        playerController.GetSkills(out skill00, out skill01, out skill02);
    }

    public override void Enter()
    {
        // 플레이어의 모습을 감춘다.
        playerController.PlayerOutfit.SetActive(false);

        // 몬스터의 스탯을 적용한다.
        playerController.SetSpeeds(pM_moveSpeed, pM_rotateSpeed, pM_jumpPower);
        skill00 = pM_skill00;
        skill01 = pM_skill01;
        skill02 = pM_skill02;

        // 플레이어의 빙의 지속 가능 시간을 나타낸다.
        playerController.DurationGauge.gameObject.SetActive(true);
        playerController.DurationGauge.value = 1;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
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
