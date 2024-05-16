using UnityEngine;

namespace Enemy
{
    // 거미 클래스
    public class Spider : Enemy
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
            Name = "Spider";
            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 100;
            JumpSpeed = 100;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 100;
            SkillCoolTime = 100;
            AttackRange = 3.0f;
            DetectRange = 5.0f;
        }

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Patrol()
        {
            base.Patrol();

            Debug.Log("Spider's Patrol!");
            Debug.Log("Spider's AttackRange = " + AttackRange);
        }

        public override void Chase()
        {
            base.Chase();

            Debug.Log("Spider's Chase!");
        }

        public override void Attack()
        {
            base.Attack();

            Debug.Log("Spider's Attack!");
        }
    }
}
