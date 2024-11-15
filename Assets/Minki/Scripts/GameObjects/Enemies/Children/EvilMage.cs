using ObjectPool;
using UnityEngine;

namespace Enemy
{
    // 마법사 클래스
    public class EvilMage : Enemy
    {
        #region Fields

        // 필드(Fields)
        [SerializeField] private Transform projectileTransform; // 투사체를 발사하는 위치

        private ProjectilePool projectilePool; // 투사체를 구현하기 위한 오브젝트 풀링
        private Projectile projectile; // 투사체 게임 오브젝트

        [SerializeField] private float shootPower = 50.0f; // 투사체를 쏘는 힘의 값

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

            AttackSkillCount = 2;

            HealthPoint = 100;
            MagicPoint = 100;
            MoveSpeed = 2;
            JumpSpeed = 0;
            AttackDamage = 100;
            Skiil1Damage = 100;
            Skill2Damage = 0;
            AttackCoolTime = 1.0f;
            Skill1CoolTime = 1.0f;
            Skill2CoolTime = 1.0f;
            AttackRange = 19.0f;
            DetectRange = 20.0f;

            InitSkill(Skill1CoolTime);
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

        // 원거리 뇌전 공격(Skill1)의 첫 번째 이벤트 함수 (기를 모으는 애니메이션)
        private void OnSkill1Event1()
        {
            projectile = projectilePool.OnReadyToShoot(projectileTransform);
        }

        // 원거리 뇌전 공격(Skill2)의 두 번째 이벤트 함수 (발사하는 애니메이션)
        private void OnSkill1Event2()
        {
            projectile?.Shoot();
        }

        // 공격 중 피격했을 경우, 투사체를 삭제한다.
        private void OnCancelAttack()
        {
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }

        #endregion Animation Events
    }
}
