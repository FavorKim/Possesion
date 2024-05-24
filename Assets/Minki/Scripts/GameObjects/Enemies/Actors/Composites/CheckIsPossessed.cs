using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    // 적이 플레이어에게 빙의되었는지를 판별하는 클래스
    public class CheckIsPossessed : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;

        // 생성자
        public CheckIsPossessed(Enemy enemy)
        {
            _enemy = enemy;
            _animator = enemy.GetComponent<Animator>();
            _navMeshAgent = _animator.GetComponent<NavMeshAgent>();
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            //Debug.Log(_enemy.IsPossessed);

            // 빙의 상태일 경우 성공 상태를, 아닐 경우 빙의 애니메이션을 해제하고, 실패 상태를 반환한다.
            if (_enemy.IsPossessed && _enemy.transform.root.GetComponent<PlayerController>())
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                _animator.SetBool("IsPossessed", false);
                _navMeshAgent.enabled = true;
                state = NodeState.FAILURE;
            }
            
            return state;
        }
    }
}
