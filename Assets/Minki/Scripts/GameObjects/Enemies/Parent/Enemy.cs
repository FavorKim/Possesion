using System.Collections;
using System.Collections.Generic;
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

        #endregion Components

        #region Fields

        // 필드(Fields)
        //[SerializeField] private Transform[] _patrolTransforms; // 순찰하는 위치들(Transform)
        //public Transform[] PatrolTransforms { get { return _patrolTransforms; } }

        [SerializeField] private List<Transform> _patrolTransforms; // 순찰하는 위치들
        public List<Transform> PatrolTransforms { get { return _patrolTransforms; } }

        private Transform _playerTransform;

        public bool IsPossessed { get; set; } // 빙의 상태를 판별하는 변수
        public bool IsGetHit { get; set; } // 피격을 판별하는 변수

        #region Enemy Stats

        // 필드(변수); 적의 스탯 정보
        public int AttackSkillCount { get; protected set; } // 공격 기술 개수

        public string Name { get; protected set; } // Name; 이름
        
        public int HealthPoint { get; protected set; } // HP; 체력
        public int MagicPoint { get; protected set; } // MP; 마력  
        
        public float MoveSpeed { get; protected set; } // SPD; 이동 속도
        public float JumpSpeed { get; protected set; } // J_SPD; 점프 속도

        public int AttackDamage { get; protected set; } // ATK; 기본 공격력
        public int Skiil1Damage { get; protected set; } // SK1; 스킬1 공격력
        public int Skill2Damage { get; protected set; } // SK2; 스킬2 공격력
        
        public float AttackCoolTime { get; protected set; } // ATK_CD; 공격 재사용 대기 시간
        public float Skill1CoolTime { get; protected set; } // SK1_CD; 스킬1 재사용 대기 시간
        public float Skill2CoolTime { get; protected set; } // SK2_CD; 스킬2 재사용 대기 시간

        public float AttackRange { get; protected set; } // ATK_Range; 공격 범위
        public float DetectRange { get; protected set; } // DTC_Range; 플레이어 탐지 범위

        #endregion Enemy Stats

        #endregion Fields

        #region Awake()

        protected override void Awake()
        {
            // 행동 트리의 뿌리(Root)를 생성한다.
            gameObject.AddComponent<EnemyBT>();

            // 필요한 컴포넌트들을 초기화한다.
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            // NavMeshAgent의 속성 값을 조절한다.
            _navMeshAgent.speed = MoveSpeed; // 추적 속도를 이동 속도로 지정한다.
            _navMeshAgent.stoppingDistance = AttackRange; // 정지 거리를 공격 범위로 지정한다.

            // 보스 기믹 전용, 순찰 지점이 따로 없이 바로 캐릭터에게 돌진한다.
            if (_patrolTransforms.Count == 0)
            {
                _playerTransform = FindObjectOfType<PlayerController>().transform;
                _patrolTransforms.Add(_playerTransform);
            }
        }

        #endregion Awake()

        #region Collision Events

        // 플레이어에게 피격(충돌) 시의 처리 함수
        private void OnTriggerEnter(Collider other)
        {
            // 플레이어가 던진 모자에 맞았을 경우,
            if (other.CompareTag("Hat"))
            {
                // 빙의 상태가 된다.
                IsPossessed = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // 공격(장애물, 몬스터의 공격 등)에 맞았을 경우, 무적 상태가 아니라면
            if (other.GetComponent<Obstacles>() && !isInvincible)
            {
                // 피격 상태가 된다.
                IsGetHit = true;
            }
        }

        #endregion Collision Events

        #region Abstract Methods

        // 스탯을 초기화하는 함수, 각 자식 클래스에서 알맞게 수치를 조정하여 초기화한다.
        protected abstract void InitializeStats();

        #endregion Abstract Methods

        #region Action Methods

        // 적의 행동을 다루는 함수들, 부모 클래스에서 공통된 속성을 지정하고, 자식 클래스에서 각 특성에 맞게 추가한다.
        public override void Attack()
        {
            _animator.SetInteger("AttackIndex", 0);
            _animator.SetTrigger("Attack");
        }

        public override void Skill1()
        {
            _animator.SetInteger("AttackIndex", 1);
            _animator.SetTrigger("Attack");
        }

        public override void Skill2()
        {
            _animator.SetInteger("AttackIndex", 2);
            _animator.SetTrigger("Attack");
        }

        public override void GetDamage(int damage)
        {
            base.GetDamage(damage);

            IsGetHit = true;
        }

        #endregion Action Methods
    }
}
