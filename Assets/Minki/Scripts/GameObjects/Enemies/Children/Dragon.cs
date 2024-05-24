using UnityEngine;

namespace Enemy
{
    // 드래곤 클래스
    public class Dragon : Enemy
    {
        #region Fields

        // 필드(Fields)
        [SerializeField] private ParticleSystem _fireParticle; // 공격 모션 중 원거리 화염 공격을 구현하기 위한 파티클 시스템
        [SerializeField] private SphereCollider _sphereCollider; // 몸통을 감싸는 충돌체

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

            AttackSkillCount = 2;

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
            AttackRange = 5.0f;
            DetectRange = 10.0f;

            InitSkill(Skill2CoolTime);
        }

        #endregion Initialize Methods

        #region Action Methods

        // 적(Enemy)의 공통된 행동 함수를 재정의한다.
        public override void Attack()
        {
            // 거리가 충분하지 않을 경우, Skill1을 사용하게 한다.
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance > 3.0f)
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

        // 원거리 화염 공격(Skill1)의 첫 번째 이벤트 함수
        private void OnSkill1Event1()
        {
            // 화염 효과를 재생한다.
            _fireParticle.Play();
        }

        // 원거리 화염 공격(Skill1)의 두 번째 이벤트 함수
        private void OnSkill1Event2()
        {
            // 화염 효과를 중지한다.
            _fireParticle.Stop();
        }

        private void OnCancelAttack()
        {
            _fireParticle.Stop();
            _sphereCollider.isTrigger = false;
        }

        #endregion Animation Events
    }
}
