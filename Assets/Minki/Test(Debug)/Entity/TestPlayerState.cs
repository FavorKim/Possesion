interface IPlayerState
{
    // State 클래스에 있어야 적절
    void Enter(); // 상태에 들어갈 때의 함수
    void Execute(); // 상태를 유지하는 동안 수행할 함수
    void Exit(); // 상태를 나갈 때의 함수
}

public abstract class TestPlayerState : IPlayerState
{
    protected TestPlayer playerController;

    // 생성자 (Constructor)
    public TestPlayerState(TestPlayer playerController)
    {
        this.playerController = playerController;
    }

    #region Interface Methods

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();

    #endregion Interface Methods
}
