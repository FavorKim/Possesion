using UnityEngine;

public class TestPossessingState : TestPlayerState
{
    // 빙의한 몬스터
    private TestMonster possessingMonster;

    public TestPossessingState(TestPlayer playerController) : base(playerController) { }

    public override void Enter()
    {
        // 플레이어의 모습을 감춘다.
        playerController.PlayerOutfit.SetActive(false);

        // 몬스터의 일반 공격 및 스킬을 습득한다.
        playerController.Skill00 = possessingMonster.Skill00;
        playerController.Skill01 = possessingMonster.Skill01;
        playerController.Skill02 = possessingMonster.Skill02;

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

        // 몬스터의 속성 값을 적용한다.
        playerController.MoveSpeed = monster.MoveSpeed;
        playerController.JumpPower = monster.JumpPower;
    }
}
