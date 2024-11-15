using UnityEngine;

namespace Enemy
{
    // 슬라임 클래스
    public class Slime : Enemy
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
            Name = "Slime";

            AttackSkillCount = 2;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 1;
            JumpSpeed = 20;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 0;
            AttackCoolTime = 1.0f;
            Skill1CoolTime = 1.0f;
            Skill2CoolTime = 1.0f;
            AttackRange = 2.0f;
            DetectRange = 5.0f;

            InitSkill(Skill1CoolTime);
        }

        #endregion Initialize Methods

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Attack()
        {
            // 거리가 충분하지 않을 경우, Skill1을 사용하게 한다.
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance > 1.5f)
            {
                Skill1();
            }
            else
            {
                base.Attack();
            }
        }

        public override void Skill1()
        {
            base.Skill1();
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        #endregion Animation Events
    }
}
