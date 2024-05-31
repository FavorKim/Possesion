using System.Collections.Generic;

// 플레이어의 상태 기계를 관리하는 클래스
public class TestStateMachine
{
    #region Fields

    // 플레이어의 현재 상태를 다루는 클래스
    private TestPlayerState currentState;

    // 상태 목록을 가지는 Dictionary
    private Dictionary<string, TestPlayerState> states = new Dictionary<string, TestPlayerState>();

    #endregion Fields

    // 생성자 (Constructor)
    public TestStateMachine(TestPlayer playerController)
    {
        // 상태 목록에 플레이어가 취할 수 있는 모든 상태들을 등록한다.
        states.Add("Normal", new TestNormalState(playerController));
        states.Add("Possessing", new TestPossessingState(playerController));

        // 최초의 상태는 일반 상태이다.
        currentState = states["Normal"];
    }

    #region UpdateState / ChangeState

    // 주기적으로 갱신(호출)하는 함수
    public void UpdateState()
    {
        currentState.Execute();
    }

    // 상태가 변할 때 호출하는 함수 (Normal)
    public void ChangeState(TestPlayer player)
    {
        currentState.Exit(); // 현재 상태를 빠져나오고,
        currentState = states["Normal"]; // 현재 상태를 다음 상태로 바꾼다.
        currentState.Enter(); // 다음 상태에 들어간다.
    }

    // 상태가 변할 때 호출하는 함수 (Monster)
    public void ChangeState(TestMonster monster)
    {
        currentState.Exit(); // 현재 상태를 빠져나오고,
        currentState = states["Possessing"]; // 현재 상태를 다음 상태로 바꾼다.
        ((TestPossessingState)currentState).GetMonster(monster); // 받은 몬스터로 바꾼다.
        currentState.Enter(); // 다음 상태에 들어간다.
    }

    #endregion UpdateState / ChangeState
}
