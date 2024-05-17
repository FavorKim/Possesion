using ObjectPool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Enemy
{
    // 마법사 클래스
    public class EvilMage : Enemy
    {
        #region Fields

        // 필드(Fields)
        private ProjectilePool projectilePool; // 투사체를 구현하기 위한 오브젝트 풀링

        #endregion Fields

        #region Awake()

        protected override void Awake()
        {
            // 스탯을 초기화한다.
            InitializeStats();

            // 오브젝트 풀을 가져온다.
            projectilePool = GetComponent<ProjectilePool>();

            base.Awake();
        }

        #endregion Awake()

        #region Initialize Methods

        // 적(Enemy)의 기본 스탯을 초기화하는 함수
        protected override void InitializeStats()
        {
            Name = "EvilMage";

            _attackSkillCount = 2;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 100;
            JumpSpeed = 100;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 100;
            Skill3Damage = 100; // Attack03이 존재하지 않는다.
            SkillCoolTime = 100;
            AttackRange = 3.0f;
            DetectRange = 5.0f;
        }

        #endregion Initialize Methods

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Patrol()
        {
            base.Patrol();

            Debug.Log("EvilMage's Patrol!");
        }

        public override void Chase()
        {
            base.Chase();

            Debug.Log("EvilMage's Chase!");
        }

        public override void Attack()
        {
            base.Attack();

            Debug.Log("EvilMage's Attack!");
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        // 원거리 뇌전 공격(Attack02)의 첫 번째 이벤트 함수
        private void OnAttack02Event01()
        {
            projectilePool.OnShoot();
        }

        // 원거리 뇌전 공격(Attack02)의 두 번째 이벤트 함수
        private void OnAttack02Event02()
        {
            
        }

        #endregion Animation Events
    }
}
