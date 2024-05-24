using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    // 적이 플레이어에게 빙의되었을 때의 상태를 구현하는 클래스
    public class BeingPossessed : Node
    {
        // 필드(Field)
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly PlayerController _player;

        // 생성자
        public BeingPossessed(Enemy enemy)
        {
            _animator = enemy.GetComponent<Animator>();
            _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

            // 플레이어는 생성자의 호출 시점에서 Find 함수를 사용하여 찾는다.
            _player = Object.FindObjectOfType<PlayerController>();
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 빙의 상태가 된다.
            DoBeingPossessed();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 빙의 상태를 담당하는 함수
        public void DoBeingPossessed()
        {
            // 모든 애니메이션을 초기화하고, 빙의에 대응하는 애니메이션을 활성화한다.
            _animator.SetBool("IsPossessed", true);
            _animator.SetFloat("Player_Locomotion", _player.Dir.magnitude);

            _animator.SetBool("AI_Patrol_Move", false);
            _animator.SetBool("AI_Patrol_Sense", false);
            _animator.SetBool("AI_Chase", false);
            //_animator.SetInteger("AttackIndex", 0);

            // 네비게이션을 비활성화한다.
            _navMeshAgent.enabled = false;
        }
    }
}
