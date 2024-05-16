using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    // 적(몬스터)를 정의하는 부모 클래스
    public abstract class Enemy : MonoBehaviour
    {
        // 적의 공통된 정보를 부모 클래스에서 우선 정의한다.

        // 컴포넌트
        private Animator _animator; // 적 자신의 애니메이터
        private NavMeshAgent _navMeshAgent; // AI 추적 기능을 위한 NavMesh
        [SerializeField] private Transform _playerTransform; // 플레이어의 위치 값 
        public Transform _enemyTransform { get; private set; }

        // 필드(Fields)
        public bool _getHit { get; private set; } // 피격을 판별하는 변수
        public bool _isDead { get; private set; } // 죽음을 판별하는 변수

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
        public float SkillCoolTime { get; protected set; } // CT; 스킬 재사용 대기시간
        public float AttackRange { get; protected set; } // ATK_Range; 공격 범위
        public float DetectRange { get; protected set; } // DTC_Range; 플레이어 탐지 범위

        public bool IsPossessed { get; set; } // IsPoss; 빙의 여부
        #endregion Enemy Stats

        // Get 함수 목록
        public Transform GetPlayerTransform() { return _playerTransform; }

        protected virtual void Awake()
        {
            // Root를 생성한다.
            gameObject.AddComponent<EnemyBT>();

            // 필요한 컴포넌트들을 초기화한다.
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _enemyTransform = GetComponent<Transform>();
        }

        // 플레이어에게 피격(충돌) 시의 처리 함수
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                _getHit = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                _getHit = false;
            }
        }

        // 스탯을 초기화하는 함수, 각 자식 클래스에서 알맞게 수치를 조정하여 초기화한다.
        protected abstract void InitializeStats();

        #region Action Methods
        // 적의 행동을 다루는 함수들, 부모 클래스에서 공통된 속성을 지정하고, 자식 클래스에서 각 특성에 맞게 추가한다.

        // 피격을 담당하는 함수
        public virtual void GetHit()
        {
            // 피격 애니메이션을 재생한다.
            if (_getHit)
                _animator.SetTrigger("GetHit");
        }

        // 죽음을 담당하는 함수
        public virtual void Die()
        {
            // 죽음 애니메이션을 재생한다.
            _animator.SetTrigger("Die");

            // 게임 오브젝트를 비활성화한다.
            gameObject.SetActive(false);
        }

        // 순찰을 구현하는 함수
        public virtual void Patrol()
        {
            _animator.SetBool("Patrol", true);
            // _animator.SetBool("LookAround", false);
            _animator.SetBool("Chase", false);

            // 이동 방향을 바라보게 한다.
            // transform.LookAt(transform.TransformDirection(transform.forward));

            Debug.Log("Enemy's Patrol() is Called.");
        }

        // 추적을 구현하는 함수
        public virtual void Chase()
        {
            _animator.SetBool("Chase", true);
            _animator.SetInteger("AttackIndex", 0);

            // 플레이어를 바라보며 다가간다.
            //transform.LookAt(_playerTransform.position);
            //transform.position = Vector3.MoveTowards(_enemyTransform.position, _playerTransform.position, 0.01f);

            // → NavMesh로 재구현할 것.
            // if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            _navMeshAgent.SetDestination(_playerTransform.position);

            Debug.Log("Enemy's Chase() is Called.");
        }

        // 공격을 구현하는 함수
        public virtual void Attack()
        {
            // 공격 애니메이션을 여러 개 준비하여, 무작위로 적용시킨다. (애니메이션 블렌드 사용?)
            _animator.SetInteger("AttackIndex", Random.Range(1, 3)); // 1, 2

            Debug.Log("Enemy's Attack() is Called.");
        }
        #endregion Action Methods
    }
}
