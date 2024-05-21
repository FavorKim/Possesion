using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemy
{
    // 적(몬스터)를 정의하는 부모 클래스
    public abstract class Enemy : Monsters
    {
        // 적의 공통된 정보를 부모 클래스에서 우선 정의한다.

        #region Components

        // 컴포넌트(Components)
        private Animator _animator; // 적 자신의 애니메이터(Animator)
        private NavMeshAgent _navMeshAgent; // 플레이어를 추적하기 위한 네비게이션(NavMesh)
        [SerializeField] private RuntimeAnimatorController _poAnimator; // 빙의 시 애니메이터

        [SerializeField] private RuntimeAnimatorController _AIAnimator; /// 일반 애니메이터

        [SerializeField] private Transform[] patrolTransforms; // 순찰하는 위치들(Transform)
        public Transform _playerTransform { get; private set; } // 플레이어의 위치(Transform)

        #endregion Components

        #region Fields

        // 필드(Fields)

        // 애니메이터를 AI용, 빙의용으로 2개 생성하여 교환할 수 있도록 수정할 것.

        private int _patrolIndex = 0; // 순찰할 때 현재 이동할 위치의 순서

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
        private UnityAction _unityAction;
        private bool isCorRunning = false; // 코루틴의 중복 실행을 방지하기 위한 변수

        #endregion Delegates

        #region Awake()

        protected override void Awake()
        {
            base.Awake();

            // 행동 트리의 뿌리(Root)를 생성한다.
            gameObject.AddComponent<EnemyBT>();

            // 필요한 컴포넌트들을 초기화한다.
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            // 현재 Scene에서 "PlayerController" 스크립트를 가진 게임 오브젝트(= 플레이어)를 찾는다.
            _playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();

            /// 일반 애니메이터 저장
            _AIAnimator = _animator.runtimeAnimatorController;
        }

        #endregion Awake()

        #region Collision Events
        
        /* //플레이어에게 피격(충돌) 시의 처리 함수
        private void OnCollisionEnter(Collision collision)
        {
            // 플레이어의 모자와 충돌했을 경우,
            if (collision.collider.CompareTag("Hat"))
            {
                // 빙의 상태가 된다.
                IsPossessed = true;
            }
        }*/

        ///Trigger로 변경
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hat"))
            {
                IsPossessed = true;

                /// 빙의 시 한번만 호출
                OnPossessed();
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

        // 빙의를 담당하는 함수 (빙의 중일 때 지속적으로 호출)
        public virtual void BeingPossessed()
        {
            // 모든 애니메이션을 초기화한다.
            _animator.SetBool("Patrol", false);
            _animator.SetBool("LookAround", false);
            _animator.SetBool("Chase", false);
            _animator.SetInteger("AttackIndex", 0);
            
            
            /// 애니메이션 초기화 함수 단, 매 프레임마다 호출될 것
            _animator.Rebind();

            //Debug.Log("Enemy's BeingPossessed() is Called.");
        }

        /// 빙의됐을 때 한 번 만 호출되는 함수
        private void OnPossessed()
        {
            /// 애니메이션 초기화 함수
            _animator.Rebind();

            /// 1안. 애니메이터를 추가하고, 빙의시 애니메이터를 교체하는 방식
            if (_poAnimator != null)
                _animator.runtimeAnimatorController = _poAnimator;
            /// 빙의 해제 시 애니메이터 원래대로 설정할 것
            else
            /// 2안. 기존 애니메이터를 사용하고, 애니메이션이 Attack01, Attack02로 전이할 수 있는 Chase상태를 유지시키는 방식
            _animator.SetBool("Chase", true);

            _navMeshAgent.ResetPath();

         
            ///1안의 경우 : 모든 몬스터에 각각 애니메이터를 생성한다. 빙의 시작과 빙의 끝에서 애니메이터 변경 함수를 호출한다.
            ///2안의 경우 : 모든 몬스터가 빙의 중에 Chase 상태의 애니메이션으로 고정된다. (움직이지 않아도 계속 걸어다닐 것)
        }

        /// 빙의 탈출 시 한번만 호출되는 함수
        public void OffPossess()
        {
            /// 원래 애니메이터로 돌아오기
            _animator.runtimeAnimatorController = _AIAnimator;
        }

        /// 빙의 애니메이터의 이벤트 함수
        public void ResetAttackIndex()
        {
            _animator.SetInteger("AttackIndex", 0);
        }

        // 피격을 담당하는 함수
        public virtual void GetHit()
        {
            // 피격 애니메이션을 재생한다.
            _animator.SetTrigger("GetHit");

            // Debug.Log("Enemy's GetHit() is Called.");
        }

        // 죽음을 담당하는 함수
        public virtual void Die()
        {
            // 죽음 애니메이션을 재생한다.
            _animator.SetTrigger("Die");

            // 게임 오브젝트를 비활성화한다. 단, 죽음 애니메이션이 끝난 후 호출해야 하므로 일정 시간 여유를 둔다.
            StartCoroutine(SetDelay(3.0f, () => gameObject.SetActive(false), isCallFirst: false)); // 약 3.0초 후 호출한다.

            // Debug.Log("Enemy's Die() is Called.");
        }

        // 순찰을 구현하는 함수
        public virtual void Patrol()
        {
            Debug.Log("Pat");
            // 순찰의 종류
            
            // 1. 가만히 서서 주변을 둘러본다.
            UnityAction lookAround = () =>
            {
                // 주변을 둘러보는 애니메이션을 재생한다.
                _animator.SetBool("LookAround", true);
                _animator.SetBool("Patrol", false);
                _animator.SetBool("Chase", false);

                // 다음 순찰 위치를 지정한다.
                _patrolIndex = ++_patrolIndex % patrolTransforms.Length;
            };

            // 2. 일정 구역을 돌아다닌다.
            UnityAction patrol = () =>
            {
                // 돌아다니는 애니메이션을 재생한다.
                _animator.SetBool("Patrol", true);
                _animator.SetBool("LookAround", false);
                _animator.SetBool("Chase", false);

                // 정해진 순찰 구역으로 이동한다.
                _navMeshAgent.SetDestination(patrolTransforms[_patrolIndex].position);
            };

            // 기본적으로, (그리고 주변을 둘러보는 함수가 실행 중이지 않을 때,)
            if (!isCorRunning)
            {
                // 돌아다니는 애니메이션을 재생한다.
                _animator.SetBool("Patrol", true);
                _animator.SetBool("LookAround", false);
                _animator.SetBool("Chase", false);

                // 정해진 순찰 구역으로 이동한다.
                _navMeshAgent.SetDestination(patrolTransforms[_patrolIndex].position);
            }

            // 순찰 목적지까지 이동했다면,
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !isCorRunning)
            {
                // 잠시 주변을 둘러본다. (약 5.0초간)
                StartCoroutine(SetDelay(5.0f, lookAround, isCallFirst: true));
            }
        }

        // 추적을 구현하는 함수
        public virtual void Chase()
        {
            Debug.Log("chase");

            // 추적 애니메이션을 재생한다.
            _animator.SetBool("Chase", true);
            _animator.SetInteger("AttackIndex", 0);

            // → NavMesh로 재구현할 것.
            _navMeshAgent.SetDestination(_playerTransform.position);

            // Debug.Log("Enemy's Chase() is Called.");
        }

        

        // 공격을 구현하는 함수
        public virtual void AIAttack()  /// Attack -> AIAttack으로 이름 변경
        {
            // 공격 스킬을 바꿀 주기
            float changeTime = 1.0f;

            // 소지한 공격 스킬 중 무작위로 하나를 고른다.
            int curAttackIndex = UnityEngine.Random.Range(1, _attackSkillCount + 1);

            switch (curAttackIndex)
            {
                case 0:
                    _unityAction = null;
                    break;
                case 1:
                    _unityAction = Attack;
                    break;
                case 2:
                    _unityAction = Skill1;
                    break;
                case 3:
                    _unityAction = Skill2;
                    break;
                default:
                    _unityAction = null;
                    break;
            }

            // 코루틴을 사용하여, 일정 주기마다 스킬을 달리하여 공격한다.
            if (!isCorRunning)
            {
                StartCoroutine(SetDelay(changeTime, _unityAction, isCallFirst: true));
            }

            // Debug.Log("Enemy's AIAttack() is Called.");
        }

        public override void Attack()   /// Attack01 -> Attack으로 이름 변경
        {
            _animator.SetInteger("AttackIndex", 1);
        }
        
        public override void Skill1()   /// Attack02 -> Skill1로 이름 변경
        {
            _animator.SetInteger("AttackIndex", 2);
        }

        public override void Skill2()   /// Attack03 -> Skill2로 이름 변경
        {
            _animator.SetInteger("AttackIndex", 3);
        }

        #endregion Action Methods
    }
}
