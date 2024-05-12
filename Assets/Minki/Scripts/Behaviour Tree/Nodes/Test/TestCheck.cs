using BehaviourTree;
using UnityEngine;

public class TestCheck : Node
{
    public override NodeState Evaluate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            state = NodeState.SUCCESS;
            Debug.Log(state);
            return state;
        }

        if (Input.GetKey(KeyCode.S))
        {
            state = NodeState.RUNNING;
            Debug.Log(state);
            return state;
        }

        return NodeState.FAILURE;
    }
}
