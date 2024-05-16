using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    public class CheckNearToChase : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 필드(Field)
        private float detectRange; // 플레이어를 탐지하는 범위

        // 생성자
        public CheckNearToChase(Enemy enemy)
        {
            _enemy = enemy;
            detectRange = enemy.DetectRange;
        }

        public override NodeState Evaluate()
        {
            // 적과 플레이어 사이의 거리가 일정 미만(탐지 범위 내)일 경우,
            if (Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position) <= detectRange)
            {
                // 성공 상태를 반환한다. (Composite 안의 다음 노드로 이동한다.)
                state = NodeState.SUCCESS;
                return state;
            }
            // 아닐 경우,
            else
            {
                // 실패 상태를 반환한다. (Composite 노드를 종료한다.)
                state = NodeState.FAILURE;
                return state;
            }
        }
    }
}
