using DG.Tweening;
using UnityEngine;

public class Hat : MonoBehaviour
{
    #region Components

    // Rigidbody
    private Rigidbody rb;

    // DOTween 애니메이션
    private DOTweenAnimation dotAnim;

    // 플레이어
    private TestPlayer playerController;

    // 파티클 시스템
    [SerializeField] private ParticleSystem hitParticle;
    // 오디오
    private AudioSource hitSFX;

    #endregion Components

    #region Fields

    // 모자 던지기의 강도
    [SerializeField] private float power;

    // Raycast
    private RaycastHit hit;

    #endregion Fields

    #region Life Cycle Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dotAnim = GetComponent<DOTweenAnimation>();
        
        playerController = FindAnyObjectByType<TestPlayer>();

        hitParticle = Instantiate(hitParticle);
        hitSFX = hitParticle.gameObject.GetComponent<AudioSource>();
    }

    // 활성화될 때 호출되는 함수
    private void OnEnable()
    {
        // 기존에 가해진 힘을 초기화하고, 다시 앞으로 힘을 가한다.
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * power, ForceMode.Impulse);

        dotAnim.DORestartAllById("Shoot");
    }

    #endregion Life Cycle Methods

    #region Unity Event Methods

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어나 카메라 이외의 것과 충돌할 경우에 한해,
        if (!other.CompareTag("Player") && !other.CompareTag("Camera"))
        {
            // 몬스터와 충돌했을 경우에는,
            if (other.GetComponentInParent<Monsters>() != null)
            {
                // 이펙트를 발생시킨다.
                hitParticle.transform.position = transform.position;
                hitParticle.Play();
                hitSFX.Play();

                // 빙의 상태에 돌입한다.
                playerController.SetState(other.GetComponent<TestMonster>());
            }

            // 각각 모자를 활성화, 비활성화한다.
            playerController.SetActiveHats(wears: true, throws: false);
        }
    }

    #endregion Unity Event Methods
}
