using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemy
{
    // 적(몬스터)를 정의하는 부모 클래스
    public abstract class Enemy : MonoBehaviour
    {
        // 적의 공통된 정보를 부모 클래스에서 우선 정의한다.

        #region Components

        // 컴포넌트(Components)
        private Animator _animator; // 적 자신의 애니메이터
        private NavMeshAgent _navMeshAgent; // 플레이어를 추적하기 위한 네비게이션(NavMesh)

        public Transform _playerTransform { get; private set; } // 플레이어의 위치(Transform)

        #endregion Components

        #region Fields

        // 필드(Fields)

        // 애니메이터를 AI용, 빙의용으로 2개 생성하여 교환할 수 있도록 수정할 것.

        private Action _attackAction;

        protected int _attackSkillCount; // 공격 기술 개수
        public bool IsPossessed { get; private set; } // 빙의 상태를 판별하는 변수
        public bool IsGetHit { get; private set; } // 피격을 판별하는 변수
        public bool IsDead { get; private set; } // 죽음을 판별하는 변수

        #region Enemy Stats

        // 필드(변수); 적의 스탯 정보
        public string Name { get; protected set; } // Name; 이름
        public int HealthPoint { get; protected set; } // HP; 체력
        public int MagicPoint { get; protected set; } // MP; 마력  
        public float MoveSpeed { get; protected set; } // SPD; 이동 속도
        public float JumpSpeed { get; protected set; } // J_SPD; 점프 속도
        public int AttackDamage { get; protected set; } // ATK; 기본 공격력
        public int Skiil1Damage { get; protected set; } // SK1; 스킬1 공격력
        public int Skill2Damage { get; protected set; } // SK2; 스킬2 공격력
        public int Skill3Damage {  get; protected set; } // SK3; 스킬3 공격력 (없는 적도 있음)
        public float SkillCoolTime { get; protected set; } // CT; 스킬 재사용 대기시간
        public float AttackRange { get; protected set; } // ATK_Range; 공격 범위
        public float DetectRange { get; protected set; } // DTC_Range; 플레이어 탐지 범위

        #endregion Enemy Stats

        #endregion Fields

        #region Delegates

        // 대리자 (코루틴용)
        private delegate void CorDelegate(); // 대리자
        private bool isCorRunning = false; // 코루틴의 중복 실행을 방지하기 위한 변수
        
        UnityAction _unityAction;


        #endregion Delegates

        #region Awake()

        protected virtual void Awake()
        {
            // 행동 트리의 뿌리(Root)를 생성한다.
            gameObject.AddComponent<EnemyBT>();

            // 필요한 컴포넌트들을 초기화한다.
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            // 현재 Scene에서 "PlayerController" 스크립트를 가진 게임 오브젝트(= 플레이어)를 찾는다.
            _playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        }

        #endregion Awake()

        #region Collision Events

        // 플레이어에게 피격(충돌) 시의 처리 함수
        private void OnCollisionEnter(Collision collision)
        {
            // 플레이어의 모자와 충돌했을 경우,
            if (collision.collider.CompareTag("Hat"))
            {
                // 빙의 상태가 된다.
                IsPossessed = true;
            }
        }

        #endregion Collision Events

        #region Coroutines

        // 일부 함수에 대해, 대기 시간을 포함한 함수를 호출하기 위한 코루틴 함수
        private IEnumerator SetDelay(float time, UnityAction unityAction, bool isCallFirst)
        {
            // 코루틴을 시작한다.
            isCorRunning = true;

            // 함수를 먼저 호출할 경우,
            if (isCallFirst)
            {
                // 전달받은 대리자(UnityAction)를 호출한다.
                unityAction.Invoke();

                // 받은 시간만큼 대기한다.
                yield return new WaitForSeconds(time);
            }
            // 대기 시간을 먼저 취할 경우,
            else
            {
                // 받은 시간만큼 대기한다.
                yield return new WaitForSeconds(time);

                // 전달받은 대리자를 호출한다.
                unityAction.Invoke();
            }

            // 코루틴을 종료한다.
            isCorRunning = false;
        }

        #endregion Coroutines

        #region Abstract Methods

        // 스탯을 초기화하는 함수, 각 자식 클래스에서 알맞게 수치를 조정하여 초기화한다.
        protected abstract void InitializeStats();

        #endregion Abstract Methods

        #region Action Methods

        // 적의 행동을 다루는 함수들, 부모 클래스에서 공통된 속성을 지정하고, 자식 클래스에서 각 특성에 맞게 추가한다.

        // 빙의를 담당하는 함수
        public virtual void BeingPossessed()
        {
            // 모든 애니메이션을 초기화한다.
            _animator.SetBool("Patrol", false);
            _animator.SetBool("LookAround", false);
            _animator.SetBool("Chase", false);
            _animator.SetInteger("AttackIndex", 0);

            Debug.Log("Enemy's BeingPossessed() is Called.");
        }

        // 피격을 담당하는 함수
        public virtual void GetHit()
        {
            // 피격 애니메이션을 재생한다.
            _animator.SetTrigger("GetHit");

            Debug.Log("Enemy's GetHit() is Called.");
        }

        // 죽음을 담당하는 함수
        public virtual void Die()
        {
            // 죽음 애니메이션을 재생한다.
            _animator.SetTrigger("Die");

            // 게임 오브젝트를 비활성화한다. 단, 죽음 애니메이션이 끝난 후 호출해야 하므로 일정 시간 여유를 둔다.
            StartCoroutine(SetDelay(3.0f, () => gameObject.SetActive(false), false)); // 약 3.0초 후 호출한다.

            Debug.Log("Enemy's Die() is Called.");
        }

        // 순찰을 구현하는 함수
        public virtual void Patrol()
        {
            // 순찰 애니메이션을 재생한다.
            _animator.SetBool("Patrol", true);
            // _animator.SetBool("LookAround", false);
            _animator.SetBool("Chase", false);

            Debug.Log("Enemy's Patrol() is Called.");
        }

        // 추적을 구현하는 함수
        public virtual void Chase()
        {
            // 추적 애니메이션을 재생한다.
            _animator.SetBool("Chase", true);
            _animator.SetInteger("AttackIndex", 0);

            // → NavMesh로 재구현할 것.
            _navMeshAgent.SetDestination(_playerTransform.position);

            Debug.Log("Enemy's Chase() is Called.");
        }

        // 공격을 구현하는 함수
        public virtual void Attack()
        {
            // 공격 스킬을 바꿀 주기
            float changeTime = 3.0f;

            // 소지한 공격 스킬 중 무작위로 하나를 고른다.
            int curAttackIndex = UnityEngine.Random.Range(1, _attackSkillCount + 1);

            switch (curAttackIndex)
            {
                case 0:
                    _unityAction = null;
                    break;
                case 1:
                    _unityAction = Attack01;
                    break;
                case 2:
                    _unityAction = Attack02;
                    break;
                case 3:
                    _unityAction = Attack03;
                    break;
                default:
                    _unityAction = null;
                    break;
            }

            // 코루틴을 사용하여, 일정 주기마다 스킬을 달리하여 공격한다.
            if (!isCorRunning)
            {
                StartCoroutine(SetDelay(changeTime, _unityAction, true));
            }

            Debug.Log("Enemy's Attack() is Called.");
        }

        public virtual void Attack01()
        {
            _animator.SetInteger("AttackIndex", 1);
        }

        public virtual void Attack02()
        {
            _animator.SetInteger("AttackIndex", 2);
        }

        public virtual void Attack03()
        {
            _animator.SetInteger("AttackIndex", 3);
        }

        #endregion Action Methods
    }
}
