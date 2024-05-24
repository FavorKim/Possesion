using BehaviourTree;
using UnityEngine;

namespace Enemy
{
    // 적의 피격을 구현하는 클래스
    public class GetHit : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Animator _animator;

        // 생성자
        public GetHit(Enemy enemy)
        {
            _enemy = enemy;
            _animator = enemy.GetComponent<Animator>();
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 피격한다.
            DoGetHit();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 피격을 담당하는 함수
        private void DoGetHit()
        {
            // 빙의 상태를 해제한다.
            // _enemy.IsPossessed = false;

            // 피격 애니메이션을 재생한다.
            _animator.SetTrigger("GetHit");

            // 한 번 피격된 후, 계속 피격되지 않도록 한다.
            _enemy.IsGetHit = false;
        }
    }
}
