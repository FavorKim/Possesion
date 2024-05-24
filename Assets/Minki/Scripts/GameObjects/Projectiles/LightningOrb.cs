using ObjectPool;
using UnityEngine;

public class LightningOrb : Projectile
{
    #region Components

    // 컴포넌트(Components)

    // AudioSource
    private AudioSource audioSource;

    // ParticleSystem
    [SerializeField] private ParticleSystem _missile; // 투사체의 본체
    [SerializeField] private ParticleSystem _explosion; // 투사체가 터질 때 1
    [SerializeField] private ParticleSystem _muzzle; // 투사체가 터질 때 2

    #endregion Components

    #region Fields

    // 회수 전까지 중복 타격을 막기 위한 변수
    private bool isAlreadyHit = false;

    #endregion Fields

    #region Awake()

    protected override void Awake()
    {
        base.Awake();

        // 컴포넌트를 초기화한다.
        audioSource = GetComponent<AudioSource>();

        _missile = Instantiate(_missile, transform).GetComponent<ParticleSystem>();
        _explosion = Instantiate(_explosion, transform).GetComponent<ParticleSystem>();
        _muzzle = Instantiate(_muzzle, transform).GetComponent<ParticleSystem>();
    }

    #endregion Awake()

    #region OnEnable() / OnDisable()

    private void OnEnable()
    {
        // 타격 여부를 초기화한다.
        isAlreadyHit = false;

        // 생성 시 투사체 기본 효과를 재생한다.
        _missile.Play();
    }

    #endregion Enable() / Disable()

    #region Collision Events

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌할 경우,
        if (other.CompareTag("Player") && !isAlreadyHit)
        {
            // 타격했다.
            isAlreadyHit = true;

            // 움직임을 멈춘다.
            // _rigidbody.velocity = Vector3.zero;

            //// 터지는 효과를 재생한다.
            _missile.Stop();
            _explosion.Play();
            _muzzle.Play();

            // 일정 시간 후, 삭제한다.
            StartCoroutine(DelayedDestroy());
        }
    }

    #endregion Collision Events
}
