using ObjectPool;
using UnityEngine;

namespace Enemy
{
    // 골렘 클래스
    public class Golem : Enemy
    {
        #region Awake()

        protected override void Awake()
        {
            // 스탯을 초기화한다.
            InitializeStats();

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

            Debug.Log("Golem's Patrol!");
        }

        public override void Chase()
        {
            base.Chase();

            Debug.Log("Golem's Chase!");
        }

        public override void Attack()
        {
            base.Attack();

            Debug.Log("Golem's Attack!");
        }

        public override void Attack01()
        {
            base.Attack01();
        }

        public override void Attack02()
        {
            base.Attack02();
        }

        public override void Attack03()
        {
            // 세 번째 공격 스킬이 존재하지 않는다.
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        #endregion Animation Events
    }
}
