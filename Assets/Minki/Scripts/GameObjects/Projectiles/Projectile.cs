using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class Projectile : MonoBehaviour
    {
        #region Components

        // 컴포넌트(Components)

        // Rigidbody
        private Rigidbody rb;

        // AudioSource
        private AudioSource audioSource;

        #endregion Components

        #region Fields

        // 필드(Fields)

        // 오브젝트 풀
        private IObjectPool<Projectile> objectPool;

        // 투사체의 속도
        [SerializeField] private float shootPower = 100.0f;

        #endregion Fields

        #region Awake()

        private void Awake()
        {
            // 컴포넌트를 초기화한다.
            rb = GetComponent<Rigidbody>();
        }

        #endregion Awake()

        #region OnEnable() / Disable()

        // 활성화되었을 때(풀에서 가져올 때)의 함수
        private void OnEnable()
        {

        }

        // 비활성화되었을 때(풀에 돌려놓을 때)의 함수
        private void OnDisable()
        {
            // 아마 필요 없을 듯?
        }

        #endregion OnEnable() / Disable()

        // 오브젝트 풀의 참조를 받아온다.
        public void SetObjectPool(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        #region Coroutines

        public IEnumerator Shoot()
        {
            // 투사체 게임 오브젝트를 최상위 계층으로 옮긴다.
            //transform.SetParent(null);

            // 투사체에 앞으로 힘을 가한다.
            rb.AddForce(transform.TransformDirection(Vector3.forward) * shootPower, ForceMode.Impulse);

            // 발사 시 효과음을 재생한다.
            audioSource.Play();

            // 약 3초 후,
            yield return new WaitForSeconds(3.0f);

            // 이 투사체를 제거한다.
            objectPool.Release(this);
        }

        #endregion Coroutines
    }

}
