using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Enemy
{
    // 드래곤 클래스
    public class Dragon : Enemy
    {
        // 필드(Fields)
        [SerializeField] private ParticleSystem _particleSystem; // 공격 모션 중 불을 뿜는 공격을 구현하기 위한 파티클 시스템

        protected override void Awake()
        {
            // _particleSystem = GetComponentInChildren<ParticleSystem>();

            // 스탯을 초기화한다.
            InitializeStats();

            base.Awake();
        }

        // 적(Enemy)의 기본 스탯을 초기화하는 함수
        protected override void InitializeStats()
        {
            Name = "Dragon";
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

            _particleSystem.Stop();

            Debug.Log("Dragon's Patrol!");
            Debug.Log("Dragon's AttackRange = " + AttackRange);
        }

        public override void Chase()
        {
            base.Chase();

            _particleSystem.Stop();

            Debug.Log("Dragon's Chase!");
        }

        public override void Attack()
        {
            base.Attack();

            Debug.Log("Dragon's Attack!");
        }

        private void OnPlayFire()
        {
            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }

        private void OnStopFire()
        {
            _particleSystem.Stop();
        }
    }
}
