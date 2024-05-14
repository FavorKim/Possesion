using System.Collections.Generic;

namespace BehaviourTree
{
    /*
     * Selector: 자식 노드를 순회하다가,
     * Failure 상태인 노드를 만날 경우, 다음 노드를 평가한다.
     * Success/Running 상태인 노드를 만날 경우, 그 노드까지만 실행하고 멈춘다.
    */

    public class Selector : Node
    {
        // 자식 노드 목록
        private List<Node> children;

        public Selector(List<Node> children)
        {
            this.children = children;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 만약 자식 노드가 없다면,
            if (children == null || children.Count == 0)
            {
                // Failure를 반환한다.
                return NodeState.FAILURE;
            }

            // 자식 노드를 순회하면서,
            foreach (Node child in children)
            {
                // 상태를 평가한다.
                switch (child.Evaluate())
                {
                    // Running일 경우, Running을 반환한다.
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;

                    // Success일 경우, Success를 반환한다.
                    case NodeState.SUCCESS:
                        return NodeState.SUCCESS;

                    // Failure일 경우, 다음 노드를 평가한다.
                    case NodeState.FAILURE:
                        continue;
                }
            }

            // 전부 순회했을 경우, Failure를 반환한다. (아무것도 선택하여 실행하지 않았다는 뜻)
            return NodeState.FAILURE;
        }
    }
}
