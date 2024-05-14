namespace BehaviourTree
{
    // 노드의 상태를 나타내는 열거형
    public enum NodeState
    {
        SUCCESS, RUNNING, FAILURE
    }

    // 노드 클래스
    public abstract class Node
    {
        // 노드의 상태
        protected NodeState state { get; set; }

        // 노드의 상태를 평가하는 함수
        public abstract NodeState Evaluate();
    }
}
