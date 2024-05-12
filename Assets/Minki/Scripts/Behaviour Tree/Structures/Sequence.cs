using System.Collections.Generic;

namespace BehaviourTree
{
    // Sequence(AND) 클래스
    public class Sequence : Node
    {
        // 생성자
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        // 평가 함수
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE: // FAILURE: FAILURE를 반환한다.
                        state = NodeState.FAILURE;
                        return state;

                    case NodeState.SUCCESS: // SUCCESS: 다음 노드로 이동한다.
                        continue;

                    case NodeState.RUNNING: // RUNNING: 이번 노드를 다시 평가한다.
                        anyChildIsRunning = true;
                        continue;

                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }

}

