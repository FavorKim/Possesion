using UnityEngine;

namespace BehaviourTree
{
    // 트리(Tree) 클래스
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }

}

