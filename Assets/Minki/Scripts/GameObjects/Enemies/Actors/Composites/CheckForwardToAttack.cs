using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    // 플레이어가 추적 범위 내에 있는지를 판별하는 클래스
    public class CheckForwardToAttack : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Animator _animator;
        private readonly Transform _playerTransform;

        // 생성자
        public CheckForwardToAttack(Enemy enemy)
        {
            _enemy = enemy;
            _animator = enemy.GetComponent<Animator>();

            // 플레이어는 생성자의 호출 시점에서 Find 함수를 사용하여 찾는다.
            _playerTransform = Object.FindObjectOfType<PlayerController>().transform;
        }

        public override NodeState Evaluate()
        {
            // 공격 모션에 대한 후딜레이를 적용한다.
            string[] animStates = { "Attack", "Skill 01", "Skill 02" };
            bool isRunningState = false;

            foreach (string state in animStates)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName(state))
                {
                    isRunningState = true;
                }
            }

            // 공격하고 있지 않을 때,
            if (!isRunningState)
            {
                // 공격을 수행하기 전, 적이 플레이어를 바라보고 있어야 한다.
                Vector3 playerPos = new Vector3(_playerTransform.position.x, _enemy.transform.position.y, _playerTransform.position.z); // 플레이어의 위치, y 값(상하)은 무시한다.
                Vector3 enemyPos = _enemy.transform.position; // 적의 위치

                float angle = Vector3.Angle(_enemy.transform.forward, playerPos - enemyPos); // 플레이어와 적 간의 각도

                if (angle > 10) // 그 각도가 약 좌우 각 10도 이상일 경우, (바라보고 있지 않다면)
                {
                    // 플레이어를 바라보게 회전시킨다.
                    Quaternion turnTo = Quaternion.LookRotation(playerPos - enemyPos);
                    _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, turnTo, 0.01f);

                    // 10도 미만일 때까지 계속한다. (RUNNING)
                    state = NodeState.RUNNING;
                }
                else
                {
                    // 아니라면 공격으로 넘어간다. (SUCCESS)
                    state = NodeState.SUCCESS;
                }
            }
            else
            {
                state = NodeState.SUCCESS;
            }

            // 상태를 반환한다.
            return state;
        }
    }
}
