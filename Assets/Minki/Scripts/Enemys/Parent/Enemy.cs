using UnityEngine;

namespace Enemy
{
    // 적(몬스터)를 정의하는 부모 클래스
    public abstract class Enemy : MonoBehaviour
    {
        // 적의 공통된 정보를 부모 클래스에서 우선 정의한다.

        // 행동 트리
        protected EnemyBT _enemyBT;

        // 컴포넌트
        public CharacterController _characterController { get; private set; }
        private Animator _animator; // 적 자신의 애니메이터
        [SerializeField] private Transform _playerTransform; // 플레이어의 위치 값 
        public Transform _enemyTransform { get; private set; }

        // 필드(변수); 적의 스탯 정보
        public int _healthPoint { get; protected set; } // HP; 체력
        public int _magicPoint { get; protected set; } // MP; 마력
        public int _attackDamage { get; protected set; } // ATK; 기본 공격력
        public int _skiil1Damage { get; protected set; } // SK1; 스킬1 공격력
        public int _skill2Damage { get; protected set; } // SK2; 스킬2 공격력
        // public int _defendPoint { get; protected set; } // DEF; 방어력
        public float _moveSpeed { get; protected set; } // SPD; 이동 속도
        public float _jumpSpeed { get; protected set; } // J_SPD; 점프 속도
        public float _skillCoolTime { get; protected set; } // CT; 스킬 재사용 대기시간

        public float _attackRange { get; protected set; } // ATK_Range; 공격 범위
        public float _detectRange { get; protected set; } // DTC_Range; 플레이어 탐지 범위

        public string _name { get; protected set; } = "몬스터 상위";

        // Get 함수 목록
        public Transform GetPlayerTransform() { return _playerTransform; }

        protected virtual void Awake()
        {
            _enemyBT = gameObject.AddComponent<EnemyBT>();
            
            //_characterController = GetComponent<CharacterController>();
            //_animator = GetComponent<Animator>();
            _enemyTransform = GetComponent<Transform>();
        }

        // 스탯을 초기화하는 함수, 각 자식 클래스에서 알맞게 수치를 조정하여 초기화한다.
        protected abstract void InitializeStats();


        // 적의 행동을 다루는 함수들
        public virtual void Patrol()
        {
            //_animator.SetBool("Patrol", true);
            //_animator.SetBool("Chse", false);
            //_animator.SetBool("Attack", false);

            Debug.Log("Enemy's Patrol() is Called.");
        }

        public virtual void Chase()
        {
            //_animator.SetBool("Patrol", false);
            //_animator.SetBool("Chase", true);
            //_animator.SetBool("Attack", false);

            transform.position = Vector3.MoveTowards(_enemyTransform.position, _playerTransform.position, 0.01f);

            Debug.Log("Enemy's Chase() is Called.");
        }

        public virtual void Attack()
        {
            // 공격 애니메이션을 여러 개 준비하여, 무작위로 적용시킨다. (애니메이션 블렌드 사용?)

            //_animator.SetBool("Patrol", false);
            //_animator.SetBool("Chase", false);
            //_animator.SetBool("Attack", true);

            Debug.Log("Enemy's Attack() is Called.");
        }
    }
}
