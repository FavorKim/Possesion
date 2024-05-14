using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class Projectile : MonoBehaviour
    {
        private IObjectPool<Projectile> objectPool;

        /* OnEnable / Start / Update 등 활성화 때의 생명 주기에서 투사체의 움직임 등을 구현하고,
         * 특정 상황(충돌 등)에 objectPool.Release() 함수를 호출하여 풀에 되돌려 놓습니다. */

        // 활성화되었을 때(풀에서 가져올 때)의 함수
        private void OnEnable()
        {
            // 보통 투사체의 발사를 구현합니다. (AddForce 등)
        }

        // 비활성화되었을 때(풀에 돌려놓을 때)의 함수
        private void OnDisable()
        {
            // 아마 필요 없을 듯?
        }

        public void GetObjectPool(IObjectPool<Projectile> objectPool)
        {
            this.objectPool = objectPool;
        }
    }

}
