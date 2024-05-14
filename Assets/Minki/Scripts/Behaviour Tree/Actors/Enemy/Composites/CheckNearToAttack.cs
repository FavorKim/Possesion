using BehaviourTree;
using UnityEngine;

namespace Enemy
{
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
            _attackRange = enemy._attackRange;
        }

        public override NodeState Evaluate()
        {
            float dis = Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position);
            Debug.Log($"Distance = {dis}");
            Debug.Log($"Attack Range = {_attackRange}");

            // 적과 플레이어 사이의 거리가 일정 미만(공격 범위 내)일 경우,
            if (Vector3.Distance(_enemy._enemyTransform.position, _enemy.GetPlayerTransform().position) <= _attackRange)
            {
                Debug.Log("CheckNearToAttack = Success");

                // 다음 노드로 진행한다.
                state = NodeState.SUCCESS;
                return state;
            }
            // 아닐 경우,
            else
            {
                Debug.Log("CheckNearToAttack = Failure");

                // 이 노드를 실패 처리한다.
                state = NodeState.FAILURE;
                return state;
            }
        }
    }
}
