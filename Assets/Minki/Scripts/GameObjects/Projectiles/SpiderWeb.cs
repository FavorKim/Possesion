using ObjectPool;
using UnityEngine;

public class SpiderWeb : Projectile
{
    #region Components

    // 컴포넌트(Components)

    // AudioSource
    private AudioSource audioSource;

    // Mesh (Prefab)
    [SerializeField] private GameObject _webPrefab;

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
    }

    #endregion Awake()

    #region OnEnable() / OnDisable()

    private void OnEnable()
    {
        // 타격 여부를 초기화한다.
        isAlreadyHit = false;
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
            _rigidbody.velocity = Vector3.zero;

            transform.parent = other.transform;

            // 일정 시간 후, 삭제한다.
            StartCoroutine(DelayedDestroy());
        }
    }

    #endregion Collision Events
}
