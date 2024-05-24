using BehaviourTree;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    // 적의 죽음을 구현하는 클래스
    public class Die : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Animator _animator;

        // 생성자
        public Die(Enemy enemy)
        {
            _enemy = enemy;
            _animator = enemy.GetComponent<Animator>();
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 죽는다.
            DoDie();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 죽음을 담당하는 함수
        private void DoDie()
        {
            // 빙의 상태를 해제한다.
            _enemy.IsPossessed = false;

            // 죽음 애니메이션을 재생한다.
            _animator.SetTrigger("Die");

            // 게임 오브젝트를 비활성화한다. 단, 죽음 애니메이션이 끝난 후 호출해야 하므로 일정 시간 여유를 둔다.
            _enemy.StartCoroutine(SetDelay(3.0f, () => _enemy.gameObject.SetActive(false))); // 약 3.0초 후 호출한다.
        }

        // 함수의 실행에 대기 시간을 적용하기 위한 코루틴 함수
        private IEnumerator SetDelay(float time, UnityAction unityAction)
        {
            // 받은 시간만큼 대기한다.
            yield return new WaitForSeconds(time);

            // 전달받은 대리자(UnityAction)를 호출한다.
            unityAction.Invoke();
        }
    }
}
