using System.Collections.Generic;

// 플레이어의 상태 기계를 관리하는 클래스
public class TestStateMachine
{
    #region Fields

    // 플레이어의 현재 상태를 다루는 클래스
    private TestPlayerState currentState;
    // 빙의하지 않은 일반 상태를 다루는 클래스
    private TestNormalState normalState;
    // 빙의한 상태를 다루는 클래스
    private TestPossessingState possessingState;
    // 상태 목록을 가지는 Dictionary
    private Dictionary<string, TestPlayerState> states = new Dictionary<string, TestPlayerState>();

    // 플레이어의 컨트롤을 다루는 클래스
    private TestPlayer playerController;

    #endregion Fields

    // 생성자 (Constructor)
    public TestStateMachine(TestPlayer playerController)
    {
        this.playerController = playerController;

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
    public void ChangeState(string nextState)
    {
        currentState.Exit(); // 현재 상태를 빠져나오고,
        states[nextState].Enter(); // 다음 상태에 들어간다.
        currentState = states[nextState]; // 현재 상태를 다음 상태로 바꾼다.
    }

    // 상태가 변할 때 호출하는 함수 (Monster)
    public void ChangeState(TestMonster monster)
    {
        currentState.Exit();
        currentState = possessingState;
        possessingState.GetMonster(monster);
    }

    #endregion UpdateState / ChangeState

    public bool IsPossessing() { return currentState == possessingState; }
}
