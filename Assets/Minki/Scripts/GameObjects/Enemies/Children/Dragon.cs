using UnityEngine;

namespace Enemy
{
    // 드래곤 클래스
    public class Dragon : Enemy
    {
        #region Fields

        // 필드(Fields)
        [SerializeField] private ParticleSystem _fireParticle; // 공격 모션 중 원거리 화염 공격을 구현하기 위한 파티클 시스템

        #endregion Fields

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
            Name = "Dragon";

            _attackSkillCount = 2;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 5;
            JumpSpeed = 5;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 0;
            AttackCoolTime = 1.0f;
            Skill1CoolTime = 1.0f;
            Skill2CoolTime = 1.0f;
            AttackRange = 3.0f;
            DetectRange = 10.0f;

            InitSkill(Skill1CoolTime, Skill2CoolTime);
        }

        #endregion Initialize Methods

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Patrol()
        {
            base.Patrol();

            _fireParticle.Stop();

            Debug.Log("Dragon's Patrol!");
        }

        public override void Chase()
        {
            base.Chase();

            _fireParticle.Stop();

            Debug.Log("Dragon's Chase!");
        }

        public override void AttackAI()
        {
            // 플레이어와의 거리가 1번 공격의 사정 범위보다 멀 경우, 2번 공격만 시전한다.
            if (_navMeshAgent.remainingDistance > 3.0f)
            {
                Skill1();
            }
            else // 그 외에는 무작위로 공격한다.
            {
                base.AttackAI();
            }

            Debug.Log("Dragon's Attack!");
        }

        public override void Attack()
        {
            base.Attack();
        }

        public override void Skill1()
        {
            base.Skill1();

            // 도트 대미지를 구현할 것인가?
            // 상태 이상에 대해서 별도의 코드를 작성하는 것이 바람직해 보인다.
        }

        public override void Skill2()
        {
            // 세 번째 공격 스킬이 존재하지 않는다.
        }

        #endregion Action Methods

        #region Animation Events

        // 아래는 애니메이션(Animation) 클립에서 이벤트를 추가하여 호출하는 함수들이다.

        // 원거리 화염 공격(Skill1)의 첫 번째 이벤트 함수
        private void OnSkill1Event1()
        {
            _fireParticle.Play();
        }

        // 원거리 화염 공격(Attack02)의 두 번째 이벤트 함수
        private void OnSkill1Event2()
        {
            _fireParticle.Stop();
        }

        #endregion Animation Events
    }
}
