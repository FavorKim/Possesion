using BehaviourTree;
using UnityEngine;

public class TestTask2 : Node
{
    Transform _transform;

    public TestTask2(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("TestTask2 Evaluate(), state = " + state);

        _transform.Translate(Vector3.up * Time.deltaTime);

        return NodeState.SUCCESS;
    }
}
