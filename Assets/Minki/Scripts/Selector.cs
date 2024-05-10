using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : Node
    {
        // 생성자
        public Selector() : base() { }

        public Selector(List<Node> children) : base(children) { }


        // 재정의 함수
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NodeState.FAILURE;

            return state;
        }
    }
}