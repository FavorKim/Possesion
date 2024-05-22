namespace Enemy
{
    // 거미 클래스
    public class Spider : Enemy
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
            Name = "Spider";

            AttackSkillCount = 3;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 6;
            JumpSpeed = 0;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 100;
            AttackCoolTime = 100.0f;
            Skill1CoolTime = 100.0f;
            Skill2CoolTime = 100.0f;
            AttackRange = 3.0f;
            DetectRange = 5.0f;

            InitSkill(Skill1CoolTime, Skill2CoolTime);
        }

        #endregion Initialize Methods

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
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
            base.Skill2();
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        #endregion Animation Events
    }
}
