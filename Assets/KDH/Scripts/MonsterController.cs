using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //State랑 스킬이랑 별개로 분류하고, 

    enum MonsterState
    { 
        Idle,
        Chase,
        Attack
    }

    public StateMachine stateMachine;

    private Dictionary<MonsterState, IState> dicState = new Dictionary<MonsterState, IState>();

    // Start is called before the first frame update
    void Start()
    {
        IState idle = new IdleState();

        dicState.Add(MonsterState.Idle, idle);
        dicState.Add(MonsterState.Chase, gameObject.AddComponent<ChaseState>());
        dicState.Add(MonsterState.Attack, gameObject.AddComponent<AttackState>());

        stateMachine = new StateMachine(idle);
    }
    /// <summary>
    /// 몬스터의 State를 세팅해줄 때
    /// </summary>
    /// <param name="state"></param>
    public void setMonsterState(IState state)
    {
        stateMachine.SetState(state);
    }
}

public class StateMachine
{
    public IState currentState { get; private set; }

    /// <summary>
    /// 기본 상태 시 생성자를 만들어 줌.
    /// </summary>
    /// <param name="defaultState"></param>
    public StateMachine(IState defaultState)
    {
        currentState = defaultState;
    }

    public void SetState(IState state)
    {
        //상태 변화 없을 때, 예외처리
        if (currentState == state)
        {
            Debug.Log("현재 이미 해당 상태입니다.");
            return;
        }

        currentState.StateEnd();

        currentState = state;

        currentState.StateEnter();
    }
}
public class IdleState : MonoBehaviour, IState
{
    //Idle에는 멈추는 거랑 패트롤까지 구현
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}
public class ChaseState : MonoBehaviour, IState
{
    //Chase에는 추격만
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}

public class AttackState : MonoBehaviour, IState
{
    //Attack에는 공격, 스킬1, 스킬2 중에 상황봐서 쓸 것.
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateEnd() { }
}

public interface IState
{
    void StateEnter();
    void StateUpdate();
    void StateEnd();
}