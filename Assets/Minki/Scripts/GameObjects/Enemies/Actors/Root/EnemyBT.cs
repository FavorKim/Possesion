using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    // 적(Enemy)의 행동 트리(BT; Behaviour Tree) 클래스
    public class EnemyBT : MonoBehaviour
    {
        // Enemy 객체
        public Enemy _enemy { get; set; }

        // 뿌리(Root) 노드
        Node rootNode;

        private void Awake()
        {
            // 생성 시 자신의 Enemy(를 상속받는 클래스) 스크립트를 컴포넌트에서 참조한다.
            _enemy = GetComponent<Enemy>();

            // 행동 트리를 작성한다.
            rootNode = SetBT();
        }

        private void Update()
        {
            // 매 프레임마다 상태를 평가한다.
            rootNode.Evaluate();
        }

        // 행동 트리를 작성한다.
        public Node SetBT()
        {
            /*
                                                   Root
                                                     ┃
                                                 Selector
                                 ┏━━━━━━━━━━━━━━━━━━━┻━━━━━━━━━━━━━━━━━━┓
                             Sequence                               Selector
                        ┏━━━━━━━━┻━━━━━━━━┓            ┏━━━━━━━━━━━━━━━━┻━━━━━━━━━━━━━━━━┓
                CheckIsPossessed → BeingPossessed  Sequence                          Selector
                                                  ┏━━━━┻━━━━┓       ┏━━━━━━━━━━━━━━━━━━━━┻━━━━━━━━━━━━━━━━━━━┓
                                             CheckIsDead → Die  Sequence                                 Selector
                                                              ┏━━━━━┻━━━━━┓        ┏━━━━━━━━━━━━━━━━━━━━━━━━━╋━━━━━━━━━━━━━━━━━━━━━━━━━┓
                                                         CheckGetHit → GetHit  Sequence                  Sequence                   Patrol
                                                                           ┏━━━━━━━┻━━━━━━┓           ┏━━━━━━┻━━━━━━┓
                                                                   CheckNearToAttack → AIAttack  CheckNearToChase → Chase

            */

            // 0. 뿌리 노드(Selector)
            Node node = new Selector(new List<Node>
            {
                // 1. 빙의 상태 여부 확인
                new Sequence(new List<Node>()
                {
                   new CheckIsPossessed(_enemy), new BeingPossessed(_enemy) 
                }),

                new Selector(new List<Node>()
                {
                    // 2. 죽음 상태 여부 확인
                    new Sequence(new List<Node>()
                    {
                        new CheckIsDead(_enemy), new Die(_enemy)
                    }),

                    new Selector(new List<Node>()
                    {
                        // 3. 피격 상태 여부 확인
                        new Sequence(new List<Node>()
                        {
                            new CheckGetHit(_enemy), new GetHit(_enemy)
                        }),

                        new Selector(new List<Node>()
                        {
                            // 4. 공격 가능 상태 여부 확인
                            new Sequence(new List<Node>()
                            {
                                new CheckNearToAttack(_enemy), new Attack(_enemy)
                            }),

                            // 5. 추격 가능 상태 여부 확인
                            new Sequence(new List<Node>()
                            {
                                new CheckNearToChase(_enemy), new Chase(_enemy)
                            }),

                            // 6. 순찰
                            new Patrol(_enemy)
                        })

                    })

                })

            });

            // 작성한 노드를 반환한다.
            return node;
        }
    }
}
