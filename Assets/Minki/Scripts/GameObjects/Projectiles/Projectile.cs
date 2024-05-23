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
        protected Rigidbody _rigidbody;

        #endregion Components

        #region Fields

        // 필드(Fields)

        // 오브젝트 풀
        private IObjectPool<Projectile> objectPool;

        // 투사체의 속도
        [SerializeField] private float shootPower = 100.0f;

        #endregion Fields

        #region Awake()

        protected virtual void Awake()
        {
            // 컴포넌트를 초기화한다.
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion Awake()

        // 오브젝트 풀의 참조를 받아온다.
        public void SetObjectPool(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }

        #region Action Methods

        // 투사체를 발사하는 함수
        public virtual void Shoot()
        {
            // 투사체 게임 오브젝트를 최상위 계층으로 옮긴다.
            transform.SetParent(null);

            // 투사체에 앞으로 힘을 가한다.
            _rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * shootPower, ForceMode.Impulse);

            // 일정 시간 후, 삭제한다.
            StartCoroutine(DelayedDestroy());
        }

        #endregion Action Methods

        #region Coroutines

        // 일정 시간이 지난 후, 여전히 남아 있을 경우 회수하는 코루틴 함수
        protected IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(3.0f);

            if (this != null)
            {
                objectPool.Release(this);
            }
        }

        #endregion Coroutines
    }

}
