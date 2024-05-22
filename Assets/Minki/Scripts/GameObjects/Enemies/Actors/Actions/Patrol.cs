using BehaviourTree;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemy
{
    // 적의 순찰을 구현하는 클래스
    public class Patrol : Node
    {
        // 필드(Field)
        private readonly Enemy _enemy;
        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;

        private readonly Transform[] _patrolTransforms; // 순찰할 위치들

        private int _patrolIndex = 0; // 다음으로 이동할 순찰 위치의 변수
        private bool _isCorRunning = false; // 코루틴이 실행 중인지를 판별하는 변수

        private UnityAction _senseAction; // 순찰의 종류 1. 가만히 서서 주변을 탐색한다.
        private UnityAction _patrolAction; // 순찰의 종류 2. 일정 구역을 돌아다닌다.

        // 생성자
        public Patrol(Enemy enemy)
        {
            _enemy = enemy;
            _animator = enemy.GetComponent<Animator>();
            _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

            _patrolTransforms = enemy.PatrolTransforms;

            SetActions();
        }

        // 평가 함수
        public override NodeState Evaluate()
        {
            // 순찰한다.
            DoPatrol();

            // 성공 상태를 반환한다.
            return NodeState.SUCCESS;
        }

        // 순찰을 담당하는 함수
        private void DoPatrol()
        {
            // 네비게이션을 실행한다.
            _navMeshAgent.isStopped = false;

            // 기본적으로, (그리고 주변을 둘러보는 함수가 실행 중이지 않을 때,)
            if (!_isCorRunning)
            {
                // 일정 구역을 돌아다닌다.
                _patrolAction.Invoke();
            }

            // 순찰 목적지까지 이동했다면,
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_isCorRunning)
            {
                // 잠시 주변을 둘러본다. (약 5.0초간)
                _enemy.StartCoroutine(SetDelay(5.0f, _senseAction));
            }
        }

        // 순찰의 종류를 지정하는 함수
        private void SetActions()
        {
            _senseAction = () =>
            {
                // 주변을 탐색하는 애니메이션을 재생한다.
                _animator.SetBool("AI_Patrol_Sense", true);
                _animator.SetBool("AI_Patrol_Move", false);
                _animator.SetBool("AI_Chase", false);

                // 다음 순찰 위치를 지정한다.
                _patrolIndex = ++_patrolIndex % _patrolTransforms.Length;
            };

            _patrolAction = () =>
            {
                // 돌아다니는 애니메이션을 재생한다.
                _animator.SetBool("AI_Patrol_Move", true);
                _animator.SetBool("AI_Patrol_Sense", false);
                _animator.SetBool("AI_Chase", false);

                // 정해진 순찰 구역으로 이동한다.
                _navMeshAgent.SetDestination(_patrolTransforms[_patrolIndex].position);
            };
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
