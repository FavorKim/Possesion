using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBT : MonoBehaviour
    {
        // Enemy 객체
        public Enemy _enemy { get; set; }

        Node rootNode;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            rootNode = SetBT();
            
        }

        private void Update()
        {
            rootNode.Evaluate();
        }

        public Node SetBT()
        {
            Node node = new Selector(new List<Node>
            {
                // 1. 빙의 상태 여부 확인
                new CheckIsPossessed(_enemy),

                // 2. 죽음 상태 여부 확인
                new Sequence(new List<Node>()
                {
                    new CheckIsDead(_enemy), new Die(_enemy),
                }),

                // 3. 피격 상태 여부 확인
                new Sequence(new List<Node>()
                {
                    new CheckGetHit(_enemy), new GetHit(_enemy)
                }),

                // 4. 공격 가능 상태 여부 확인
                new Sequence(new List<Node>()
                {
                    new CheckNearToAttack(_enemy), new Attack(_enemy)
                }),

                // 5. 추격 가능 상태 여부 확인
                new Sequence(new List<Node>()
                {
                    new CheckNearToChase(_enemy), new Chase(_enemy),
                }),

                // 6. 순찰
                new Patrol(_enemy),
            });

            return node;
        }
    }
}
