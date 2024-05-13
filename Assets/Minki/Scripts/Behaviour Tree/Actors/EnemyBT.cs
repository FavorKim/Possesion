using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBT : MonoBehaviour
{
    Node rootNode;

    private void Awake()
    {
        rootNode = SetBT();
    }

    private void Update()
    {
        rootNode.Evaluate();
    }

    private Node SetBT()
    {
        Node node = new Selector(new List<Node>
        {
            new Sequence(new List<Node>()
            {
                new Patrol(), new Chase(), new Attack()
            }),

            new Idle()
        });

        return node;
    }
}


public class Patrol : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("Patrol!");

        return NodeState.FAILURE;
    }
}

public class Chase : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("Chase!");

        return NodeState.RUNNING;
    }
}

public class Attack : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("Attack!");

        return NodeState.SUCCESS;
    }
}

public class Idle : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("Idle!");

        return NodeState.SUCCESS;
    }
}