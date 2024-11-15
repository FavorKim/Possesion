using UnityEngine;
using UnityEngine.InputSystem;

namespace ObjectPool
{
    // 투사체를 오브젝트 풀링으로 관리하는 클래스
    public class ProjectilePool : ObjectPoolManager<Projectile>
    {
        // 이미 생성된 투사체가 없을 경우, 새롭게 생성(Instantiate)한다.
        protected override Projectile CreatePrefab()
        {
            Projectile obj = Instantiate(poolPrefab).GetComponent<Projectile>();
            obj.SetObjectPool(objectPool);

            return obj;
        }

        // 입력 없이 투사체를 생성(풀에서 가져오기)한다. (AI)
        public Projectile OnReadyToShoot(Transform transform)
        {
            Projectile projectile = objectPool.Get();

            // 위치를 조정한다.
            projectile.transform.SetParent(transform);
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;

            projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;

            return projectile;
        }

        // 입력을 받아 투사체를 생성(풀에서 가져오기)한다. (빙의)
        public void OnShoot(InputAction.CallbackContext context)
        {
            objectPool.Get();
        }
    }
}
