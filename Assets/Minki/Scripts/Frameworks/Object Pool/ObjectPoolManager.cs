using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    // 오브젝트 풀링을 관리하는 클래스
    public abstract class ObjectPoolManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        // IObjectPool; 유니티에서 지원하는 오브젝트 풀링 인터페이스
        protected IObjectPool<T> objectPool;

        // 오브젝트 풀링에 사용할 게임 오브젝트 프리팹
        [SerializeField] protected GameObject poolPrefab;

        private void Awake()
        {
            objectPool = new ObjectPool<T>(CreatePrefab);
        }

        // 새로운 오브젝트를 생성하는 함수
        protected abstract T CreatePrefab();
        /*
        {
            T obj = Instantiate(poolPrefab).GetComponent<T>();
            obj.GetObjectPool(objectPool);
            return obj;
        }
        */

        // 풀에서 오브젝트를 가져오는 함수
        private void OnGetPrefab(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        // 풀에 오브젝트를 돌려놓는 함수
        private void OnReleasePrefab(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        // 풀에 돌려놓지 않는 오브젝트를 파괴하는 함수
        private void OnDestroyPrefab(T obj)
        {
            Destroy(obj.gameObject);
        }
    }
}
