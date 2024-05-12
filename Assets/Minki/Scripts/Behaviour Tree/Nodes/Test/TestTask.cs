using BehaviourTree;
using UnityEngine;

public class TestTask : Node
{
    Transform _transform;

    public TestTask(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("TestTask Evaluate(), state = " + state);

        _transform.Translate(Vector3.forward * Time.deltaTime);

        return NodeState.FAILURE;
    }
}
