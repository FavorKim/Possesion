using BehaviourTree;
using UnityEngine.AI;
using UnityEngine;

namespace Enemy
{
    // 적의 추적을 구현하는 클래스
    public class Chase : Node
    {
        // 필드(Field)
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _playerTransform;

        // 생성자
        public Chase(Enemy enemy)
        {
            _animator = enemy.GetComponent<Animator>();
            _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

            // 플레이어는 생성자의 호출 시점에서 Find 함수를 사용하여 찾는다.
            _playerTransform = Object.FindObjectOfType<PlayerController>().transform;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 추적한다.
            DoChase();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 추적을 담당하는 함수
        private void DoChase()
        {
            // 추적 애니메이션을 재생한다.
            _animator.SetBool("AI_Chase", true);
            _animator.SetInteger("AttackIndex", 0);

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

            // 플레이어를 추적한다.
            if (!isRunningState)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_playerTransform.position);
            }
        }
    }
}
