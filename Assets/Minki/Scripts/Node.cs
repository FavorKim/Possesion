using System.Collections.Generic;

namespace BehaviourTree
{
    // 노드의 상태를 나타내는 enum
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
    }

    public class Node
    {
        // 노드의 상태를 나타내는 enum
        protected NodeState state;


        // 부모 노드
        public Node parent;
        // 자식 노드
        protected List<Node> children = new List<Node>();


        // asdf
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();


        // 생성자
        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }


        // 노드를 부착하는 함수
        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }


        // 노드의 상태를 평가하는 함수
        public virtual NodeState Evaluate() => NodeState.FAILURE;


        // 데이터를 삽입하는 함수
        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }


        // 데이터를 가져오는 함수
        public object GetData(string key)
        {
            object value = null;

            if (dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;

            while (node != null)
            {
                value = node.GetData(key);

                if (value != null)
                {
                    return value;
                }

                node = node.parent;
            }

            return null;
        }


        // 데이터를 삭제하는 함수
        public bool RemoveData(string key)
        {
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);

                return true;
            }

            Node node = parent;

            while (node != null)
            {
                bool cleared = node.RemoveData(key);

                if (cleared)
                {
                    return true;
                }

                node = node.parent;
            }

            return false;
        }
    }
}

