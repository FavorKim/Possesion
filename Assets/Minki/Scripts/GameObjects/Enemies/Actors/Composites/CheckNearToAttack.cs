using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    // 플레이어가 공격 범위 내에 있는지를 판별하는 클래스
    public class CheckNearToAttack : Node
    {
        // 적(Enemy) 클래스
        private Enemy _enemy;

        // 필드(Field)
        private float _attackRange; // 공격을 시전하는 범위

        // 생성자
        public CheckNearToAttack(Enemy enemy)
        {
            _enemy = enemy;
            _attackRange = enemy.AttackRange;
        }

        public override NodeState Evaluate()
        {
            // 적과 플레이어 사이의 거리가 일정 미만(공격 범위 내)일 경우,
            if (Vector3.Distance(_enemy.transform.position, _enemy._playerTransform.position) <= _attackRange)
            {
                Debug.Log("Enemy Attack Success!");

                // 성공 상태를 반환한다. (Composite 안의 다음 노드로 이동한다.)
                state = NodeState.SUCCESS;
                return state;
            }
            // 아닐 경우,
            else
            {
                Debug.Log("Enemy Attack Failure!");

                // 실패 상태를 반환한다. (Composite 노드를 종료한다.)
                state = NodeState.FAILURE;
                return state;
            }
        }
    }
}
