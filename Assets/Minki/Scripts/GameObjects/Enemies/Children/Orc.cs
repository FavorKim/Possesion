using UnityEngine;

namespace Enemy
{
    // 오크 클래스
    public class Orc : Enemy
    {
        protected override void Awake()
        {
            // 스탯을 초기화한다.
            InitializeStats();

            base.Awake();
        }

        // 적(Enemy)의 기본 스탯을 초기화하는 함수
        protected override void InitializeStats()
        {
            Name = "Orc";

            _attackSkillCount = 2;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 4;
            JumpSpeed = 0;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 0;
            AttackCoolTime = 100.0f;
            Skill1CoolTime = 100.0f;
            Skill2CoolTime = 0.0f;
            AttackRange = 3.0f;
            DetectRange = 5.0f;

            InitSkill(AttackCoolTime, Skill1CoolTime);
        }

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Patrol()
        {
            base.Patrol();

            Debug.Log("Orc's Patrol!");
        }

        public override void Chase()
        {
            base.Chase();

            Debug.Log("Orc's Chase!");
        }

        public override void AttackAI()
        {
            base.AttackAI();

            Debug.Log("Orc's Attack!");
        }

        public override void Attack()
        {
            base.Attack();
        }

        public override void Skill1()
        {
            base.Skill1();
        }

        public override void Skill2()
        {
            // 세 번째 공격 스킬이 존재하지 않는다.
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        #endregion Animation Events
    }
}
