using System.Collections.Generic;

namespace BehaviourTree
{
    // Selector(OR) 클래스
    public class Selector : Node
    {
        // 생성자
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }


        // 평가 함수
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE: // FAILURE: 다음 자식 노드로 이동한다.
                        continue;

                    case NodeState.SUCCESS: // SUCCESS: SUCCESS를 반환한다.
                        state = NodeState.SUCCESS;
                        return state;

                    case NodeState.RUNNING: // RUNNING: RUNNING을 반환한다.
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