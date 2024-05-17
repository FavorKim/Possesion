using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class Projectile : MonoBehaviour
    {
        #region Components

        // 컴포넌트(Components)
        private Rigidbody rb;

        #endregion Components

        #region Fields

        // 필드(Fields)
        private IObjectPool<Projectile> objectPool;

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
            StartCoroutine(Shoot());
        }

        // 비활성화되었을 때(풀에 돌려놓을 때)의 함수
        private void OnDisable()
        {
            // 아마 필요 없을 듯?
        }

        #endregion OnEnable() / Disable()

        // 오브젝트 풀의 참조를 받아온다.
        public void GetObjectPool(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        #region Coroutines

        private IEnumerator Shoot()
        {
            // 투사체에 앞으로 힘을 가한다.
            rb.AddForce(Vector3.forward * shootPower, ForceMode.Impulse);

            // 약 3초 후,
            yield return new WaitForSeconds(3.0f);

            // 이 투사체를 제거한다.
            objectPool.Release(this);
        }

        #endregion Coroutines
    }

}
