using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    // 플레이어가 공격 범위 내에 있는지를 판별하는 클래스
    public class CheckNearToAttack : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Transform _playerTransform;

        // 생성자
        public CheckNearToAttack(Enemy enemy)
        {
            _enemy = enemy;

            // 플레이어는 생성자의 호출 시점에서 Find 함수를 사용하여 찾는다.
            _playerTransform = Object.FindObjectOfType<PlayerController>().transform;
        }

        public override NodeState Evaluate()
        {
            // 거리 계산 시 y축(상하)는 계산하지 않는다.
            Vector3 enemyTransform = new Vector3(_enemy.transform.position.x, 0, _enemy.transform.position.z);
            Vector3 playerTransform = new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z);

            // 적과 플레이어 사이의 거리
            float distance = Vector3.Distance(enemyTransform, playerTransform);

            // 거리가 공격 범위 안일 경우,
            if (distance <= _enemy.AttackRange)
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
