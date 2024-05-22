using BehaviourTree;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace Enemy
{
    // 적의 공격을 구현하는 클래스
    public class Attack : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly NavMeshAgent _navMeshAgent;

        private int _attackSkillCount; // 공격 기술의 개수
        private bool _isCorRunning = false; // 코루틴이 실행 중인지를 판별하는 변수

        private UnityAction _attackAction;

        // 생성자
        public Attack(Enemy enemy)
        {
            _enemy = enemy;
            _navMeshAgent = _enemy.GetComponent<NavMeshAgent>();

            _attackSkillCount = _enemy.AttackSkillCount;
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 공격한다.
            DoAttack();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 공격을 담당하는 함수
        private void DoAttack()
        {
            // !! 중요 !!
            // 공격을 수행하기 전, 적이 플레이어를 바라보고 있어야 한다. 현재는 플레이어 근처에 있다면 방향에 상관 없이 공격을 수행하고 있다.

            // 추적을 멈춘다.
            _navMeshAgent.isStopped = true;

            // 소지한 공격 스킬 중 무작위로 하나를 고른다.
            int curAttackIndex = Random.Range(1, _attackSkillCount + 1);

            switch (curAttackIndex)
            {
                case 0:
                    _attackAction = null;
                    break;
                case 1:
                    _attackAction = _enemy.Attack;
                    break;
                case 2:
                    _attackAction = _enemy.Skill1;
                    break;
                case 3:
                    _attackAction = _enemy.Skill2;
                    break;
                default:
                    _attackAction = null;
                    break;
            }

            // 코루틴을 사용하여, 일정 주기마다 스킬을 달리하여 공격한다.
            if (!_isCorRunning)
            {
                _enemy.StartCoroutine(SetDelay(1.0f, _attackAction));
            }
        }

        // 함수의 실행에 대기 시간을 적용하기 위한 코루틴 함수
        private IEnumerator SetDelay(float time, UnityAction unityAction)
        {
            // 코루틴을 시작한다.
            _isCorRunning = true;

            // 전달받은 대리자(UnityAction)를 호출한다.
            unityAction.Invoke();

            // 받은 시간만큼 대기한다.
            yield return new WaitForSeconds(time);

            // 코루틴을 종료한다.
            _isCorRunning = false;
        }
    }
}
