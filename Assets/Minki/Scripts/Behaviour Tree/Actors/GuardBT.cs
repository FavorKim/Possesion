using System.Collections.Generic;
using BehaviourTree;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2.0f;
    public static float fovRange = 6.0f;
    public static float attackRange = 1.0f;

    // 트리를 구성하는 함수
    // Selector, Sequence 구조를 이용하여 트리를 구성한다.
    protected override Node SetupTree()
    {
        /*
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),

            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),

            new TaskPatrol(transform, waypoints),
        });
        */

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TestCheck(), new TestTask(transform), new TestTask2(transform),
            })
        });

        return root;
    }
}
