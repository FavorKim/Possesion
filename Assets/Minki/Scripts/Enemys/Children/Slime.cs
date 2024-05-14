using UnityEngine;

namespace Enemy
{
    public class Slime : Enemy
    {
        protected override void Awake()
        {
            // 스탯을 초기화한다.
            InitializeStats();

            base.Awake();
        }

        // 적의 기본 스탯을 초기화하는 함수
        protected override void InitializeStats()
        {
            _healthPoint = 100;
            _magicPoint = 100;
            _attackDamage = 100;
            _skill2Damage = 100;
            _skill2Damage = 100;
            // _defendPoint = 100;
            _moveSpeed = 100;
            _jumpSpeed = 100;
            _skillCoolTime = 100;

            _attackRange = 2.0f;
            _detectRange = 5.0f;

            _name = "슬라임";
        }

        public override void Patrol()
        {
            base.Patrol();

            Debug.Log("Smile's Patrol!");
            Debug.Log("Smile's AttackRange = " + _attackRange);
        }

        public override void Chase()
        {
            base.Chase();

            Debug.Log("Smile's Chase!");
        }

        public override void Attack()
        {
            base.Attack();

            Debug.Log("Smile's Attack!");
        }
    }
}
